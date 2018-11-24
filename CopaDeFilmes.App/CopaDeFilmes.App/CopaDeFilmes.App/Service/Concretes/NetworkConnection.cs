using System.Net;
using Xamarin.Forms;
using CopaDeFilmes.App.Service.Abstracts;

namespace CopaDeFilmes.App.Service.Concretes
{
    public class NetworkConnection
    {
        public static bool CheckInternetConnection()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            try
            {
                networkConnection.CheckNetworkConnection();
                var networkStatus = networkConnection.IsConnected ? true : false;
                return networkStatus;

            }
            catch(WebException ex)
            {
                return false;
            }
        }
    }
}
