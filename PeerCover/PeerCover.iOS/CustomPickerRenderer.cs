using PeerCover.CustomControls;
using PeerCover.iOS;
using System.Runtime.Remoting.Contexts;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace PeerCover.iOS
{
    public class CustomPickerRenderer : PickerRenderer
    {
        //public CustomPickerRenderer(Context context) : base(context)
        //{
        //    AutoPackage = false;
        //}
        //protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        //{
        //    base.OnElementChanged(e);
        //    if (Control != null)
        //    {
        //        Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
        //    }
        //}
    }
}