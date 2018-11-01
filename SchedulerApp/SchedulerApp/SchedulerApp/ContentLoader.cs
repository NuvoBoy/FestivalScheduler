using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FestivalScheduler.Data;
using Microsoft.AspNet.SignalR.Client;

namespace SchedulerApp
{
    public class ContentLoader : INotifyPropertyChanged
    {
        /// <summary>
        /// server connection
        /// </summary>
        private readonly HubConnection _connection;
        /// <summary>
        /// hub for data loading operations
        /// </summary>
        private readonly IHubProxy _dataProviderProxy;
        /// <summary>
        /// ctor
        /// </summary>
        public ContentLoader()
        {
            _connection = new HubConnection("http://drivingservice.ddns.net:88/driving");
            _dataProviderProxy = _connection.CreateHubProxy("DataProviderHub");
        }
        /// <summary>
        /// create and start connection
        /// </summary>
        /// <returns></returns>
        public static async Task<ContentLoader> CreateAndStart()
        {
            var client = new ContentLoader();
            await client.Start();
            return client;
        }
        /// <summary>
        /// start connection
        /// </summary>
        /// <returns></returns>
        public Task Start()
        {
            return _connection.Start();
        }
        /// <summary>
        /// check connection
        /// </summary>
        public bool IsConnectedOrConnecting
        {
            get
            {
                return _connection.State != ConnectionState.Disconnected;
            }
        }
        /// <summary>
        /// get connectionstate
        /// </summary>
        public ConnectionState ConnectionState { get { return _connection.State; } }

        /// <summary>
        /// load tripnodes
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<JsTripNode> LoadTripNodes(DateTime date)
        {
            try
            {
                if (!IsConnectedOrConnecting) return new List<JsTripNode>();

                var start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                var end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                var results = _dataProviderProxy.Invoke<IEnumerable<JsTripNode>>("LoadAllChoirTrips", start, end).Result;

                return results.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<JsTripNode>();
            }
        }
        /// <summary>
        /// property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
