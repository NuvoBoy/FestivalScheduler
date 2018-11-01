using System;
using System.Collections.Generic;

namespace FestivalScheduler.Data
{
    /// <summary>
    /// database class for trips
    /// </summary>
    public class TripNode
    {
        /// <summary>
        /// global id
        /// style: TripNode/{Passenger}/{GUID}
        /// </summary>
        public string NodeId { get; set; }
        /// <summary>
        /// name of passenger or choir-id
        /// </summary>
        public string Passenger { get; set; }
        /// <summary>
        /// timestamp of the start
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// timestamp of the expected arrival
        /// </summary>
        public DateTime ExpectedArrival { get; set; }
        /// <summary>
        /// id of the driver
        /// </summary>
        public string DriverNodeId { get; set; }
        /// <summary>
        /// optional description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// start point of the trip
        /// </summary>
        public string StartPoint { get; set; }
        /// <summary>
        /// destination place
        /// </summary>
        public string EndPoint { get; set; }
        /// <summary>
        /// type of the used vehicle
        /// </summary>
        public VehicleType Vehicle { get; set; }
        /// <summary>
        /// list of changes, only on server
        /// </summary>
        public List<string> Changes { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public TripNode()
        {
            Changes = new List<string>();
        }
    }
}
