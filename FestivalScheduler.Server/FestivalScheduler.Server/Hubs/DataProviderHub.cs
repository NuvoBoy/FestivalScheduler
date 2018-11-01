using System;
using System.Collections.Generic;
using System.Linq;
using FestivalScheduler.Data;
using Microsoft.AspNet.SignalR;

namespace FestivalScheduler.Server.Hubs
{
    public class DataProviderHub : Hub
    {
        /// <summary>
        /// load tripnodes of a special choir for the given interval
        /// </summary>
        /// <param name="choirId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<JsTripNode> LoadChoirTrips(string choirId, DateTime start, DateTime end)
        {
            Console.WriteLine($"49x000289: Loading choir tripnodes");
            var results = ServerConfig.Instance.DbCon.LoadChoirTripNodes(choirId, start, end);
            if (results == null) return new List<JsTripNode>();

            return results.Select(result => new TripNodeToJsTripNodeConverter().ConvertTripNode(result)).ToList();
        }
        /// <summary>
        /// load all tripnodes of choirs in a given intervall
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<JsTripNode> LoadAllChoirTrips(DateTime start, DateTime end)
        {
            Console.WriteLine($"49x000289: Loading choir tripnodes");
            var results = ServerConfig.Instance.DbCon.LoadChoirTripNodes(null, start, end);
            if (results == null) return new List<JsTripNode>();

            return results.Select(result => new TripNodeToJsTripNodeConverter().ConvertTripNode(result)).ToList();
        }
        /// <summary>
        /// first version to load mobil content
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [Obsolete]
        public IEnumerable<string> LoadMobilTripNodes(DateTime start, DateTime end)
        {
            Console.WriteLine($"49x000289: Loading choir tripnodes");
            var results = ServerConfig.Instance.DbCon.LoadChoirTripNodes(null, start, end).Select(result => new TripNodeToJsTripNodeConverter().ConvertTripNode(result)).ToList();
            var mobilList = new List<string>();

            foreach (var node in results)
            {
                mobilList.Add($"{node.StartTime:HH:mm}  {node.Passenger} von {node.DeparturePlace} nach {node.Destination}, mit {node.DriverName}");
            }

            return mobilList.ToArray();
        }
        /// <summary>
        /// load tripnodes for the drivingservice in a given interval
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<JsTripNode> LoadTripNodes(DateTime start, DateTime end)
        {
            Console.WriteLine($"49x00028A: Loading tripnodes");
            
            var results = ServerConfig.Instance.DbCon.LoadTripNodes(start, end);
            if (results == null) return new List<JsTripNode>();

            return results.Select(result => new TripNodeToJsTripNodeConverter().ConvertTripNode(result)).ToList();
        }
        /// <summary>
        /// load all entries of choirs
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ChoirNode> LoadChoirNodes()
        {
            Console.WriteLine($"49x00028A: Loading choirtripnodes");
            return ServerConfig.Instance.DbCon.LoadChoirNodes();
        }
        /// <summary>
        /// load all entries of drivers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DriverNode> LoadDriverNodes()
        {
            Console.WriteLine($"49x00028A: Loading driverNodes");
            return ServerConfig.Instance.DbCon.LoadDriverNodes();
        }
        /// <summary>
        /// load all entries of advisors
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdvisorNode> LoadAdvisorNodes()
        {
            Console.WriteLine($"49x00028A: Loading AdvisiorNodes");
            return ServerConfig.Instance.DbCon.LoadAdvisorNodes();
        }
    }
}
