using System;

namespace FestivalScheduler.Data
{
    /// <summary>
    /// class used to show data in the clients
    /// </summary>
    public class JsTripNode
    {
        /// <summary>
        /// global id
        /// style: TripNode/{Passenger}/{GUID}
        /// </summary>
        public string NodeId { get; set; }
        /// <summary>
        /// name of passenger or choir
        /// </summary>
        public string Passenger { get; set; }
        /// <summary>
        /// destination place
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// timestamp of the start
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// timestamp of the expected arrival
        /// </summary>
        public DateTime ExpectedArrival { get; set; }
        /// <summary>
        /// name of the driver
        /// </summary>
        public string DriverName { get; set; }
        /// <summary>
        /// optional description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// start point of the trip
        /// </summary>
        public string DeparturePlace { get; set; }
        /// <summary>
        /// type of the used vehicle
        /// </summary>
        public VehicleType Vehicle { get; set; }
    }
}
