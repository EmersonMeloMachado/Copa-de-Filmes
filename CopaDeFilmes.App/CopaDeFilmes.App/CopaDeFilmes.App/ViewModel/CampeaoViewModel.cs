using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CopaDeFilmes.App.Model;
using CopaDeFilmes.App.Service.Abstracts;
using CopaDeFilmes.App.Service.Concretes;
using Xamarin.Forms;

namespace CopaDeFilmes.App.ViewModel
{
    internal sealed class CampeaoViewModel : BaseViewModel
    {
        public override string Title => "Resultado";

        private readonly IFilmesService _iFilmesService;
        private ObservableCollection<Filmes> listaDeFilmes;
        public ObservableCollection<Filmes> ListaDosVencedores { get; }

        public CampeaoViewModel()
        {
            _iFilmesService = DependencyService.Get<IFilmesService>();
        }

        public CampeaoViewModel(ObservableCollection<Filmes> listaDeFilmesEscolhido)
        {
            _iFilmesService = DependencyService.Get<IFilmesService>();
            listaDeFilmes = new ObservableCollection<Filmes>();
            listaDeFilmes = listaDeFilmesEscolhido;
            ListaDosVencedores = new ObservableCollection<Filmes>();
            ExecutaObterVencedorCommand();
        }

        private string _posicaoFilmes;
        public string PosicaoFilmes
        {
            get => _posicaoFilmes = $"Selecionados {1} de 8 filmes";
            set => SetProperty(ref _posicaoFilmes, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private async void ExecutaObterVencedorCommand()
        {
            try
            {
                if(IsBusy)
                {
                    return;
                }

                IsBusy = true;
                if(NetworkConnection.CheckInternetConnection())
                {
                    int Posicao = 1;
                    ObservableCollection<Filmes> resultado = await _iFilmesService.PostObterCampeaoAsync(listaDeFilmes);
                    if(resultado.Count > 0 || resultado != null)
                    {
                        ListaDosVencedores.Clear();
                        foreach(var item in resultado)
                        {
                            if(string.IsNullOrWhiteSpace(item.erro))
                            {
                                item.PosicaoFilmes = Posicao.ToString();
                                ListaDosVencedores.Add(item);
                            }
                            else
                            {
                                MensagemErro(item.erro);
                                break;
                            }
                            Posicao++;
                        }
                    }
                    else
                    {
                        MensagemErro("Sinal de Internet não localizado.");
                    }
                }
                else
                {
                    MensagemErro("Sinal de Internet não localizado.");
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                MensagemErro("Sinal de Internet não localizado.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void MensagemErro(string erro)
        {
            Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Aviso", erro, "Ok"); });
        }
    }
}
