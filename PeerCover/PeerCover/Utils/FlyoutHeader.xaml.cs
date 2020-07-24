using Newtonsoft.Json;
using PeerCover.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Utils
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutHeader : ContentView
    {
        string ProfileImage;
        string fullname;
        string username;
        string commName;

        public FlyoutHeader()
        {
            InitializeComponent();
            GetUserById();
        }

        public async void GetUserById()
        {
            HttpClient client = new HttpClient();
            var UserdetailEndpoint = Helper.getMembersUrl + HelperAppSettings.id;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

            var result = await client.GetStringAsync(UserdetailEndpoint);
            var MemberDetails = JsonConvert.DeserializeObject<MemberDetailsModel>(result);
            fullname = MemberDetails.member[0].capName;
            FlyFullname.Text = fullname;
            username = MemberDetails.member[0].username;
            FlyUsername.Text = username;
            //commName = MemberDetails.member[0].community_name;
            //FlyCommName.Text = commName;
            ProfileImage = MemberDetails.member[0].profile_img_url;

            if (string.IsNullOrEmpty(ProfileImage))
            {
                AshImage.Source = "undrawPro.svg";
            }
            else
            {
                var imgUrl = Helper.ImageUrl + ProfileImage;
                AshImage.Source = imgUrl;
            }
        }
    }
}