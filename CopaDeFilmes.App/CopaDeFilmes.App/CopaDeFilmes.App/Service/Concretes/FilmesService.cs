using System;
using Newtonsoft.Json;
using System.Net.Http;
using CopaDeFilmes.App.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CopaDeFilmes.App.Service.Abstracts;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;

namespace CopaDeFilmes.App.Service.Concretes
{
    public class FilmesService : IFilmesService
    {
        //private readonly string baseUrl = $"https://copadosfilmes.azurewebsites.net/api/filmes";
        private readonly string baseUrl = $"https://copadefilmesapi20181125024148.azurewebsites.net/api/filmes";

        private ObservableCollection<Filmes> FilmesResult;
        private HttpClient _httpClient;

        public async Task<ObservableCollection<Filmes>> GetListaFilmesServiceAsync()
        {
            try
            {
                using(_httpClient = new HttpClient())
                {
                    using(var _response = await _httpClient.GetAsync(baseUrl))
                    {
                        if(!_response.IsSuccessStatusCode)
                        {
                            throw new Exception($"Não foi possivel obter a lista de filmes. Code({_response.StatusCode})");
                        }
                        else
                        {
                            var _responseContent = await _response.Content.ReadAsStringAsync();
                            FilmesResult = JsonConvert.DeserializeObject<ObservableCollection<Filmes>>(_responseContent);
                        }
                    }
                }

            }
            catch(Exception e)
            {
                throw e;
            }

            return FilmesResult;
        }

        public async Task<ObservableCollection<Filmes>> PostObterCampeaoAsync(IList<Filmes> listaDeFilmes)
        {
            ObservableCollection<Filmes> lFilmes = new ObservableCollection<Filmes>();
            Filmes Filme = new Filmes();
            try
            {
                var httpClient = new HttpClient();
                var conteudoJson = JsonConvert.SerializeObject(listaDeFilmes);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(conteudoJson, Encoding.UTF8, "application/json");
                using(var response = await httpClient.PostAsync(baseUrl, content))
                {
                    if(response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        if(string.IsNullOrWhiteSpace(result))
                        {
                            Filme.erro = "";
                            lFilmes.Add(Filme);
                        }
                        else
                        {
                            using(var resposeStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                            {

                                var settings = new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    MissingMemberHandling = MissingMemberHandling.Ignore
                                };

                                lFilmes =  JsonConvert.DeserializeObject<ObservableCollection<Filmes>>(await new System.IO.StreamReader(resposeStream)
                                    .ReadToEndAsync().ConfigureAwait(false), settings);
                            }
                        }
                    }
                    else
                    {
                        Filme.erro = "falha na requisição, tente novamente";
                        lFilmes.Add(Filme);
                    }
                }

            }
            catch(Exception ex)
            {
                Console.Write(ex);
                Filme.erro = "falha na requisição, tente novamente";
                lFilmes.Add(Filme);
            }

            return lFilmes;
        }
    }
}
