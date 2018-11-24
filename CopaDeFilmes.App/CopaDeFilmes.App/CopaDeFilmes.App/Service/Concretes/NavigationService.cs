using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CopaDeFilmes.App.Service.Concretes
{   
    sealed class NavigationService
    {
        private static Lazy<NavigationService> _Lazy = new Lazy<NavigationService>(() => new NavigationService());
        public static NavigationService Current { get => _Lazy.Value; }
        private NavigationService() { }

        private INavigation _Navigation
        {
            get
            {
                return View.App.Current.MainPage.Navigation;
            }
        }

        public Task PushAsync(Page page, bool animated = true) => _Navigation.PushAsync(page, animated);

        public Task PopAsync(bool animated = true) => _Navigation.PopAsync(animated);

    }
}
