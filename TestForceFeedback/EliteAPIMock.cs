using EliteAPI.Abstractions;
using EliteAPI.Options.Bindings.Models;
using EliteAPI.Status.Backpack.Abstractions;
using EliteAPI.Status.Cargo.Abstractions;
using EliteAPI.Status.Commander.Abstractions;
using EliteAPI.Status.Market.Abstractions;
using EliteAPI.Status.Modules.Abstractions;
using EliteAPI.Status.NavRoute.Abstractions;
using EliteAPI.Status.Outfitting.Abstractions;
using EliteAPI.Status.Ship.Abstractions;
using EliteAPI.Status.Shipyard.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForceFeedback
{
    public class EliteAPIMock : IEliteDangerousApi
    {
        public string Version => throw new NotImplementedException();

        public EliteAPI.Event.Handler.EventHandler Events => throw new NotImplementedException();

        public IShip Ship => throw new NotImplementedException();

        public ICommander Commander => throw new NotImplementedException();

        public IBackpack Backpack => throw new NotImplementedException();

        public IShipyard Shipyard => throw new NotImplementedException();

        public INavRoute NavRoute => throw new NotImplementedException();

        public ICargo Cargo => throw new NotImplementedException();

        public IMarket Market => throw new NotImplementedException();

        public IModules Modules => throw new NotImplementedException();

        public IOutfitting Outfitting => throw new NotImplementedException();

        public IBindings Bindings => throw new NotImplementedException();

        public bool IsInitialized => throw new NotImplementedException();

        public bool IsRunning => throw new NotImplementedException();

        public bool HasCatchedUp => throw new NotImplementedException();

        public event System.EventHandler OnCatchedUp;
        public event System.EventHandler OnStart;
        public event System.EventHandler OnStop;
        public event EventHandler<Exception> OnError;

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }
    }
}
