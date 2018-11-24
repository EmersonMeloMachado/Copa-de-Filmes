using System;
using Newtonsoft.Json;
using System.Net.Http;
using CopaDeFilmes.App.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CopaDeFilmes.App.Service.Abstracts;

namespace CopaDeFilmes.App.Service.Concretes
{
    public class FilmesService : IFilmesService
    {
        private readonly string baseUrl = $"https://copadosfilmes.azurewebsites.net/api/filmes";

        private ObservableCollection<Filmes> FilmesResult;
        private HttpClient _httpClient;

        public async Task<ObservableCollection<Filmes>> ObterListaFilmesServiceAsync()
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
    }
}
