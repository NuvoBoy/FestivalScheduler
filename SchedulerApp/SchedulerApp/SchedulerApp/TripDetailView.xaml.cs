using FestivalScheduler.Data;
using Xamarin.Forms.Xaml;

namespace SchedulerApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TripDetailView
	{
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="node"></param>
	    public TripDetailView (JsTripNode node)
		{
            //create wrapper object
            var view =  new TripDetailWrapper
            {
                DepartureTime = node.StartTime,
                ExpectedArrival = node.ExpectedArrival,
                DeparturePlace = node.DeparturePlace,
                Destination = node.Destination,
                DriverName = node.DriverName
            };
            //start page
            InitializeComponent ();
            //fill page with data
		    ListViewTripDetail.ItemsSource = new[] {view};
		}
	}
}