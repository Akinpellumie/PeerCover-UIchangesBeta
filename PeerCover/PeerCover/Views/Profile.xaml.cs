using PeerCover.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public string ComName
        {
            get
            {
                return HelperAppSettings.community_code;
            }
            set
            {
                CommName.Text = HelperAppSettings.community_code;
            }
        }
        public Profile()
        {
            InitializeComponent();
            GetUserById();
            CheckInternet();
            //PageName.BindingContext = HelperAppSettings.capName;
            //UserImagePro.BindingContext = HelperAppSettings.profile_img_url;
        }

        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }

        public async void GetUserById()
        {
            HttpClient client = new HttpClient();
            var UserdetailEndpoint = Helper.getMembersUrl + HelperAppSettings.id;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserdetailEndpoint);
            var MemberDetails = JsonConvert.DeserializeObject<MemberDetailsModel>(result);

            PageName.BindingContext = MemberDetails.member[0];
            UserName.BindingContext = MemberDetails.member[0];
            emailTxt.Text = MemberDetails.member[0].email;
            dateJoined.Text = "Joined on" + " " + MemberDetails.member[0].Pr_date;
            CommName.Text = MemberDetails.member[0].community_name;
            var ProfileImage = MemberDetails.member[0].profile_img_url;

            if (string.IsNullOrEmpty(ProfileImage))
            {
                UserImagePro.Source = "undrawMale.svg";
                HeaderImg.Source = "undrawMale.svg";
            }
            else
            {
                UserImagePro.Source = Helper.ImageUrl + ProfileImage;
                HeaderImg.Source = Helper.ImageUrl + ProfileImage;
            }

        }

        async void EditProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProfile());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetUserById();
        }

        public async void changePassClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new changePassword());
        }

        public async void SharingClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = "Community Name:" + " " + HelperAppSettings.community_name + "," + " " + "Community Code:" + " " + HelperAppSettings.community_code,
                Title = "Share Community Code!!!"
            });
        }
        public async void NotifyIconClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());
        }

        public async void SignOutClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Hey!", "Are you sure you want to sign out?", "Yes", "No");
            if (result == true)
            {
                ContentPage fpm = new LoginPage();
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
                Application.Current.MainPage = fpm;
            }
            else
            {
                return;
            }

        }
    }
}