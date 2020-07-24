using System;
using Xamarin.Forms;
using System.Windows.Input;
using PeerCover.Views;

namespace PeerCover
{
    public partial class AppShellNewUser : Shell
    {
        public AppShellNewUser()
        {
            InitializeComponent();
            myBtn.Clicked += (sender, e) => {
                LogoutExit();
            };
          
        }

        public ICommand LogoutCommand => new Command(LogoutExit);

        private async void LogoutExit()
        {
            bool result = await DisplayAlert("Hey!", "Are you sure you want to sign out?", "Yes", "No");
            if (result == true)
            {
                HelperAppSettings.Token = "";
                HelperAppSettings.firstname = "";
                HelperAppSettings.lastname = "";
                HelperAppSettings.username = "";
                HelperAppSettings.email = "";
                HelperAppSettings.phonenumber = "";
                HelperAppSettings.community_code = "";
                HelperAppSettings.community_name = "";
                HelperAppSettings.id = "";
                HelperAppSettings.profile_img_url = "";
                HelperAppSettings.priviledges = "";
                HelperAppSettings.capName = "";
                HelperAppSettings.Name = "";
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                return;
            }

        }

        protected void onAppearing(object sender, EventArgs e)
        {
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }

    }
}