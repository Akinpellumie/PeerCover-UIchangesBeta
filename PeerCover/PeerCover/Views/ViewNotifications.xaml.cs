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

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewNotifications : ContentPage
    {
        int isRd;
        string NotclaimId;
        string notifyId;
        string dateMate;
        string claimStats;

        public string DateFormat
        {
            get
            {

                var r = DateTime.Parse(this.dateMate.Replace("[UTC]", ""));
                return r.ToLocalTime().ToShortDateString();
            }

        }
        string LblDate => this.DateFormat;
        public ViewNotifications(string id)
        {
            InitializeComponent();
            LoadSingleMsg(id);
        }

        public async void LoadSingleMsg(string id)
        {
            try
            {
                var url = Helper.GetNotificationsById + id;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                var result = await client.GetStringAsync(url);
                var MsgsList = JsonConvert.DeserializeObject<NotificationsModel>(result);
                SingleMsgDetails.BindingContext = MsgsList.notifications[0];
                NotclaimId = MsgsList.notifications[0].link_type_id;
                notifyId = MsgsList.notifications[0].id;
                isRd = MsgsList.notifications[0].is_read;
                dateMate = MsgsList.notifications[0].date_sent;
                //LblMsgdate.Text = LblDate;

                if (!string.IsNullOrEmpty(NotclaimId))
                {
                    HttpClient myclient = new HttpClient();
                    var dashboardEndpoint = Helper.GetSingleClaimUrl + NotclaimId;
                    myclient.DefaultRequestHeaders.Clear();
                    myclient.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);
                    var myresult = await client.GetStringAsync(dashboardEndpoint);
                    var myClaimsList = JsonConvert.DeserializeObject<SingleClaimListModel>(myresult);
                    claimStats = myClaimsList.claim[0].status;
                }

                if (notifyId != null && isRd == 0)
                {
                    UpdateNotification update = new UpdateNotification()
                    {
                        username = HelperAppSettings.username,
                        isRead = 1,
                        notificationId = notifyId,
                    };

                    var clientee = new HttpClient();
                    clientee.DefaultRequestHeaders.Clear();
                    clientee.DefaultRequestHeaders.Add("Authorization", Helper.userprofile.token);

                    var json = JsonConvert.SerializeObject(update);
                    HttpContent resulted = new StringContent(json);
                    resulted.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var responseee = client.PutAsync(Helper.GetNotificationsById, resulted);
                    

                }
                else
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

      
        public async void LoadSingleClaim(object sender, EventArgs e)
        {
            try
            {
                if (claimStats.Contains("Recommendation Accepted"))
                {
                    await Shell.Current.Navigation.PushAsync(new SingleClaim(NotclaimId));
                }
                else if (claimStats.Contains("Recommendation Rejected"))
                {
                    await Shell.Current.Navigation.PushAsync(new SingleClaim(NotclaimId));
                }
                else
                {
                    await Shell.Current.Navigation.PushAsync(new ClaimResponse(NotclaimId));
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}