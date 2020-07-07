using Newtonsoft.Json;
using PeerCover.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PeerCover.ViewModels;
using Rg.Plugins.Popup.Services;
using PeerCover.Utils;
using Xamarin.Essentials;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            InitializeComponent();
             BindingContext = this;
            CheckInternet();
        }
        async void CheckInternet()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
            }
        }
        public async void CreateUserBtn_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await PopupNavigation.Instance.PushAsync(new PopUpNoInternet());
                return;
            }
            await PopupNavigation.Instance.PushAsync(new PopLoader());
            try
            {

                if (string.IsNullOrEmpty(FTInput.Text) || string.IsNullOrEmpty(USNInput.Text) || string.IsNullOrEmpty(PWDInput.Text) || string.IsNullOrEmpty(EAInput.Text) || string.IsNullOrEmpty(CCInput.Text) || string.IsNullOrEmpty(LTInput.Text) || string.IsNullOrEmpty(PNInput.Text))
                {
                    await PopupNavigation.Instance.PopAsync(true);
                    await DisplayAlert("Alert", "Input fields cannot be empty", "Ok");
                    return;
                }
                AddMemberModel members = new AddMemberModel(FTInput.Text, LTInput.Text, USNInput.Text, EAInput.Text, PNInput.Text, CCInput.Text, PWDInput.Text)
                {
                    firstname = FTInput.Text.Trim(),
                    username = USNInput.Text.Trim(),
                    lastname = LTInput.Text.Trim(),
                    email = EAInput.Text.Trim(),
                    phonenumber = PNInput.Text.Trim(),
                    communityCode = CCInput.Text.Trim(),
                    password = PWDInput.Text.Trim()

                };
                if (members.CheckInformation())

                {
                    var client = new HttpClient();

                    var json = JsonConvert.SerializeObject(members);
                    HttpContent result = new StringContent(json);
                    result.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Helper.SignUpUrl, result);

                    if (response.IsSuccessStatusCode)
                    {
                        //await DisplayAlert("InHub", "Registration Succesful", "Ok");
                        await Navigation.PushModalAsync(new SignUpConfirmation());
                        await PopupNavigation.Instance.PopAsync(true);
                        FTInput.Text = "";
                        LTInput.Text = "";
                        USNInput.Text = "";
                        EAInput.Text = "";
                        PNInput.Text = "";
                        CCInput.Text = "";
                        PWDInput.Text = "";

                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            await PopupNavigation.Instance.PopAsync(true);
                            await DisplayAlert("Oops!", "Server Unavailable at the moment. Please try again later!", "Ok");
                        }
                        else
                        {
                            await PopupNavigation.Instance.PopAsync(true);
                            await DisplayAlert("Oops!", "Connection Timeout. Please try again later", "Ok");

                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public async void Username_Unfocused(object sender, FocusEventArgs e)
        {
            //Loader.IsVisible = true;
            try
            {

                var url = Helper.UsernameAvailability + USNInput.Text;
                HttpClient client = new HttpClient();
                var result = await client.GetStringAsync(url);
                //var UsersList = JsonConvert.DeserializeObject<MembersListModel>(result);
                if (result.Contains("Username is available"))
                {
                    ///Loader.IsVisible = false;
                    await DisplayAlert("Success", "Username is Available", "Ok");
                }
                else if (result.Contains("Username is not available"))
                {
                    //Loader.IsVisible = false;
                    await DisplayAlert("Attention", "Username already Exist", "Ok");
                    LblUser.TextColor = Color.Red;
                    LblUsn.Text = "Username already exist. Try another one*";
                    USNInput.TextColor = Color.Red;
                    //USNInput = Color.Red;
                }

                if (string.IsNullOrEmpty(FTInput.Text) || string.IsNullOrEmpty(LTInput.Text) || string.IsNullOrEmpty(USNInput.Text) || string.IsNullOrEmpty(PNInput.Text))
                {
                    StackBtn1.IsVisible = true;
                    StackBtn2.IsVisible = false;
                }
                else
                {
                    StackBtn2.IsVisible = true;
                    StackBtn1.IsVisible = false;
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        public void BackToLoginPressed(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        public void Input2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (e.NewTextValue.Length >= 1)
                {
                    //UsnFrm.BorderColor = Color.Accent;
                    USNInput.TextColor = Color.Default;
                    LblUser.TextColor = Color.Default;
                    LblUsn.Text = "";
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitStates();
        }

        public void FirstName_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(FTInput.Text) || string.IsNullOrEmpty(LTInput.Text) || string.IsNullOrEmpty(USNInput.Text) || string.IsNullOrEmpty(PNInput.Text))
            {
                StackBtn1.IsVisible = true;
                StackBtn2.IsVisible = false;
            }
            else
            {
                StackBtn2.IsVisible = true;
                StackBtn1.IsVisible = false;
            }
        }

        public void LastName_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(FTInput.Text) || string.IsNullOrEmpty(LTInput.Text) || string.IsNullOrEmpty(USNInput.Text) || string.IsNullOrEmpty(PNInput.Text))
            {
                StackBtn1.IsVisible = true;
                StackBtn2.IsVisible = false;
            }
            else
            {
                StackBtn2.IsVisible = true;
                StackBtn1.IsVisible = false;
            }
        }

        public void PhoneNumber_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(FTInput.Text) || string.IsNullOrEmpty(LTInput.Text) || string.IsNullOrEmpty(USNInput.Text) || string.IsNullOrEmpty(PNInput.Text))
            {
                StackBtn1.IsVisible = true;
                StackBtn2.IsVisible = false;
            }
            else
            {
                StackBtn2.IsVisible = true;
                StackBtn1.IsVisible = false;
            }
        }

        public void Email_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(EAInput.Text) || string.IsNullOrEmpty(CCInput.Text) || string.IsNullOrEmpty(PWDInput.Text) || TermsCheck.IsChecked==false)
            {
                BtnClick1.IsVisible = true;
                BtnClick2.IsVisible = false;
            }
            else
            {
                BtnClick2.IsVisible = true;
                BtnClick1.IsVisible = false;
            }
        }

        public void CommCode_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(EAInput.Text) || string.IsNullOrEmpty(CCInput.Text) || string.IsNullOrEmpty(PWDInput.Text) || TermsCheck.IsChecked == false)
            {
                BtnClick1.IsVisible = true;
                BtnClick2.IsVisible = false;
            }
            else
            {
                BtnClick2.IsVisible = true;
                BtnClick1.IsVisible = false;
            }
        }

        public void Password_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(EAInput.Text) || string.IsNullOrEmpty(CCInput.Text) || string.IsNullOrEmpty(PWDInput.Text) || TermsCheck.IsChecked == false)
            {
                BtnClick1.IsVisible = true;
                BtnClick2.IsVisible = false;
            }
            else
            {
                BtnClick2.IsVisible = true;
                BtnClick1.IsVisible = false;
            }
        }
        void InitStates()
        {
            var stateGroup = new VisualStateGroup
            {
                Name = "StrengthStates",
                TargetType = typeof(Label)
            };

            stateGroup.States.Add(CreateState("Blank", "", Color.White));
            stateGroup.States.Add(CreateState("VeryWeak", "*", Color.Red));
            stateGroup.States.Add(CreateState("Weak", "**", Color.Orange));
            stateGroup.States.Add(CreateState("Medium", "***", Color.FromHex("FFDF00")));
            stateGroup.States.Add(CreateState("String", "****", Color.FromHex("2FCF3F")));
            stateGroup.States.Add(CreateState("VeryStrong", "*****", Color.FromHex("2FCF8F")));

            VisualStateManager.SetVisualStateGroups(this.StrengthIndicator, new VisualStateGroupList { stateGroup });

        }


        void Handle_TextChanged(object sender, TextChangedEventArgs e)
        {
            var strength = PasswordAdvisor.CheckStrength(e.NewTextValue);
            var strengthName = Enum.GetName(typeof(PasswordScore), strength);
            VisualStateManager.GoToState(this.StrengthIndicator, strengthName);
        }


        string strength;

        public string Strength
        {
            get => strength;
            set
            {
                strength = value;
                OnPropertyChanged(nameof(Strength));
            }
        }

        static VisualState CreateState(string strength, string text, Color color)
        {
            var textSetter = new Setter { Value = text, Property = Label.TextProperty };
            var colorSetter = new Setter { Value = color, Property = Label.TextColorProperty };

            return new VisualState
            {
                Name = strength,
                TargetType = typeof(Label),
                Setters = { textSetter, colorSetter }
            };
        }


        public void ShowSecondStack_Clicked(object sender, EventArgs e)
        {
            FirstStack.IsVisible = false;
            SecondStack.IsVisible = true;
        }
    }
}