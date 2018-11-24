using CopaDeFilmes.App.Model;
using CopaDeFilmes.App.Service.Abstracts;
using CopaDeFilmes.App.Service.Concretes;
using CopaDeFilmes.App.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;


namespace CopaDeFilmes.App.ViewModel
{
    internal sealed class MainViewModel : BaseViewModel
    {
        public override string Title => "Lista de Filmes";

        private readonly IFilmesService _iFilmesService;
        public ObservableCollection<Filmes> ListaDeFilmes { get; }

        public ObservableCollection<Filmes> ListaDeFilmesOrdenadas { get; }
        public ObservableCollection<Filmes> ListaQuartas { get; }

        public ObservableCollection<Filmes> lFilmes;
        public Filmes ItemSelecionado { get; set; }

        public MainViewModel() : base()
        {
            Name = "Remover";
            _iFilmesService = DependencyService.Get<IFilmesService>();
            ListaDeFilmes = new ObservableCollection<Filmes>();
            ListaDeFilmesOrdenadas = new ObservableCollection<Filmes>();
            ListaQuartas = new ObservableCollection<Filmes>();
            MI_ObterListaFilmesAsync();
        }

        private string _quantidadeFilmes;
        public string QuantidadeFilmes
        {
            get => _quantidadeFilmes = $"Selecionados {ListaDeFilmes.Count().ToString()} de 8 filmes";
            set => SetProperty(ref _quantidadeFilmes, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _IsRefreshing = false;
        public bool IsRefreshing { get { return _IsRefreshing; } set { SetProperty(ref _IsRefreshing, value); OnPropertyChanged(); } }

        #region [AnteriorCommand]
        private Command _AnteriorCommand;

        public Command AnteriorCommand => _AnteriorCommand ??
            (_AnteriorCommand = new Command(async () => await AnteriorCommandExecute(), () => AnteriorCommandCanExecute()));

        private bool AnteriorCommandCanExecute()
        {
            return true;
        }

        private Task AnteriorCommandExecute()
        {
            return PopAsync();
        }
        #endregion

        #region [ExcluirCommand]
        private Command _ExcluirCommand;

        public Command ExcluirCommand => _ExcluirCommand ??
            (_ExcluirCommand = new Command(() => ExcluirCommandExecute(), () => ExcluirCommandCanExecute()));

        private bool ExcluirCommandCanExecute()
        {
            if(ListaDeFilmes.Count == 8)
            {
                Name = "Gerar";
            }

            return true;
        }
        private void ExcluirCommandExecute()
        {
            try
            {
                if(IsBusy)
                {
                    return;
                }

                IsBusy = true;
                ExcluirCommand.ChangeCanExecute();
                if(ItemSelecionado != null && ListaDeFilmes.Count > 8)
                {
                    ListaDeFilmes.Remove(ItemSelecionado);
                    ObterQuantidadeDeFiles();
                }
                else if(ListaDeFilmes.Count == 8)
                {
                    ObterVencedores(4);
                }
                else if(ListaDeFilmes.Count == 4)
                {
                    ObterVencedores(2);
                }
                else if(ListaDeFilmes.Count == 2)
                {
                    ObterVencedores(1);
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex);
            }
            finally
            {
                IsBusy = false;
                ExcluirCommand.ChangeCanExecute();
            }
        }

        private void ObterVencedores(int quantidadeVencedores)
        {
            Filmes Campeao = new Filmes();

            ListaDeFilmesOrdenadas.Clear();
            ListaDeFilmes.OrderBy(w => w.titulo).ToList().ForEach(item => ListaDeFilmesOrdenadas.Add(item));

            ListaDeFilmes.Clear();
            ListaDeFilmesOrdenadas.ForEach(item => ListaDeFilmes.Add(item));

            ListaQuartas.Clear();

            for(int i = 0; i < quantidadeVencedores; i++)
            {
                Filmes filme = ListaDeFilmes.Take(1).OrderBy(n => n.titulo).FirstOrDefault();
                Filmes filme2 = ListaDeFilmes.OrderByDescending(n => n.titulo).FirstOrDefault();
                
                if(filme.nota > filme2.nota)
                {
                    if(quantidadeVencedores == 1)
                    {
                        Campeao = filme;
                    }
                    else
                    {
                        ListaQuartas.Add(filme);
                    }
                }
                else if(filme2.nota > filme.nota)
                {
                    if(quantidadeVencedores == 1)
                        Campeao = filme2;
                    else
                        ListaQuartas.Add(filme2);
                }
                else
                {
                    string[] filmes = { filme.titulo, filme2.titulo };
                    IEnumerable<string> query = from f in filmes
                                                orderby f.Length, f.Substring(0, 1)
                                                select f;

                    foreach(string str in query)
                    {
                        if(str == filme.titulo)
                        {
                            if(quantidadeVencedores == 1)
                                Campeao = filme;
                            else
                                ListaQuartas.Add(filme);
                        }
                        else
                        {
                            if(quantidadeVencedores == 1)
                                Campeao = filme2;
                            else
                                ListaQuartas.Add(filme2);
                        }
                    }

                }

                ListaDeFilmes.Remove(filme);
                ListaDeFilmes.Remove(filme2);
            }

            if(quantidadeVencedores == 1)
                ListaDeFilmes.Add(Campeao);
            else
            {
               ListaQuartas.ForEach(item => ListaDeFilmes.Add(item));
            }
        }

        #endregion

        #region Atualizar Lista
        public Command _LoadLista;
        public Command LoadListaCommand => _LoadLista ?? (_LoadLista = new Command(async () => await LoadListaCommandAsync()));
        public async Task LoadListaCommandAsync()
        {
            try
            {
                if(IsBusy)
                {
                    return;
                }

                IsRefreshing = true;
                IsBusy = true;
                LoadListaCommand.ChangeCanExecute();
                MI_ObterListaFilmesAsync();
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                IsRefreshing = false;
                IsBusy = false;
                LoadListaCommand.ChangeCanExecute();
            }
        }
        #endregion

        public async void MI_ObterListaFilmesAsync()
        {
            if(NetworkConnection.CheckInternetConnection())
            {
                try
                {
                    IsRefreshing = true;
                    lFilmes = await _iFilmesService.ObterListaFilmesServiceAsync();
                    if(lFilmes.Count > 0 || lFilmes != null)
                    {
                        ListaDeFilmes.Clear();
                        lFilmes.ForEach(item => ListaDeFilmes.Add(item));
                        ObterQuantidadeDeFiles();
                    }
                }
                catch(Exception ex)
                {
                    Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Aviso", ex.ToString(), "Ok"); });
                }
                finally
                {
                    IsRefreshing = false;
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Aviso", "Sinal de Internet não localizado.", "Ok"); });
                IsRefreshing = false;
            }
        }

        public void ObterQuantidadeDeFiles()
        {
            QuantidadeFilmes = $"Selecionados 16  de {ListaDeFilmes.Count().ToString()} filmes";

            if(ListaDeFilmes.Count() == 8)
            {
                ListaDeFilmesOrdenadas.Clear();
                ListaDeFilmes.OrderBy(w => w.titulo).ToList().ForEach(item => ListaDeFilmesOrdenadas.Add(item));

                ListaDeFilmes.Clear();
                ListaDeFilmesOrdenadas.ForEach(item => ListaDeFilmes.Add(item));
            }
        }
    }
}
