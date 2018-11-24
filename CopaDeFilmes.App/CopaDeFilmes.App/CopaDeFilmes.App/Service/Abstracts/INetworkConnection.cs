using System;
using System.Collections.Generic;
using System.Text;

namespace CopaDeFilmes.App.Service.Abstracts
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworkConnection();
    }
}
