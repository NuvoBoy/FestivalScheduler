using System;

namespace FestivalScheduler.Data
{
    /// <summary>
    /// helper class for trip createn
    /// </summary>
    public class TripRawData
    {
        /// <summary>
        /// name of passenger or choir
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
        public string StartPoint { get; set; }
        /// <summary>
        /// destination place
        /// </summary>
        public string EndPoint { get; set; }
        /// <summary>
        /// type of the used vehicle
        /// </summary>
        public VehicleType Vehicle { get; set; }
    }
}
