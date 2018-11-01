using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FestivalScheduler.Data;

namespace FestivalScheduler.Client.Windows.SharedData
{
    public class ClientDataContext : INotifyPropertyChanged
    {
        /// <summary>
        /// current user
        /// </summary>
        public UserNode User { get; set; }
        /// <summary>
        /// is server available
        /// </summary>
        public bool ServerAvailable { get; set; } = false;
        /// <summary>
        /// datacontext for tripnode tables
        /// threadsave
        /// </summary>
        public ConcurrentDictionary<string, JsTripNode> ChoirTripNodes { get; set; }
        public ConcurrentDictionary<string, JsTripNode> TripNodes { get; set; }
        /// <summary>
        /// help variable for trip changes
        /// </summary>
        public bool TripsChanged = true;
        /// <summary>
        /// datacontext for user tables
        /// </summary>
        public ConcurrentDictionary<string, DriverNode> Driver { get; set; }
        public ConcurrentDictionary<string, AdvisorNode> Advisor { get; set; }
        /// <summary>
        /// help variable for user changes
        /// </summary>
        public bool UserChanged = true;
        /// <summary>
        /// datacontext for choir table
        /// </summary>
        public ConcurrentDictionary<string, ChoirNode> Choirs { get; set; }
        /// <summary>
        /// help variable for choirs changes
        /// </summary>
        public bool ChoirsChanged = true;
        /// <summary>
        /// datacontext for tripnode tables
        /// </summary>
        public ObservableCollection<JsTripNode> ViewChoirTripNodes { get; set; }
        public ObservableCollection<JsTripNode> ViewTripNodes { get; set; }
        /// <summary>
        /// datacontext for user tables
        /// </summary>
        public ObservableCollection<DriverNode> ViewDriver { get; set; }
        public ObservableCollection<AdvisorNode> ViewAdvisor { get; set; }
        public ObservableCollection<ChoirNode> ViewChoirs { get; set; }

        /// <summary>
        /// search stuff
        /// </summary>
        public string SearchChoirTrip { get; set; }
        public string SearchTrip { get; set; }
        public DateTime SelectedChoirDate { get; set; }
        public DateTime SelectedDate { get; set; }

        #region Singelton Stuff
        /// <summary>
        /// thrown when data changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// singleton instance
        /// </summary>
        private static volatile ClientDataContext _instance;
        /// <summary>
        /// lock
        /// </summary>
        private static readonly object MLock = new object();
        private ClientDataContext()
        {
            ChoirTripNodes = new ConcurrentDictionary<string, JsTripNode>();
            TripNodes = new ConcurrentDictionary<string, JsTripNode>();
            Driver = new ConcurrentDictionary<string, DriverNode>();
            Advisor = new ConcurrentDictionary<string, AdvisorNode>();
            Choirs = new ConcurrentDictionary<string, ChoirNode>();

            ViewChoirTripNodes = new ObservableCollection<JsTripNode>();
            ViewTripNodes = new ObservableCollection<JsTripNode>();
            ViewDriver = new ObservableCollection<DriverNode>();
            ViewAdvisor = new ObservableCollection<AdvisorNode>();
            ViewChoirs = new ObservableCollection<ChoirNode>();
        }
        /// <summary>
        /// get an instance of the WorkLogDataContext
        /// </summary>
        /// <returns></returns>
        public static ClientDataContext GetInstance()
        {
            if (_instance == null)
                lock (MLock)
                {
                    if (_instance == null) _instance = new ClientDataContext();
                }
            Console.WriteLine("49x0001AA: WorkLogDataContext: Instance called");
            return _instance;
        }
        #endregion
    }
}
