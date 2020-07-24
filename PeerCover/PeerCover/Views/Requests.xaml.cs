﻿using PeerCover.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Requests : ContentPage
    {
        public Requests()
        {
            InitializeComponent();
            GetRequests();
            CheckInternet();
            AllRequestList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetRequests();
                AllRequestList.IsRefreshing = false;
            });
        }
        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

        public async void GetRequests()
        {

            try
            {
                indicator.IsRunning = true;
                indicator.IsVisible = true;

                HttpClient client = new HttpClient();
                var dashboardEndpoint = Helper.GetRequestsUrl + HelperAppSettings.community_code;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                var result = await client.GetStringAsync(dashboardEndpoint);
                var requestsList = JsonConvert.DeserializeObject<ReqModel>(result);
                AllRequestList.ItemsSource = requestsList.requests;

                if (requestsList.requests.Count == 0)
                {
                    empStack.IsVisible = true;
                    ReqStack.IsVisible = false;
                }
                else
                {
                    empStack.IsVisible = false;
                    ReqStack.IsVisible = true;
                }

                indicator.IsRunning = false;
                indicator.IsVisible = false;
            }
            catch (Exception)
            {
                await DisplayAlert("Oops!", "check your internet connection", "Ok");
                return;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetRequests();
        }

        async void AcceptedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AcceptedRequest());
        }
        async void PendingClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PendingRequest());
        }
        async void DeclinedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeclinedRequest());
        }

        public void RequestTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (e.Item == null) return;
                var selectedUser = e.Item as RequestsModel;
                if (selectedUser.status.Contains("Accepted"))
                {
                    DisplayAlert("Oops!", "Request Accepted Already!", "Ok");
                    return;
                }
                else if (selectedUser.status.Contains("Declined"))
                {
                    DisplayAlert("Oops!", "Request Declined Already", "Ok");
                }
                else if (selectedUser.status.Contains("Pending"))
                {
                    PopupNavigation.Instance.PushAsync(new PopUpRequests(selectedUser.request_id));
                    MessagingCenter.Subscribe<RequestsModel>(this, "PopUpData", (value) => { });
                    GetRequests();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

    }
}