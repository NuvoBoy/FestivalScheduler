using System;

namespace SchedulerApp
{
    /// <summary>
    /// helper class for detail view
    /// </summary>
    public class TripDetailWrapper
    {
        /// <summary>
        /// timestamp of departure
        /// </summary>
        public DateTime DepartureTime { get; set; }
        /// <summary>
        /// timestamp of expected arrival
        /// </summary>
        public DateTime ExpectedArrival { get; set; }
        /// <summary>
        /// start location
        /// </summary>
        public string DeparturePlace { get; set; }
        /// <summary>
        /// end location
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// name of the driver
        /// </summary>
        public string DriverName { get; set; }
    }
}
