using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.Xaml;
using WebView = Xamarin.Forms.WebView;

namespace PeerCover.Utils
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayWithCardWeb : ContentPage
    {
        double amount;
        string name = HelperAppSettings.Name;
        string phone_number = HelperAppSettings.phonenumber;
        string email = HelperAppSettings.email;
        string consumer_mac = HelperAppSettings.AndroidId;
        string tx_ref = TransHelper.transactionId;
        int consumer_id = Int32.Parse(HelperAppSettings.id);
        public PayWithCardWeb(double pre)
        {
            amount = pre;
            App.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();
            
        }

        private  void Payview_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Indic.IsVisible = true;
        }

        //public async void onPageFinished(WebView view, String url)
        //{
        //    String user = "u";
        //    String pwd = "p";
        //    string result = await browser.EvaluateJavaScriptAsync($"makePayment('{amount}')");
        //    view.loadUrl("javascript:document.getElementById('username').value = '" + user + "';document.getElementById('password').value='" + pwd + "';");
        //}
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Indic.ProgressTo(0.9, 900, Easing.SpringIn);
            var browser = new Xamarin.Forms.WebView();
            var htmlSource = new HtmlWebViewSource();

            htmlSource.Html = @"<html>
                                <head>
                                <link rel=""stylesheet"" href=""default.css"">
                                </head>
                                <body>
                                   <form action=""local.html"" method=""get"" target=""_blank"">
                                        <button type =""submit"">Continue to Payment Dashboard</button>
                                  </form>
                                </body>
                                </html>";

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            browser.Source = htmlSource;
            Content = browser;

            string result = await browser.EvaluateJavaScriptAsync($"makePayment('{amount}')");
        }

        //private async void FlutterPay_Clicked(object sender, EventArgs e)
        //{
        //    FirstBar.IsVisible = false;
        //    string result = await Payview.EvaluateJavaScriptAsync($"makePayment('{amount}')");
        //    Payview.Source = new HtmlWebViewSource()
        //    {
        //        Html =
        //            $@"<html>" +
        //            "<head>" +
        //            "<title>webViewSample</title>" +
        //            "<link rel=\"stylesheet\" type=\"text/css\" href=\"default.css\" />" +
        //            "</head>" +
        //            "<body>" +
        //            "<form>" +
        //            "<script src=\"https://checkout.flutterwave.com/v3.js\"></script>" +
        //            "</form>" +
        //            "<script type=\"text/javascript\">" +
        //            "window.onload = function makePayment(amount) {" +
        //            "FlutterwaveCheckout({" +
        //            "public_key: \"FLWPUBK_TEST-31d61a13026483fc38f15f0e90232374-X\" " +
        //            "tx_ref: \"hooli-tx-1920bbtyt\" " +
        //            "amount: amount;" +
        //            "currency: \"NGN\"" +
        //            "payment_options: \"card,mobilemoney,ussd\"" +
        //            "redirect_url: \"https://distracted-keller-c58be8.netlify.app/\" " +
        //            "meta: {" +
        //            "consumer_id: \"23\" " +
        //            "consumer_mac: \"92a3-912ba-1192a\" " +
        //            "}" +
        //            "customer: {" +
        //            "email: \"akinlade3195@gmail.com\" " +
        //            "phone_number: \"07037221814\" " +
        //            "name: \"Akinlade Akinpelumi\" " +
        //            "}" +
        //            "callback: function(data) {" +
        //            "console.log(data);" +
        //            "}" +
        //            "customizations: {" +
        //            "title: \"My store\" " +
        //            "description: \"Payment for items in cart\" " +
        //            "logo: \"https://assets.piedpiper.com/logo.png\" " +
        //            "}" +
        //            "});" +
        //            "}" +
        //            "</script>" +
        //            "</body>" +

        //            "</html>"
        //    };

        //}
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