﻿using PeerCover.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PendingRequest : ContentPage
    {
        public PendingRequest()
        {
            InitializeComponent();
            GetPenRequests();
            CheckInternet();
            PendingRequestList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetPenRequests();
                PendingRequestList.IsRefreshing = false;
            });
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

        public async void GetPenRequests()
        {
            indicator.IsRunning = true;
            indicator.IsVisible = true;

            try
            {
                HttpClient client = new HttpClient();
                var dashboardEndpoint = Helper.GetRequestsUrl + HelperAppSettings.community_code + Helper.getRequestFilter + "pending";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                var result = await client.GetStringAsync(dashboardEndpoint);
                var requestsList = JsonConvert.DeserializeObject<ReqModel>(result);
                PendingRequestList.ItemsSource = requestsList.requests;

                if (requestsList.requests.Count == 0)
                {
                    PenStack.IsVisible = true;
                    PenReStack.IsVisible = false;
                }
                else
                {
                    PenStack.IsVisible = false;
                    PenReStack.IsVisible = true;
                }
                indicator.IsRunning = false;
                indicator.IsVisible = false;

            }
            catch (Exception)
            {
                await DisplayAlert("Oops!", " Check Your Internet Connection", "Ok");
                return;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetPenRequests();
        }

        public void PendingTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as RequestsModel;
            PopupNavigation.Instance.PushAsync(new PopUpRequests(selectedUser.request_id));
            MessagingCenter.Subscribe<RequestsModel>(this, "PopUpData", (value) => { });
            GetPenRequests();
            //await Shell.Current.Navigation.PushAsync(new PopUp(selectedUser.id));

        }
    }
}