using FestivalScheduler.Data;

namespace FestivalScheduler.Server
{
    public class ChangeDetection
    {
        /// <summary>
        /// compare two entries and change the values
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TripNode CompareTripNodeWithJsNode(JsTripNode input)
        {
            if (input == null) return null;
            var dbCon = ServerConfig.Instance.DbCon;

            var output = (TripNode) dbCon.LoadEntry(input.NodeId);
            if (output == null) return null;

            if (!output.Description.Equals(input.Description)){
                output.Description = input.Description;
                output.Changes.Add($"49x000280: description changed to: {input.Description}");
            }
            if (!output.StartPoint.Equals(input.DeparturePlace)){
                output.StartPoint = input.DeparturePlace;
                output.Changes.Add($"49x000281: startpoint changed to: {input.DeparturePlace}");
            }
            if (!output.EndPoint.Equals(input.Destination)){
                output.EndPoint = input.Destination;
                output.Changes.Add($"49x000282: EndPoint changed to: {input.Destination}");
            }
            if (output.Vehicle != input.Vehicle){
                output.Vehicle = input.Vehicle;
                output.Changes.Add($"49x000284: Vehicle changed to: {input.Vehicle}");
            }
            if (output.StartTime != input.StartTime){
                output.StartTime = input.StartTime;
                output.Changes.Add($"49x000285: StartTime changed to: {input.StartTime}");
            }
            if (output.ExpectedArrival != input.ExpectedArrival){
                output.ExpectedArrival = input.ExpectedArrival;
                output.Changes.Add($"49x000286: ExpectedArrival changed to: {input.ExpectedArrival}");
            }
            if (!output.DriverNodeId.Contains(input.DriverName)){
                output.DriverNodeId = $"DriverNode/{input.DriverName}";
                output.Changes.Add($"49x000287: Driver changed to: {input.DriverName}");
            }
            if (!output.Passenger.Equals(input.Passenger))
            {
                if (!output.Passenger.Contains("Choir"))
                {
                    output.EndPoint = input.Destination;
                    output.Changes.Add($"49x000282: EndPoint changed to: {input.Destination}");
                }
            }
            //store changes to database
            dbCon.StoreEntry(output.NodeId, output);
            return output;
        }
    }
}
