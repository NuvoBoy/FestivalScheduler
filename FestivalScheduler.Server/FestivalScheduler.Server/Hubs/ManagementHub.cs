using FestivalScheduler.Data;
using Microsoft.AspNet.SignalR;

namespace FestivalScheduler.Server.Hubs
{
    /// <summary>
    /// manage all operations on user and choirs
    /// </summary>
    public class ManagementHub : Hub
    {
        /// <summary>
        /// create a new advisor
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="permission"></param>
        /// <param name="choirId"></param>
        /// <param name="phoneNumber"></param>
        public AdvisorNode CreateAdvisor(string username, string password, string name, PermissionGroups permission, 
            string choirId, string phoneNumber)
        {
            var dbCon = ServerConfig.Instance.DbCon;

            var node = new AdvisorNode();
            node.ChoirId = choirId;
            node.Name = name;
            node.Password = password;
            node.PermissionLevel = permission;
            node.Section = "Chorbetreuer";
            node.ShortCut = name.Split(' ').Length > 1 ? name.Split(' ')[0] : name;
            node.UserName = username;
            node.PhoneNumber = phoneNumber;
            node.NodeId = $"UserNode/{node.Section}/{node.UserName}";

            dbCon.StoreEntry(node.NodeId, node);

            return node;
        }

        /// <summary>
        /// create new driver
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="permission"></param>
        /// <param name="membership"></param>
        /// <param name="phoneNumber"></param>
        public DriverNode CreateDriver(string username, string password, string name, PermissionGroups permission, 
            DriverMembership membership, string phoneNumber)
        {
            var dbCon = ServerConfig.Instance.DbCon;

            var node = new DriverNode();
            node.Membership = membership;
            node.Name = name;
            node.Password = password;
            node.PermissionLevel = permission;
            node.Section = "Fahrdienst";
            node.ShortCut = name.Split(' ').Length > 1 ? name.Split(' ')[0] : name;
            node.UserName = username;
            node.PhoneNumber = phoneNumber;
            node.NodeId = $"UserNode/{node.Section}/{node.UserName}";

            dbCon.StoreEntry(node.NodeId, node);

            return node;
        }
        /// <summary>
        /// delete one user entry from the database
        /// </summary>
        /// <param name="nodeId"></param>
        public void DeleteNode(string nodeId)
        {
            ServerConfig.Instance.DbCon.DeleteEntry(nodeId);
        }
        /// <summary>
        /// store changed value to db
        /// </summary>
        /// <param name="node"></param>
        public void ChangeNode(DriverNode node)
        {
            ServerConfig.Instance.DbCon.StoreEntry(node.NodeId, node);
        }
        /// <summary>
        /// create new choirNode
        /// </summary>
        /// <param name="name"></param>
        /// <param name="shortCut"></param>
        /// <param name="nationality"></param>
        /// <param name="membercount"></param>
        /// <param name="accomodationPlace"></param>
        /// <returns></returns>
        public ChoirNode CreateChoirNode(string name, string shortCut, string nationality, int membercount = 0,
            string accomodationPlace = null)
        {
            var result = new ChoirNode();

            result.Name = name;
            result.ShortCut = shortCut;
            result.Nationality = nationality;

            if (membercount > 0) result.MemberCount = membercount;

            if (accomodationPlace != null && !accomodationPlace.Equals(""))
                result.AccommodationPlace = accomodationPlace;

            result.NodeId = $"ChoirNode/{nationality}/{shortCut}";
            ServerConfig.Instance.DbCon.StoreEntry(result.NodeId, result);

            return result;
        }
    }
}
