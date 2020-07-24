using PeerCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        string appToken;
        string ProfileImage;

        public ObservableCollection<PlansListModel> plan_id { get; set; }
        public Dashboard()
        {
            InitializeComponent();
            CheckInternet();
            GetNewNots();
            Permission();
            GetUserById();
            DashList.RefreshCommand = new Command(() => {
                //Do your stuff.    
                GetSubDetails();
                DashList.IsRefreshing = false;
            });
            //GetSubs();
            GetSubDetails();
            LblName.Text = HelperAppSettings.Name;

        }
        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }
        async void Permission()
        {
            await Permissions.RequestAsync<Permissions.Camera>();
            await Permissions.RequestAsync<Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            await Permissions.RequestAsync<Permissions.LocationAlways>();
            await Permissions.RequestAsync<Permissions.NetworkState>();
        }

        public async void GetNewNots()
        {
            try
            {
                HttpClient client = new HttpClient();
                var dashboardEndpoint = Helper.GetNotificationsUrl + HelperAppSettings.username + Helper.msgReadFilter;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                var result = await client.GetStringAsync(dashboardEndpoint);
                var NotsList = JsonConvert.DeserializeObject<NotificationsModel>(result);
                if(NotsList.countUnread.Contains("0"))
                {
                    DashImage.Source = "notification.png";
                }
                else
                {
                    DashImage.Source = "notificate.png";
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        

        public async void GetSubDetails()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }
            try
            {
                HttpClient client = new HttpClient();
                var UserCountEndpoint = Helper.getActiveSubUrl + HelperAppSettings.username;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var result = await client.GetStringAsync(UserCountEndpoint);
                var UsersCnt = JsonConvert.DeserializeObject<ActSubModel>(result);
                //Users = new ObservableCollection<AddedUsers>(UsersList);
                //var hut = UsersCnt.subscriptions;
                DashList.ItemsSource = UsersCnt.subscriptions;
            }
            catch (Exception)
            {
                return;
            }
        }


        public async void ViewSubTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var selectedUser = e.Item as SubscriptionModel;
            await Shell.Current.Navigation.PushAsync(new SinglePlan(selectedUser.subscription_id));

        }

        public async void ReadMoreClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPlan());
        }

        public async void Sub_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPlan());
        }

        public async void ProDash_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }

        public async void MemDash_Clicked(object sender, EventArgs e)
        {
            if (HelperAppSettings.priviledges.Contains("ADMIN"))
            {
                await Navigation.PushAsync(new ManageMembers());
            }
            else
            {
                await Navigation.PushAsync(new UserMemberPage());
            }
        }

        public async void Handle_FabClicked(object sender, EventArgs e)
        {
            //await ((Frame)sender).ScaleTo(0.8, length: 50, Easing.Linear);
            //await Task.Delay(100);
            //await ((Frame)sender).ScaleTo(1, length: 50, Easing.Linear);
            await Navigation.PushAsync(new NewPlan());
        }

        public async void changePassClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new changePassword());
        }

        public async void SharingClicked(object sender, EventArgs e)
        {
            try
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = "Community Name:" + " " + HelperAppSettings.community_name + "," + " " + "Community Code:" + " " + HelperAppSettings.community_code,
                    Title = "Share Community Code!!!"
                });
            }
            catch (Exception)
            {
                return;
            }
        }
        public async void NotifyIconClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            GetUserById();
            GetNewNots();
            GetSubDetails();
            try
            {
                appToken = await SecureStorage.GetAsync("refreshedToken");
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(appToken))
                {
                    User update = new User()
                    {
                        username = HelperAppSettings.username,
                        registrationToken = appToken
                    };
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                    var json = JsonConvert.SerializeObject(update);
                    HttpContent result = new StringContent(json);
                    result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PutAsync(Helper.MemRegToken, result);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("User App Registration Token sent successfully!");
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public async void SignOutClicked(object sender, EventArgs e)
        {
            try
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
            catch (Exception)
            {

            }
        }

        public void menuHider_clicked(object sender, EventArgs e)
        {
            if (menuHide.IsVisible == true)
            {
                menuHide.IsVisible = false;
                Frm1.IsVisible = false;
                Frm1b.IsVisible = true;
                chevUp.Source = "chevronUp.png";
            }
            else if (menuHide.IsVisible == false)
            {
                Frm1.IsVisible = true;
                Frm1b.IsVisible = false;
                chevUp.Source = "chevronDown.png";
                menuHide.IsVisible = true;
            }
        }

        public void SubHider_clicked(object sender, EventArgs e)
        {
            if (actSubHide.IsVisible == true)
            {
                actSubHide.IsVisible = false;
                Frm2.IsVisible = false;
                Frm2b.IsVisible = true;
                chvUp.Source = "chevronUp.png";
            }
            else if (actSubHide.IsVisible == false)
            {
                actSubHide.IsVisible = true;
                Frm2.IsVisible = true;
                Frm2b.IsVisible = false;
                chvUp.Source = "chevronDown.png";
            }

        }

        public async void GetUserById()
        {
            try
            {
                HttpClient client = new HttpClient();
                var UserdetailEndpoint = Helper.getMembersUrl + HelperAppSettings.id;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                var result = await client.GetStringAsync(UserdetailEndpoint);
                var MemberDetails = JsonConvert.DeserializeObject<MemberDetailsModel>(result);

                ProfileImage = MemberDetails.member[0].profile_img_url;

            }
            catch (Exception)
            {
                return;
            }
        }
     }
}