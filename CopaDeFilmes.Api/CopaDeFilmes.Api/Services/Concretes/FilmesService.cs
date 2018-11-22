﻿using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using CopaDeFilmes.Api.Models;
using CopaDeFilmes.Api.Services.Abstracts;
using System.Collections.Generic;

namespace CopaDeFilmes.Api.Services.Concretes
{
    public class FilmesService : IFilmesService
    {
        private readonly string baseUrl = $"http://copafilmes.azurewebsites.net/api/filmes";

        private IList<Filmes> FilmesResult;
        private HttpClient _httpClient;
        private readonly string _key;

        public FilmesService()
        {

        }

        public async Task<IList<Filmes>> ObterListaFilmesService()
        {
            try
            {
                using(_httpClient = new HttpClient())
                {
                    using(var _response = await _httpClient.GetAsync(baseUrl))
                    {
                        if(!_response.IsSuccessStatusCode)
                        {
                            throw new Exception("Não foi possivel obter a lista de filmes. Code(" + _response.StatusCode + ")" );
                        }
                        else
                        {
                            var _responseContent = await _response.Content.ReadAsStringAsync();
                            FilmesResult = JsonConvert.DeserializeObject<IList<Filmes>>(_responseContent);
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