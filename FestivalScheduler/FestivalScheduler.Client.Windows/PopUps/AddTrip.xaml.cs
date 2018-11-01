using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FestivalScheduler.Client.Windows.Backend;
using FestivalScheduler.Client.Windows.SharedData;
using FestivalScheduler.Data;

namespace FestivalScheduler.Client.Windows
{
    /// <summary>
    /// Interaktionslogik für AddTrip.xaml
    /// </summary>
    public partial class AddTrip
    {
        /// <summary>
        /// is it a choir trip?
        /// </summary>
        private readonly bool _choirTrip;
        /// <summary>
        /// existing node
        /// </summary>
        private readonly JsTripNode _existing;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="tripKind"></param>
        public AddTrip(string tripKind)
        {
            InitializeComponent();

            DatePicker.SelectedDate = DateTime.Now.Date;
            DatePickerChoir.SelectedDate = DateTime.Now.Date;
            ComboBoxDriver.ItemsSource = ClientDataContext.GetInstance().ViewDriver;
            ComboBoxDriverChoir.ItemsSource = ClientDataContext.GetInstance().ViewDriver;
            ComboBoxChoir.ItemsSource = ClientDataContext.GetInstance().ViewChoirs;

            if (tripKind.Equals("Choir"))
            {
                _choirTrip = true;
                ColumnTrip.Width = new GridLength(0);
                ColumnChoirTrip.Width = new GridLength(this.Width);
            }
            else
            {
                _choirTrip = false;
                ColumnTrip.Width = new GridLength(this.Width);
                ColumnChoirTrip.Width = new GridLength(0);
                TextBoxPassenger.Focus();
            }
        }
        /// <summary>
        /// ctor with existing values
        /// </summary>
        public AddTrip(JsTripNode existing)
        {
            if (existing == null) return;
            _existing = existing;

            InitializeComponent();

            if (existing.NodeId.Contains("Choir"))
            {
                _choirTrip = true;
                ColumnTrip.Width = new GridLength(0);
                ColumnChoirTrip.Width = new GridLength(this.Width);
                ButtonSaveC.IsEnabled = false;
                ButtonSaveC.Visibility = Visibility.Hidden;
                ComboBoxChoir.ItemsSource = ClientDataContext.GetInstance().ViewChoirs;
                ComboBoxDriverChoir.ItemsSource = ClientDataContext.GetInstance().ViewDriver;

                DatePickerChoir.SelectedDate = existing.StartTime;
                ComboBoxChoir.SelectedValue = existing.Passenger;
                ComboBoxDriverChoir.SelectedValue = existing.DriverName;
                TextBoxArrivalTimeChoir.Text = existing.ExpectedArrival.ToString("HH:mm");
                TextBoxDestinationChoir.Text = existing.Destination;
                TextBoxExtraChoir.Text = existing.Description;
                TextBoxStartPointChoir.Text = existing.DeparturePlace;
                TextBoxStartTimeChoir.Text = existing.StartTime.ToString("HH:mm");
            }
            else
            {
                _choirTrip = false;
                ColumnTrip.Width = new GridLength(this.Width);
                ColumnChoirTrip.Width = new GridLength(0);
                TextBoxPassenger.Focus();
                ButtonSaveT.IsEnabled = false;
                ButtonSaveT.Visibility = Visibility.Hidden;
                ComboBoxDriver.ItemsSource = ClientDataContext.GetInstance().ViewDriver;

                DatePicker.SelectedDate = existing.StartTime;
                TextBoxPassenger.Text = existing.Passenger;
                ComboBoxDriver.SelectedValue = ClientDataContext.GetInstance().ViewDriver.FirstOrDefault(x => x.ShortCut.Equals(existing.DriverName));
                ComboBoxVehicle.SelectedValue = existing.Vehicle;
                TextBoxArrivalTime.Text = existing.ExpectedArrival.ToString("HH:mm");
                TextBoxDestination.Text = existing.Destination;
                TextBoxExtra.Text = existing.Description;
                TextBoxStartPoint.Text = existing.DeparturePlace;
                TextBoxStartTime.Text = existing.StartTime.ToString("HH:mm");
            }
        }
        /// <summary>
        /// handel save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            if(_choirTrip) CreateChoirTrip();
            else CreateTrip();

            ClearFields();

