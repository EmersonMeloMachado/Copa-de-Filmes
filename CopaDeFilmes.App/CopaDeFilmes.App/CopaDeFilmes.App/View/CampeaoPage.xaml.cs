using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CopaDeFilmes.App.Model;
using CopaDeFilmes.App.ViewModel;
using System.Collections.ObjectModel;

namespace CopaDeFilmes.App.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampeaoPage : ContentPage
    {
        public CampeaoPage()
        {
            InitializeComponent();
            BindingContext = new CampeaoViewModel();
        }
        public CampeaoPage(ObservableCollection<Filmes> listaDeFilmes)
        {
            InitializeComponent();
            BindingContext = new CampeaoViewModel(listaDeFilmes);
        }
    }
}