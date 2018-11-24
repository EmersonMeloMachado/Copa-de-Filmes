using Xamarin.Forms;
using CopaDeFilmes.App.ViewModel;

namespace CopaDeFilmes.App.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}
