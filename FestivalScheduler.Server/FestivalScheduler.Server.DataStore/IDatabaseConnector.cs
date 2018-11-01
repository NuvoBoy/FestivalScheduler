using System;
using System.Collections.Generic;
using FestivalScheduler.Data;

namespace FestivalScheduler.Server.Datastore
{
    public interface IDatabaseConnector
    {
        /// <summary>
        /// store one entry to the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        bool StoreEntry(string id, object entry);
        /// <summary>
        /// load a specific entry for a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        object LoadEntry(string id);
        /// <summary>
        /// delete entry with id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteEntry(string id);
        /// <summary>
        /// authenticate user against database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        bool AuthenticateUser(string userName, string passwordHash);
        /// <summary>
        /// replace the old user pw
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldHash"></param>
        /// <param name="newHash"></param>
        /// <returns></returns>
        bool ChangeUserPassword(string userName, string oldHash, string newHash);
        /// <summary>
        /// load all choir tripnodes for a special choir in a given interval
        /// </summary>
        /// <param name="choirId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        IEnumerable<TripNode> LoadChoirTripNodes(string choirId, DateTime start, DateTime end);
        /// <summary>
        /// load tripnodes in a given interval
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        IEnumerable<TripNode> LoadTripNodes(DateTime start, DateTime end);
        /// <summary>
        /// load all choir nodes
        /// </summary>
        /// <returns></returns>
        IEnumerable<ChoirNode> LoadChoirNodes();
        /// <summary>
        /// load all choir advisor nodes
        /// </summary>
        /// <returns></returns>
        IEnumerable<AdvisorNode> LoadAdvisorNodes();
        /// <summary>
        /// load all driver nodes
        /// </summary>
        /// <returns></returns>
        IEnumerable<DriverNode> LoadDriverNodes();
    }
}
