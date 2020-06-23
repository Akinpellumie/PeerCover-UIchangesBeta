using System;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AstraWebView : ContentPage
    {
        public AstraWebView()
        {
            App.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();
            var astraWeb = Helper.astraUrl + TransHelper.referenceNumber;
            Payview.Source = astraWeb;
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
            base.OnBackButtonPressed();
            return false;
        }

    }
}