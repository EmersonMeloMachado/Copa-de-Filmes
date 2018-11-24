using CopaDeFilmes.App.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CopaDeFilmes.App.Service.Abstracts
{
    public interface IFilmesService
    {
        Task<ObservableCollection<Filmes>> ObterListaFilmesServiceAsync();
    }
}
