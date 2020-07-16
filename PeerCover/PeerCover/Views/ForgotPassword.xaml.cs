using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private async void ForgotPassClicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }
            if (string.IsNullOrEmpty(pswdReset.Text))
            {
                await DisplayAlert("Oops!", "Kindly input a username to proceed", "Ok");
                return;
            }
            await PopupNavigation.Instance.PushAsync(new PopAcctLoader());
            User update = new User()
            {
                username = pswdReset.Text,
            };
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Clear();
            //client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var json = JsonConvert.SerializeObject(update);
            HttpContent result = new StringContent(json);
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(Helper.forgotPswdUrl, result);

            if (response.IsSuccessStatusCode)
            {
                await PopupNavigation.Instance.PopAsync(true);
                await DisplayAlert("Success!", "Kindly check your mailbox for new password", "Ok");
                ContentPage fpm = new LoginPage();
                Application.Current.MainPage = fpm;
                indicator.IsVisible = false;
                indicator.IsRunning = false;

            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Whoopps!", " Server unavailable. Please try again later!", "Ok");
                    indicator.IsVisible = false;
                    indicator.IsRunning = false;
                }
                else
                {
                    indicator.IsRunning = false;
                    indicator.IsVisible = false;
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Whoopps!", "Please try again later", "Ok");

                }
            }
        }
    }
}