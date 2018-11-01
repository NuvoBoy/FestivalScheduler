using System;
using System.Net;
using System.Threading;
using FestivalScheduler.Client.Windows.SharedData;
using Microsoft.AspNet.SignalR.Client;

namespace FestivalScheduler.Client.Windows.Backend
{
    public class ServerConnector
    {
        /// <summary>
        /// server connection
        /// </summary>
        private HubConnection _serverConnection;
        /// <summary>
        /// hub for trip management
        /// </summary>
        private IHubProxy _tripManagementProxy;
        /// <summary>
        /// hub for user handling
        /// </summary>
        private IHubProxy _managementProxy;
        /// <summary>
        /// hub for data loading operations
        /// </summary>
        private IHubProxy _dataProviderProxy;
        /// <summary>
        /// configfile of the backend
        /// </summary>
        private readonly BackendConfig _config;
        /// <summary>
        /// shared dataclass
        /// </summary>
        private readonly ClientDataContext _dataContext;
        /// <summary>
        /// instance of the server connector
        /// </summary>
        private static volatile ServerConnector _instance;

        /// <summary>
        /// ctor
        /// </summary>
        private ServerConnector()
        {
            _config = new BackendConfig();
            _dataContext = ClientDataContext.GetInstance();

            StartServerConnection();
            Thread.Sleep(1000);

            if (_serverConnection.State == ConnectionState.Connected) _dataContext.ServerAvailable = true;
            //TODO - RegisterUpdate
        }
        /// <summary>
        /// singelton get instance
        /// </summary>
        /// <returns></returns>
        public static ServerConnector GetInstance()
        {
            return _instance ?? (_instance = new ServerConnector());
        }

        /// <summary>
        /// start server connection
        /// </summary>
        private async void StartServerConnection()
        {
            try
            {
                _serverConnection = new HubConnection(_config.ServerUrl);
                _serverConnection.EnsureReconnecting();

                _tripManagementProxy = _serverConnection.CreateHubProxy(_config.TripManagmentHub);
                _managementProxy = _serverConnection.CreateHubProxy(_config.ManagementHub);
                _dataProviderProxy = _serverConnection.CreateHubProxy(_config.DataProviderHub);

                ServicePointManager.DefaultConnectionLimit = 10;
                await _serverConnection.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dataContext.ServerAvailable = false;
            }
        }

        /// <summary>
        /// get management proxy 
        /// </summary>
        /// <returns></returns>
        public IHubProxy GetManagementProxy()
        {
            try
            {
                if (_serverConnection.State == ConnectionState.Disconnected)
                {
                    StartServerConnection();
                    Thread.Sleep(1000);
                }
                return _serverConnection.State != ConnectionState.Connected ? null : _managementProxy;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dataContext.ServerAvailable = false;
                return null;
            }
        }
        /// <summary>
        /// get trip management proxy
        /// </summary>
        /// <returns></returns>
        public IHubProxy GetTripPlaningProxy()
        {
            try
            {
                if (_serverConnection.State == ConnectionState.Disconnected)
                {
                    StartServerConnection();
                    Thread.Sleep(1000);
                }
                return _serverConnection.State != ConnectionState.Connected ? null : _tripManagementProxy;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dataContext.ServerAvailable = false;
                return null;
            }
        }
        /// <summary>
        /// get data provider proxy
        /// </summary>
        /// <returns></returns>
        public IHubProxy GetDataProviderProxy()
        {
            try
            {
                if (_serverConnection.State == ConnectionState.Disconnected)
                {
                    StartServerConnection();
                    Thread.Sleep(1000);
                }
                return _serverConnection.State != ConnectionState.Connected ? null : _dataProviderProxy;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _dataContext.ServerAvailable = false;
                return null;
            }
        }
    }
}
