using System;
using System.Net;
using Xamarin.Forms;
using CoreFoundation;
using SystemConfiguration;
using CopaDeFilmes.App.iOS.Service;
using CopaDeFilmes.App.Service.Abstracts;

[assembly: Dependency(typeof(NetworkConnection))]
namespace CopaDeFilmes.App.iOS.Service
{
    public class NetworkConnection : INetworkConnection
    {
        public bool IsConnected { get; set; }
        public void CheckNetworkConnection()
        {
            if(InternetConnectionStatus())
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }

        private void UpdateNetworkStatus()
        {
            if(InternetConnectionStatus())
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }

        private event EventHandler ReachabilityChanged;
        private void OnChange(NetworkReachabilityFlags flags)
        {
            var h = ReachabilityChanged;
            if(h != null)
                h(null, EventArgs.Empty);
        }

        private NetworkReachability defaultRouteReachability;
        private bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if(defaultRouteReachability == null)
            {
                defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                defaultRouteReachability.SetNotification(OnChange);
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            if(!defaultRouteReachability.TryGetFlags(out flags))
                return false;
            return IsReachableWithoutRequiringConnection(flags);
        }

        private NetworkReachability adHocWiFiNetworkReachability;
        

        public static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

            if((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;

            return isReachable && noConnectionRequired;
        }

        private bool InternetConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvailable(out flags);
            if(defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
            {
                return false;
            }
            else if((flags & NetworkReachabilityFlags.IsWWAN) != 0)
            {
                return true;
            }
            else if(flags == 0)
            {
                return false;
            }

            return true;
        }
    }
}