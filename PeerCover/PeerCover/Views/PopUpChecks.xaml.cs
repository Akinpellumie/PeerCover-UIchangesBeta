using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PeerCover.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpChecks
    {
        public PopUpChecks()
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
        }
    }
}