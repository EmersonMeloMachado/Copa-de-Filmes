using Xamarin.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CopaDeFilmes.App.Service.Concretes;

namespace CopaDeFilmes.App.ViewModel
{
    abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual string Title { get; }

        protected internal void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T storage, T valeu, [CallerMemberName]string propertyName = null)
        {
            if(EqualityComparer<T>.Default.Equals(storage, valeu))
            {
                return false;
            }

            storage = valeu;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected internal Task PushAsync(Page page,bool animated = true) => NavigationService.Current.PushAsync(page, animated);

        protected internal Task PopAsync(bool animated = true) => NavigationService.Current.PopAsync(animated);

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await View.App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task DisplayAlert(string title, string message, string accept, string cancel)
        {
            await View.App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        private bool _IsBusy = false;
        public bool IsBusy
        {
            get => _IsBusy;
            set
            {
                _IsBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy { get => !_IsBusy; }

    }
}
