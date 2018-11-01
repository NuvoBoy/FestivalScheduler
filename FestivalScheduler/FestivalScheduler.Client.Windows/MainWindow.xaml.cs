using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FestivalScheduler.Client.Windows.Backend;
using FestivalScheduler.Client.Windows.SharedData;
using FestivalScheduler.Data;
using Timer = System.Timers.Timer;

namespace FestivalScheduler.Client.Windows
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// datacontext
        /// </summary>
        private readonly ClientDataContext _dataContext;
        /// <summary>
        /// lock
        /// </summary>
        private static readonly object MLock = new object();
        /// <summary>
        /// wait while user is typing
        /// </summary>
        private readonly Timer _waitTimerTrip;
        private readonly Timer _waitTimerChoir;

        private readonly Timer _updateTimer;

        public MainWindow()
        {
            _dataContext = ClientDataContext.GetInstance();
            
            //Textsearch variables
            _waitTimerTrip = new Timer(500);
            _waitTimerTrip.Elapsed += (sender, e) => Application.Current.Dispatcher.BeginInvoke(new Action(() => { SearchTripNode(); }));
            _waitTimerChoir = new Timer(500);
            _waitTimerChoir.Elapsed += (sender, e) => Application.Current.Dispatcher.BeginInvoke(new Action(() => { SearchChoirTripNode(); }));

            //update view routine
            _updateTimer = new Timer(5000);
            _updateTimer.Elapsed += (sender, e) => Application.Current.Dispatcher.BeginInvoke(new Action(() => { UpdateTabels(); }));
            _updateTimer.Start();

            InitializeComponent();

            //set datacontext
            DataContext = _dataContext;
            DataGridAdvisors.ItemsSource = _dataContext.ViewAdvisor;
            DataGridDriver.ItemsSource = _dataContext.ViewDriver;
            DataGridChoirs.ItemsSource = _dataContext.ViewChoirs;

            DatePickerTrip.SelectedDate = DateTime.Now;
            DatePickerChoirTrip.SelectedDate = DateTime.Now;
        }
        /// <summary>
        /// appers after loading the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => new ChoirManager().LoadChoirs());
            Task.Factory.StartNew(() => new DriverManager().LoadDriver());
            Task.Factory.StartNew(() => new AdvisorManager().LoadAdvisor());
            Task.Factory.StartNew(() => new TripManager().LoadChoirTripNodes(DateTime.Today));
            Task.Factory.StartNew(() => new TripManager().LoadTripNodes(DateTime.Today)).ContinueWith(x=>UpdateTabels());
            //Task.Factory.StartNew(() => new TripManager().RegisterUpdate());

            _dataContext.SearchChoirTrip = "";
            _dataContext.SearchTrip = "";
        }
        /// <summary>
        /// load new values from dict and display them
        /// </summary>
        private void UpdateTabels()
        {
            if (!_dataContext.ServerAvailable)
            {
                ServerAvailable.Visibility = Visibility.Visible;
            }
            else
            {
                ServerAvailable.Visibility = Visibility.Hidden;
            }

            if(_dataContext.ChoirTripNodes != null && _dataContext.TripsChanged)
            {
                _dataContext.ViewChoirTripNodes.Clear();

                foreach(var node in _dataContext.ChoirTripNodes)
                {
                    _dataContext.ViewChoirTripNodes.Add(node.Value);                    
                }
                DataGridChoirTrip.Items.Refresh();
            }
            if(_dataContext.TripNodes != null && _dataContext.TripsChanged)
            {
                _dataContext.ViewTripNodes.Clear();

                foreach (var node in _dataContext.TripNodes)
                {
                    _dataContext.ViewTripNodes.Add(node.Value);
                }
                DataGridTrip.Items.Refresh();
                _dataContext.TripsChanged = true;
            }
            if(_dataContext.Driver != null && _dataContext.UserChanged)
            {
                _dataContext.ViewDriver.Clear();

                foreach (var node in _dataContext.Driver)
                {
                    _dataContext.ViewDriver.Add(node.Value);
                }
                DataGridDriver.Items.Refresh();
            }
            if(_dataContext.Advisor != null && _dataContext.UserChanged)
            {
                _dataContext.ViewAdvisor.Clear();

                foreach (var node in _dataContext.Advisor)
                {
                    _dataContext.ViewAdvisor.Add(node.Value);
                }
                DataGridAdvisors.Items.Refresh();
                _dataContext.UserChanged = false;
            }
            if(_dataContext.Choirs != null && _dataContext.ChoirsChanged)
            {
                _dataContext.ViewChoirs.Clear();

                foreach (var node in _dataContext.Choirs)
                {
                    _dataContext.ViewChoirs.Add(node.Value);
                }
                DataGridChoirs.Items.Refresh();
                _dataContext.ChoirsChanged = false;
            }
        }

        #region MenueItems

        /// <summary>
        /// end session of this user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuLogOff_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Ausgeloggt", "Missing Method!");
        }
        /// <summary>
        /// open change user data dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenueChangeUserData_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region ChoirTrips
        /// <summary>
        /// handel search operations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchChoirTripBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _waitTimerChoir.Stop();
            _waitTimerChoir.Start();

            //if there is no search string, load all
            if (_dataContext.SearchChoirTrip.Equals(""))
            {
                _waitTimerChoir.Stop();
                if (DatePickerChoirTrip.SelectedDate == null) return;
                var selectedDate = (DateTime)DatePickerChoirTrip.SelectedDate;
                Task.Factory.StartNew(() => new TripManager().LoadChoirTripNodes(selectedDate));

                return;
            }

            if (_dataContext.SearchChoirTrip.Equals(SearchChoirTripBox.Text))
            {
                _waitTimerChoir.Stop();
            }
            _dataContext.SearchChoirTrip = SearchChoirTripBox.Text;            
        }

        /// <summary>
        /// changing of the selected date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePickerChoirTrip_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePickerChoirTrip.SelectedDate == null) return;
            _dataContext.ChoirTripNodes.Clear();
            var selectedDate = (DateTime)DatePickerChoirTrip.SelectedDate;

            Task.Factory.StartNew(() => new TripManager().LoadChoirTripNodes(selectedDate));
        }

        /// <summary>
        /// create new ChoirTripNode/s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCreateNewChoirTrip_OnClick(object sender, RoutedEventArgs e)
        {
            var addTrips = new AddTrip("Choir"){Top = Top, Left = Left};

            addTrips.ShowDialog();
        }

        private void ButtonChangeChoirTrip_OnClick(object sender, RoutedEventArgs e)
        {
            var node = DataGridChoirTrip.SelectedValue as JsTripNode;
            if (node == null) return;

            new AddTrip(node) { Top = Top, Left = Left }.ShowDialog();
        }

        private void ButtonDeleteChoirTrip_OnClick(object sender, RoutedEventArgs e)
        {
            var node = DataGridChoirTrip.SelectedValue as JsTripNode;
            if (node == null) return;

            //TODO - Delete node!
        }
        #endregion
        /// <summary>
        /// show only tripnodes which match the textsearch parameter
        /// </summary>
        public void SearchChoirTripNode()
        {
            if (_dataContext.SearchChoirTrip.Equals(""))
            {
                Task.Factory.StartNew(() => new TripManager().LoadChoirTripNodes(_dataContext.SelectedChoirDate));
                return;
            }
            var puffer = _dataContext.ViewChoirTripNodes.ToList();
            
            foreach (var trip in puffer)
            {
                if (!trip.DeparturePlace.Contains(_dataContext.SearchChoirTrip) &&
                    !trip.Destination.Contains(_dataContext.SearchChoirTrip) &&
                    !trip.DriverName.Contains(_dataContext.SearchChoirTrip) && !trip.Passenger.Contains(_dataContext.SearchChoirTrip))
                {
                    _dataContext.ViewChoirTripNodes.Remove(trip);
                }
            }
            _waitTimerChoir.Stop();
        }

        /// <summary>
        /// show only tripnodes which match the textsearch parameter
        /// </summary>
        public void SearchTripNode()
        {
            if (_dataContext.SearchTrip.Equals(""))
            {
                Task.Factory.StartNew(() => new TripManager().LoadTripNodes(_dataContext.SelectedDate));
                return;
            }
            var puffer = _dataContext.ViewTripNodes.ToList();
            
            foreach (var trip in puffer)
            {
                if (!trip.DeparturePlace.Contains(_dataContext.SearchTrip) &&
                    !trip.Destination.Contains(_dataContext.SearchTrip) &&
                    !trip.DriverName.Contains(_dataContext.SearchTrip) && !trip.Passenger.Contains(_dataContext.SearchTrip))
                {
                    _dataContext.ViewTripNodes.Remove(trip);
                }
            }
            _waitTimerTrip.Stop();
        }
        #region Trips
        /// <summary>
        /// open create new tripnodes dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCreateNewTrip_OnClick(object sender, RoutedEventArgs e)
        {
            var addTrips = new AddTrip("DrivingService") { Top = Top, Left = Left };
            addTrips.ShowDialog();
        }
        /// <summary>
        /// load trip nodes for a given date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePickerTrip_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DatePickerTrip.SelectedDate == null) return;
            _dataContext.TripNodes.Clear();

            var selectedDate = (DateTime)DatePickerTrip.SelectedDate;

            Task.Factory.StartNew(() => new TripManager().LoadTripNodes(selectedDate));
        }
        /// <summary>
        /// wait if user is still typing or search nodes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTripBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _waitTimerTrip.Stop();
            _waitTimerTrip.Start();

            if (_dataContext.SearchTrip.Equals(SearchTripBox.Text))
            {
                _waitTimerTrip.Stop();
            }
            _dataContext.SearchTrip = SearchTripBox.Text;
            if (!_dataContext.SearchTrip.Equals("")) return;
            _waitTimerTrip.Stop();
            if (DatePickerTrip.SelectedDate == null) return;
            var selectedDate = (DateTime)DatePickerTrip.SelectedDate;

            Task.Factory.StartNew(() => new TripManager().LoadTripNodes(selectedDate));
        }
        
        /// <summary>
        /// delete selected Trip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteTrip_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(DataGridTrip.SelectedValue is TripNode node)) return;

            //TODO - Delete node!
        }
        /// <summary>
        /// change selected trip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChangeTrip_OnClick(object sender, RoutedEventArgs e)
        {
            var node = DataGridTrip.SelectedValue as JsTripNode;
            if (node == null) return;

            new AddTrip(node) { Top = Top, Left = Left }.ShowDialog();
        }
        #endregion

        #region Choirs
        /// <summary>
        /// create new choir entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddChoir_OnClick(object sender, RoutedEventArgs e)
        {
            new AddChoirEntry() { Top = Top, Left = Left }.ShowDialog();
        }
        /// <summary>
        /// modify choir entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChoir_OnClick(object sender, RoutedEventArgs e)
        {
            var node = DataGridChoirs.SelectedValue as ChoirNode;
            if (node == null) return;

            new AddChoirEntry(node) { Top = Top, Left = Left }.ShowDialog();
        }
        /// <summary>
        /// delete choir entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteChoir_OnClick(object sender, RoutedEventArgs e)
        {
            var node = DataGridChoirs.SelectedValue as ChoirNode;
            if (node == null) return;
            Task.Factory.StartNew(() => new ChoirManager().DeleteChoirNode(node.NodeId));
        }

        #endregion

        #region Advisors
        /// <summary>
        /// handel delete advisor button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteAdvisor_OnClick(object sender, RoutedEventArgs e)
        {
            var node = DataGridAdvisors.SelectedValue as AdvisorNode;
            if (node == null) return;
            Task.Factory.StartNew(() => new AdvisorManager().DeleteAdvisorNode(node.NodeId));
        }
        /// <summary>
        /// handel change advisor button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChangeAdvisor_OnClick(object sender, RoutedEventArgs e)
        {
            var node = DataGridAdvisors.SelectedValue as AdvisorNode;
            if (node == null) return;
            new AddUserEntry(node) { Top = Top, Left = Left }.ShowDialog();
        }
        #endregion

        #region Drivers
        /// <summary>
        /// handel delete driver button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDeleteDriver_OnClick(object sender, RoutedEventArgs e)
        {
            var node = DataGridDriver.SelectedValue as DriverNode;
            if (node == null) return;
            Task.Factory.StartNew(() => new DriverManager().DeleteDriverNode(node.NodeId));
        }
        /// <summary>
        /// handel change driver button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChangeDriver_OnClick(object sender, RoutedEventArgs e)
        {
            var node = DataGridDriver.SelectedValue as DriverNode;
            if (node == null) return;
            new AddUserEntry(node) { Top = Top, Left = Left }.ShowDialog();
        }
        #endregion
        /// <summary>
        /// open add new userentry dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddEntry_OnClick(object sender, RoutedEventArgs e)
        {
            new AddUserEntry() { Top = Top, Left = Left }.ShowDialog();
        }
        /// <summary>
        /// handle the change of the tabs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                var tab = ((TabControl) sender).SelectedValue as TabItem;
                if(tab.Header.Equals("Chöre") && (_dataContext.Choirs != null && !_dataContext.Choirs.Any())) Task.Factory.StartNew(() => new ChoirManager().LoadChoirs());
                if (tab.Header.Equals("Benutzer")
                    && ((_dataContext.Driver != null && !_dataContext.Driver.Any())
                        || (_dataContext.Advisor != null && !_dataContext.Advisor.Any())))
                {
                    Task.Factory.StartNew(() => new AdvisorManager().LoadAdvisor());
                    Task.Factory.StartNew(() => new DriverManager().LoadDriver());
                }
            }
        }
    }
}