            if (_choirTrip) ComboBoxChoir.Focus();
            else TextBoxPassenger.Focus();
        }
        /// <summary>
        /// hande save and close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSaveExit_OnClick(object sender, RoutedEventArgs e)
        {
            if (_existing != null)
            {
                HandelExisting();
            }
            else if (_choirTrip) CreateChoirTrip();
            else CreateTrip();

            Close();
        }
        /// <summary>
        /// handle close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// create new choir tripnode
        /// </summary>
        private void CreateChoirTrip()
        {
            var result = new TripRawData();

            result.StartPoint = TextBoxStartPointChoir.Text;
            result.EndPoint = TextBoxDestinationChoir.Text;

            var selectedDate = DatePickerChoir.SelectedDate;

            if(selectedDate == null) return;

            result.StartTime = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, Convert.ToInt32(TextBoxStartTimeChoir.Text.Split(':')[0]), Convert.ToInt32(TextBoxStartTimeChoir.Text.Split(':')[1]), 0);
            result.ExpectedArrival = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, Convert.ToInt32(TextBoxArrivalTimeChoir.Text.Split(':')[0]), Convert.ToInt32(TextBoxArrivalTimeChoir.Text.Split(':')[1]), 0);

            result.Passenger = ((ChoirNode)ComboBoxChoir.SelectedValue).NodeId;

            if(ComboBoxDriverChoir.SelectedValue != null)
                result.DriverName = ((DriverNode) ComboBoxDriverChoir.SelectedValue).NodeId;
            result.Vehicle = (VehicleType) ComboBoxVehicleChoir.SelectedValue;
            result.Description = TextBoxExtraChoir.Text;

            Task.Factory.StartNew(() => new TripManager().CreateNewTripNode(result));
        }
        /// <summary>
        /// create new individual tripnode
        /// </summary>
        private void CreateTrip()
        {
            var result = new TripRawData();

            result.StartPoint = TextBoxStartPoint.Text;
            result.EndPoint = TextBoxDestination.Text;

            var selectedDate = DatePicker.SelectedDate;

            if (selectedDate == null) return;

            result.StartTime = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, Convert.ToInt32(TextBoxStartTime.Text.Split(':')[0]), Convert.ToInt32(TextBoxStartTime.Text.Split(':')[1]), 0);
            result.ExpectedArrival = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, Convert.ToInt32(TextBoxArrivalTime.Text.Split(':')[0]), Convert.ToInt32(TextBoxArrivalTime.Text.Split(':')[1]), 0);

            result.Passenger = TextBoxPassenger.Text;
            if (ComboBoxDriver.SelectedValue != null)
                result.DriverName = ((DriverNode)ComboBoxDriver.SelectedValue).NodeId;
            if(ComboBoxVehicle.SelectedValue != null)
                result.Vehicle = (VehicleType)ComboBoxVehicle.SelectedValue;
            result.Description = TextBoxExtra.Text;

            Task.Factory.StartNew(() => new TripManager().CreateNewTripNode(result));
        }
        /// <summary>
        /// change values
        /// </summary>
        private void HandelExisting()
        {
            if(_existing == null) return;

            if (_existing.NodeId.Contains("Choir"))
            {
                _existing.DeparturePlace = TextBoxStartPointChoir.Text;
                _existing.Destination = TextBoxDestinationChoir.Text;
                var selectedDate = DatePickerChoir.SelectedDate;
                _existing.StartTime = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, Convert.ToInt32(TextBoxStartTimeChoir.Text.Split(':')[0]), Convert.ToInt32(TextBoxStartTimeChoir.Text.Split(':')[1]), 0);
                _existing.ExpectedArrival = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, Convert.ToInt32(TextBoxArrivalTimeChoir.Text.Split(':')[0]), Convert.ToInt32(TextBoxArrivalTimeChoir.Text.Split(':')[1]), 0);
                if (ComboBoxDriverChoir.SelectedValue != null)
                    _existing.DriverName = ((DriverNode)ComboBoxDriverChoir.SelectedValue).NodeId;
                _existing.Vehicle = (VehicleType)ComboBoxVehicleChoir.SelectedValue;
                _existing.Description = TextBoxExtraChoir.Text;
            }
            else
            {
                _existing.DeparturePlace = TextBoxStartPoint.Text;
                _existing.Destination = TextBoxDestination.Text;
                var selectedDate = DatePicker.SelectedDate;
                _existing.StartTime = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, Convert.ToInt32(TextBoxStartTime.Text.Split(':')[0]), Convert.ToInt32(TextBoxStartTime.Text.Split(':')[1]), 0);
                _existing.ExpectedArrival = new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, Convert.ToInt32(TextBoxArrivalTime.Text.Split(':')[0]), Convert.ToInt32(TextBoxArrivalTime.Text.Split(':')[1]), 0);
                if (ComboBoxDriver.SelectedValue != null)
                    _existing.DriverName = ((DriverNode)ComboBoxDriver.SelectedValue).NodeId;
                _existing.Vehicle = (VehicleType)ComboBoxVehicle.SelectedValue;
                _existing.Description = TextBoxExtra.Text;
            }
            Task.Factory.StartNew(() => new TripManager().ModifyTripNode(_existing));
        }
        /// <summary>
        /// clear all input fields
        /// </summary>
        private void ClearFields()
        {
            ComboBoxChoir.SelectedValue = null;
            TextBoxStartPointChoir.Text = "";
            TextBoxDestinationChoir.Text = "";
            TextBoxStartTimeChoir.Text = "00:00";
            TextBoxArrivalTimeChoir.Text = "00:00";
            ComboBoxDriverChoir.SelectedValue = null;
            ComboBoxVehicleChoir.SelectedValue = null;
            TextBoxExtraChoir.Text = "";

            TextBoxPassenger.Text = "";
            TextBoxStartPoint.Text = "";
            TextBoxDestination.Text = "";
            TextBoxStartTime.Text = "00:00";
            TextBoxArrivalTime.Text = "00:00";
            ComboBoxDriver.SelectedValue = null;
            ComboBoxVehicle.SelectedValue = null;
            TextBoxExtra.Text = "";
        }
    }
}
