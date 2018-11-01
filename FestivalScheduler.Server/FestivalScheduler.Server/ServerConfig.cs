using System;
using FestivalScheduler.Server.Datastore;

namespace FestivalScheduler.Server
{
    public sealed class ServerConfig
    {
        /// <summary>
        /// instance of config
        /// </summary>
        private static volatile ServerConfig _instance;
        private static object syncRoot = new Object();
        /// <summary>
        /// ctor
        /// </summary>
        private ServerConfig()
        {
            DbCon = new RavenDbConnector();
        }
        /// <summary>
        /// database connection
        /// </summary>
        public IDatabaseConnector DbCon { get; }
        /// <summary>
        /// get or create instance
        /// </summary>
        public static ServerConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new ServerConfig();
                    }
                }
                return _instance;
            }
        }
    }
}
