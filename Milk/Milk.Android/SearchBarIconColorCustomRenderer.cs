using Android.Content;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.SearchBar), typeof(MyApp.Renderers.SearchBarIconColorCustomRenderer))]
namespace MyApp.Renderers
{
    public class SearchBarIconColorCustomRenderer : SearchBarRenderer
    {
        public SearchBarIconColorCustomRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            var icon = Control?.FindViewById(Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null));
            (icon as ImageView)?.SetColorFilter(Color.Gray.ToAndroid());
        }
    }
}