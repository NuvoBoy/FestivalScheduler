using System;
using System.Collections.Generic;
using System.Linq;
using FestivalScheduler.Client.Windows.SharedData;
using FestivalScheduler.Data;

namespace FestivalScheduler.Client.Windows.Backend
{
    public class TripManager
    {
        private readonly ClientDataContext _dataContext;
        /// <summary>
        /// ctor
        /// </summary>
        public TripManager()
        {
            _dataContext = ClientDataContext.GetInstance();
        }
        /// <summary>
        /// save new nodes or update existing
        /// </summary>
        /// <param name="node"></param>
        private void ReceiveTripNode(JsTripNode node){
            if(node==null) return;

            if (node.NodeId.Contains("ChoirTrip"))
            {
                if (_dataContext.ChoirTripNodes.ContainsKey(node.NodeId))
                {
                    _dataContext.ChoirTripNodes[node.NodeId] = node;
                }
                else
                    _dataContext.ChoirTripNodes.GetOrAdd(node.NodeId, node);
            }
            else{
                if (_dataContext.TripNodes.ContainsKey(node.NodeId))
                {
                    _dataContext.TripNodes[node.NodeId] = node;
                }
                else
                    _dataContext.TripNodes.GetOrAdd(node.NodeId, node);
            }

            _dataContext.TripsChanged = true;
        }
        /// <summary>
        /// send raw data to server and wait for the new object
        /// </summary>
        /// <param name="data"></param>
        public void CreateNewTripNode(TripRawData data){
            try{
                var proxy = ServerConnector.GetInstance().GetTripPlaningProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                var result = data.Passenger.Contains("Choir") ? proxy.Invoke<JsTripNode>("CreateChoirTrip", data).Result : proxy.Invoke<JsTripNode>("CreateTrip", data).Result;

                if(result!=null) ReceiveTripNode(result);
            }catch (Exception e){
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// load all trip nodes for a selected date
        /// </summary>
        /// <param name="selectedDay"></param>
        public void LoadTripNodes(DateTime selectedDay)
        {
            try
            {
                var proxy = ServerConnector.GetInstance().GetDataProviderProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                var start = new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day, 0, 0, 0);
                var end = new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day, 23, 59, 59);
                var results = proxy.Invoke<IEnumerable<JsTripNode>>("LoadTripNodes", start, end).Result;

                foreach (var trip in results)
                {
                    if(trip.NodeId.Contains("ChoirNode")) continue;

                    if (_dataContext.TripNodes.ContainsKey(trip.NodeId))
                    {
                        _dataContext.TripNodes[trip.NodeId] = trip;
                    }
                    else
                    {
                        _dataContext.TripNodes.GetOrAdd(trip.NodeId, trip);
                    }
                }

                _dataContext.TripsChanged = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //TODO - besseres error handling
            }
        }
        /// <summary>
        /// load all choir tripnodes for a selected date
        /// </summary>
        /// <param name="selectedDay"></param>
        public void LoadChoirTripNodes(DateTime selectedDay)
        {
            try
            {
                var proxy = ServerConnector.GetInstance().GetDataProviderProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                var start = new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day, 0, 0, 0);
                var end = new DateTime(selectedDay.Year, selectedDay.Month, selectedDay.Day, 23, 59, 59);
                var results = proxy.Invoke<IEnumerable<JsTripNode>>("LoadAllChoirTrips", start, end).Result;

                foreach (var trip in results)
                {
                    if (trip.NodeId.Contains("Individual")) continue;

                    if (_dataContext.ChoirTripNodes.ContainsKey(trip.NodeId))
                    {
                        _dataContext.ChoirTripNodes[trip.NodeId] = trip;
                    }
                    else
                    {
                        _dataContext.ChoirTripNodes.GetOrAdd(trip.NodeId, trip);
                    }
                }
                _dataContext.TripsChanged = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// modify a tripnode
        /// </summary>
        /// <param name="node"></param>
        public void ModifyTripNode(JsTripNode node)
        {
            try
            {
                var proxy = ServerConnector.GetInstance().GetTripPlaningProxy();
                if (proxy == null || !_dataContext.ServerAvailable) return;

                if (node == null) return;

                proxy.Invoke("UpdateTripNode", node);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
