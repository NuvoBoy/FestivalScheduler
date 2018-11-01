namespace FestivalScheduler.Client.Windows.Backend
{
    public class BackendConfig
    {
        //local server for testing
        public string ServerUrl = "http://localhost:88/driving";
        //public string ServerUrl = "http://drivingservice.ddns.net:88/driving";
        public string TripManagmentHub = "DrivingServiceHub";
        public string ManagementHub = "ManagementHub";
        public string DataProviderHub = "DataProviderHub";
    }
}
