namespace FestivalScheduler.Server.Datastore
{
    public class DatabaseConfig
    {
        /// <summary>
        /// url of the raven db server
        /// </summary>
        public string RavenDbServer { get; set; } = "http://localhost:8888";
        /// <summary>
        /// name of the raven database
        /// </summary>
        public string DataBaseName { get; set; } = "DrivingService";
    }
}
