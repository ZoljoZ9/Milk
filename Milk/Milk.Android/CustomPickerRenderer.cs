using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using YourAppNamespace.Droid;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace YourAppNamespace.Droid
{
    public class CustomPickerRenderer : PickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = null; // This removes the underline
            }
        }
    }
}
