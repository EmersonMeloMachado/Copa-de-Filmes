using CopaDeFilmes.App.Service.Abstracts;
using CopaDeFilmes.App.Service.Concretes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CopaDeFilmes.App.View
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            RegisterServices();
            MainPage = new NavigationPage(new MainPage());
        }

        private void RegisterServices()
        {
            DependencyService.Register<IFilmesService, FilmesService>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
