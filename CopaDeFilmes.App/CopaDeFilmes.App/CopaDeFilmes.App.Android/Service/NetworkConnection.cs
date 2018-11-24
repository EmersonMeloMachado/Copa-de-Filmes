using Android.Net;
using Xamarin.Forms;
using Android.Content;
using CopaDeFilmes.App.Droid.Service;
using CopaDeFilmes.App.Service.Abstracts;

[assembly: Dependency(typeof(NetworkConnection))]
namespace CopaDeFilmes.App.Droid.Service
{
    public class NetworkConnection : INetworkConnection
    {
        public bool IsConnected { get; set; }
        public NetworkConnection()
        {

        }

        public void CheckNetworkConnection()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
            NetworkInfo activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            if(activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting)
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }
    }
}