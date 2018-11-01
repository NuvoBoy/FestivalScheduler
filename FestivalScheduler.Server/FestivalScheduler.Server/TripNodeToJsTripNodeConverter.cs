using FestivalScheduler.Data;

namespace FestivalScheduler.Server
{
    /// <summary>
    /// converter class
    /// </summary>
    public class TripNodeToJsTripNodeConverter
    {
        /// <summary>
        /// convert database typ tripnode to frontend typ jstripnode
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public JsTripNode ConvertTripNode(TripNode source)
        {
            if (source == null) return null;
            var result = new JsTripNode();
            if (source.DriverNodeId == null || source.DriverNodeId.Equals("")) result.DriverName = "";
            else{
                var driver = (DriverNode)ServerConfig.Instance.DbCon.LoadEntry(source.DriverNodeId);
                if (driver == null) return null;
                result.DriverName = driver.ShortCut;
            }
            if (source.Passenger != null && source.Passenger.Contains("ChoirNode")){
                var choir = (ChoirNode)ServerConfig.Instance.DbCon.LoadEntry(source.Passenger);
                if (choir == null) return null;
                result.Passenger = choir.Name;
            }
            else result.Passenger = source.Passenger;

            result.NodeId = source.NodeId;
            result.StartTime = source.StartTime;
            result.ExpectedArrival = source.ExpectedArrival;
            result.Description = source.Description;
            result.DeparturePlace = source.StartPoint;
            result.Destination = source.EndPoint;
            result.Vehicle = source.Vehicle;
            
            return result;
        }
    }
}
