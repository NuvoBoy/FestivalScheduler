using System;
using Xamarin.Forms;

namespace SchedulerApp
{
	public partial class MainPage
	{
        /// <summary>
        /// instance of global app
        /// </summary>
	    private readonly App _parent;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="parent"></param>
	    public MainPage(App parent)
	    {
	        _parent = parent;
            InitializeComponent();
        }

        /// <summary>
        /// handel click on login button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
	    private async void ButtonOpenSchedul_OnClicked(object sender, EventArgs e)
	    {
	        await Navigation.PushAsync(new TabbedPage
	        {
	            Children =
	            {
	                new SchedulerView(_parent, DateTime.Now),
	                new SchedulerView(_parent,DateTime.Now + TimeSpan.FromDays(1))
	            }
	        });
	    }
    }
}
