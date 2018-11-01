using System;
using System.Collections.Generic;
using FestivalScheduler.Data;
using Microsoft.AspNet.SignalR;

namespace FestivalScheduler.Server.Hubs
{
    /// <summary>
    /// every method which is needed for changetracking
    /// </summary>
    public class DrivingServiceHub : Hub
    {
        /// <summary>
        /// register user to the automatic update
        /// </summary>
        /// <returns></returns>
        public void RegisterUpdate(string userId)
        {
            Console.WriteLine($"49x00027E: {userId} registering for automatic update");
            if (!userId.Contains("Advisor"))
            {
                Groups.Add(Context.ConnectionId, "otherUsers");
                return;
            }

            var userEntry = (AdvisorNode)ServerConfig.Instance.DbCon.LoadEntry(userId);
            if (userEntry == null) return;
            //add an advisor to a group for his choir
            Groups.Add(Context.ConnectionId, userEntry.ChoirId);
        }
        /// <summary>
        /// update tripNode and inform follower
        /// </summary>
        /// <param name="node"></param>
        public void UpdateTripNode(JsTripNode node)
        {
            Console.WriteLine($"49x000288: Existing Tripnode({node.NodeId}) will be changed");
            //detect changes and save changes
            var changed = new ChangeDetection().CompareTripNodeWithJsNode(node);
            //convert to frontend typ
            var output = new TripNodeToJsTripNodeConverter().ConvertTripNode(changed);
            //inform user
            PublishNewNode(output);
        }
        /// <summary>
        /// delete tripNode from database and inform follower
        /// </summary>
        /// <param name="nodeId"></param>
        public void DeleteTripNode(string nodeId)
        {
            ServerConfig.Instance.DbCon.DeleteEntry(nodeId);
        }
        /// <summary>
        /// create new choir trip
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public JsTripNode CreateChoirTrip(TripRawData input)
        {
            Console.WriteLine($"49x000297: new trip for {input.Passenger}");
            var result = new TripNode();

            result.Passenger = input.Passenger;
            result.Description = input.Description;
            result.DriverNodeId = input.DriverName;
            result.EndPoint = input.EndPoint;
            result.ExpectedArrival = input.ExpectedArrival;
            result.StartPoint = input.StartPoint;
            result.StartTime = input.StartTime;
            result.Vehicle = input.Vehicle;
            result.NodeId = $"TripNode/{input.Passenger}/{Guid.NewGuid()}";
            result.Changes = new List<string> {"49x000298: Created"};

            if (!ServerConfig.Instance.DbCon.StoreEntry(result.NodeId, result)) return null;

            var output = new TripNodeToJsTripNodeConverter().ConvertTripNode(result);

            PublishNewNode(output);
            return output;
        }
        /// <summary>
        /// create new trip
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public JsTripNode CreateTrip(TripRawData input)
        {
            Console.WriteLine($"49x000299: new trip for {input.Passenger}");
            var result = new TripNode();

            result.Passenger = input.Passenger;
            result.Description = input.Description;
            result.DriverNodeId = input.DriverName;
            result.EndPoint = input.EndPoint;
            result.ExpectedArrival = input.ExpectedArrival;
            result.StartPoint = input.StartPoint;
            result.StartTime = input.StartTime;
            result.Vehicle = input.Vehicle;
            result.NodeId = $"TripNode/Individual/{Guid.NewGuid()}";
            result.Changes = new List<string> { "49x00029A: Created" };

            if (!ServerConfig.Instance.DbCon.StoreEntry(result.NodeId, result)) return null;

            var output = new TripNodeToJsTripNodeConverter().ConvertTripNode(result);

            PublishNewNode(output);
            return output;
        }
        /// <summary>
        /// publish new tripnodes to followers
        /// </summary>
        /// <param name="node"></param>
        private void PublishNewNode(JsTripNode node)
        {
            if (node.NodeId.Contains("Choir"))
            {
                Clients.Group(node.Passenger, Context.ConnectionId).NewTripNode(node);
            }
            Clients.Group("otherUsers").NewTripNode(node);
        }
    }
}
