using System;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayWithCard : ContentPage
    {
        double amount;
        string name = HelperAppSettings.Name;
        string fname = HelperAppSettings.firstname;
        string lname = HelperAppSettings.lastname;
        string phone_number = HelperAppSettings.phonenumber;
        string email = HelperAppSettings.email;
        string consumer_mac = HelperAppSettings.AndroidId;
        string tx_ref = TransHelper.transactionId;
        int consumer_id = Int32.Parse(HelperAppSettings.id);
        public PayWithCard(double pre)
        {
            amount = pre;
            App.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();
            var CardUrl = Helper.FlutterWebPayUrl + HelperAppSettings.username + "&amount=" + amount + "&ref=" + tx_ref + "&email=" + email + "&fname=" + fname + "&lname=" + lname;
            PayCard.Source = CardUrl;
        }

        private void Payview_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Indic.IsVisible = true;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Indic.ProgressTo(0.9, 900, Easing.SpringIn);
        }

        private void Payview_Navigated(object sender, WebNavigatedEventArgs e)
        {
            Indic.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                if (HelperAppSettings.priviledges.Contains("ADMIN"))
                {
                    AppShell fpm = new AppShell();
                    Xamarin.Forms.Application.Current.MainPage = fpm;
                }
                else
                {
                    Xamarin.Forms.Application.Current.MainPage = new AppShellUser();
                }
                //Shell.Current.Navigation.PopModalAsync();
                return true;
            }
            catch (Exception)
            {
                return true;
            }

        }

    }
}