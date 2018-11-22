using CopaDeFilmes.Api.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CopaDeFilmes.Api.Services.Abstracts
{
    public interface IFilmesService
    {
        Task<IList<Filmes>> ObterListaFilmesService();
    }
}
