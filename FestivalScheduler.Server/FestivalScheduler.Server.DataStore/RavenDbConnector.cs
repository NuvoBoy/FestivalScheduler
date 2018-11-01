using System;
using System.Collections.Generic;
using System.Linq;
using FestivalScheduler.Data;
using Raven.Client;
using Raven.Client.Document;


namespace FestivalScheduler.Server.Datastore
{
    public class RavenDbConnector : IDatabaseConnector
    {
        /// <summary>
        /// Ravendb
        /// </summary>
        private static IDocumentStore _store;
        /// <summary>
        /// ctor 
        /// </summary>
        public RavenDbConnector() : this(null)
        { }
        /// <summary>
        /// ctor with given config
        /// </summary>
        public RavenDbConnector(DatabaseConfig gConfig)
        {
            //get config
            var config = gConfig ?? new DatabaseConfig();
            ResetDbConnection(config);
        }
        /// <summary>
        /// create connection to db
        /// </summary>
        /// <param name="config"></param>
        private static void ResetDbConnection(DatabaseConfig config)
        {
            try
            {
                _store = new DocumentStore
                {
                    Url = config.RavenDbServer,
                    DefaultDatabase = config.DataBaseName,
                };
                _store.Initialize();
                Console.WriteLine("49x00028B db-connection init done");
            }
            catch (Exception)
            {
                Console.WriteLine("49x00028C db-connection init failed with error");
            }
        }
        /// <summary>
        /// store one entry to the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool StoreEntry(string id, object entry)
        {
            Console.WriteLine($"0x000102 Saving Entry {id} to Database");
            try
            {
                using (var session = _store.OpenSession())
                {
                    session.Store(entry, id);
                    session.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// load a specific entry for a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object LoadEntry(string id)
        {
            Console.WriteLine($"49x00028D Loading Entry {id} from Database");
            try
            {
                using (var session = _store.OpenSession())
                {
                    return session.Load<object>(id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        /// <summary>
        /// delete entry with id from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEntry(string id)
        {
            Console.WriteLine($"49x00028E Delete Entry {id} from Database");
            try
            {
                using (var session = _store.OpenSession())
                {
                    session.Delete(id);
                    session.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// authenticate user against database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public bool AuthenticateUser(string userName, string passwordHash)
        {
            var internalNode = (UserNode) LoadEntry($"UserNode/{userName}");

            return internalNode != null && internalNode.Password.Equals(passwordHash);
        }
        /// <summary>
        /// replace the old user pw
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldHash"></param>
        /// <param name="newHash"></param>
        /// <returns></returns>
        public bool ChangeUserPassword(string userName, string oldHash, string newHash)
        {
            var internalNode = (UserNode)LoadEntry($"UserNode/{userName}");

            if (internalNode == null || !internalNode.Password.Equals(oldHash)) return false;
            internalNode.Password = newHash;
            return StoreEntry(internalNode.NodeId, internalNode);
        }

        /// <summary>
        /// load all choir tripnodes for a special choir in a given interval
        /// </summary>
        /// <param name="choirId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<TripNode> LoadChoirTripNodes(string choirId, DateTime start, DateTime end)
        {
            Console.WriteLine($"49x00028F Load entries for {choirId} between {start} and {end}");
            try
            {
                using (var session = _store.OpenSession())
                {
                    if (choirId != null){
                        var tempChoirId = choirId;
                        var resultOne = session.Advanced.DocumentQuery<TripNode>()
                            .WhereEquals(x=>x.Passenger, tempChoirId)
                            .WhereBetween(x => x.StartTime, start, end)
                            .ToArray();
                        return resultOne;
                    }
                    var resultAll = session.Advanced.DocumentQuery<TripNode>()
                        .WhereBetween(x => x.StartTime, start, end)
                        .ToArray();
                    return resultAll;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        /// <summary>
        /// load tripnodes in a given interval
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public IEnumerable<TripNode> LoadTripNodes(DateTime start, DateTime end)
        {
            Console.WriteLine($"49x000290 Load trip entries between {start} and {end}");
            try{
                using (var session = _store.OpenSession()){
                    return session.Advanced.DocumentQuery<TripNode>()
                        .WhereBetween(x => x.StartTime, start, end)
                        .ToArray();
                }
            }catch (Exception e){
                Console.WriteLine(e);
                return null;
            }
        }
        /// <summary>
        /// load all choir nodes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ChoirNode> LoadChoirNodes()
        {
            Console.WriteLine($"49x000291 Load all entries of choirnodes");
            try
            {
                using (var session = _store.OpenSession())
                {
                    return session.Query<ChoirNode>().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        /// <summary>
        /// load all choir advisor nodes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdvisorNode> LoadAdvisorNodes()
        {
            Console.WriteLine($"49x000292 Load all entries of advisornodes");
            try
            {
                using (var session = _store.OpenSession())
                {
                    return session.Query<AdvisorNode>().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        /// <summary>
        /// load all driver nodes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DriverNode> LoadDriverNodes()
        {
            Console.WriteLine($"49x000293 Load all entries of drivernodes");
            try
            {
                using (var session = _store.OpenSession())
                {
                    return session.Query<DriverNode>().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
