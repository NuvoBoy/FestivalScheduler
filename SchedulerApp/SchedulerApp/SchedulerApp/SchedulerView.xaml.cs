using System;
using FestivalScheduler.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SchedulerApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchedulerView
	{
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="day"></param>
	    public SchedulerView(App parent, DateTime day)
	    {
	        InitializeComponent();
            //set title
	        Title = day.Date == DateTime.Now.Date ? "Heute" : "Morgen";
            //load data and fill the page with it
	        ListViewTrips.ItemsSource = parent.SignalRClient.LoadTripNodes(day);
        }
        /// <summary>
        /// handle selection of one entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
	    private async void ListViewTrips_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
	    {
            //get selected item
	        var selected = ((ListView) sender).SelectedItem as JsTripNode;

            if(selected == null) return;

            //open detail page
	        await Navigation.PushAsync(new TripDetailView(selected));
	    }
    }
}