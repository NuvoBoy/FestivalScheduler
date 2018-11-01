using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace SchedulerApp
{
	public partial class App : Application
	{
        public ContentLoader SignalRClient = new ContentLoader();

		public App ()
		{
		    //show an error if the connection doesn't succeed for some reason
		    SignalRClient.Start().ContinueWith(task => {
		        if (task.IsFaulted)
		            MainPage.DisplayAlert("Error", "An error occurred when trying to connect to SignalR: " + task.Exception.InnerExceptions[0].Message, "OK");
		    });

		    //try to reconnect every 10 seconds, just in case
            Device.StartTimer(TimeSpan.FromSeconds(10), () => {
		        if (!SignalRClient.IsConnectedOrConnecting)
		            SignalRClient.Start();

		        return true;
		    });


            InitializeComponent();

		    MainPage = new NavigationPage(new MainPage(this));
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
