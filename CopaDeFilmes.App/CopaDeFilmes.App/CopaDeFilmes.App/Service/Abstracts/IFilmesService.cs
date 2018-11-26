using CopaDeFilmes.App.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace CopaDeFilmes.App.Service.Abstracts
{
    public interface IFilmesService
    {
        Task<ObservableCollection<Filmes>> GetListaFilmesServiceAsync();
        Task<ObservableCollection<Filmes>> PostObterCampeaoAsync(IList<Filmes> listaDeFilmes);
    }
}
