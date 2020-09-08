using Xamarin.Forms;
using PeerCover.Services;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using PeerCover.Views;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using System.Net.Http;
using System;

namespace PeerCover
{
    public partial class App : Application
    {

        public static Page currentpage { get; private set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new SplashPage());
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=3c27309c-31da-42d5-9663-a893747476d0;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            CheckInternet();
            //Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        
        protected override void OnResume()
        {
            // Handle when your app resumes
            
        }

        async void CheckInternet()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.None)
                {
                    await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        async void CheckToken()
        {
            try
            {
                HttpClient client = new HttpClient();
                var UserdetailEndpoint = Helper.getMembersUrl + HelperAppSettings.id;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var result = await client.GetAsync(UserdetailEndpoint);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch (Exception)
            {
                return;
            }
        }

    }
}