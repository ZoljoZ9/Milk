using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Milk.Droid.Renderers;

[assembly: ExportRenderer(typeof(Entry), typeof(NoUnderlineEntryRenderer))]
namespace Milk.Droid.Renderers
{
    public class NoUnderlineEntryRenderer : EntryRenderer
    {
        public NoUnderlineEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = null;
            }
        }
    }
}
