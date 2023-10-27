using Android.Content;
using Android.Views;
using Android.Widget;
using App6.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms.Platform.Android;
using Google.Android.Material.BottomNavigation;
using static Google.Android.Material.Navigation.NavigationBarView;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(MyTabbedRenderer))]
namespace App6.Droid
{
    public class MyTabbedRenderer : TabbedPageRenderer, AdapterView.IOnItemSelectedListener, IOnItemReselectedListener
    {


        public MyTabbedRenderer(Context context) : base(context)
        {

        }

        private TabbedPage tabbed;
        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                tabbed = (TabbedPage)e.NewElement;
            }
            else
            {
                tabbed = (TabbedPage)e.OldElement;
            }
            if (e.OldElement == null && e.NewElement != null)
            {
                for (int i = 0; i <= this.ViewGroup.ChildCount - 1; i++)
                {
                    var childView = this.ViewGroup.GetChildAt(i);
                    if (childView is ViewGroup viewGroup)
                    {
                        for (int j = 0; j <= viewGroup.ChildCount - 1; j++)
                        {
                            var childRelativeLayoutView = viewGroup.GetChildAt(j);
                            if (childRelativeLayoutView is BottomNavigationView)
                            {
                                ((BottomNavigationView)childRelativeLayoutView).SetOnItemReselectedListener(this);
                            }
                        }
                    }
                }
            }
        }

        public async void OnItemSelected(AdapterView parent, Android.Views.View view, int position, long id)
        {
            await tabbed.CurrentPage.Navigation.PopToRootAsync();
        }

        public void OnNothingSelected(AdapterView parent)
        {

        }
        public async void OnNavigationItemReselected(IMenuItem p0)
        {
            await tabbed.CurrentPage.Navigation.PopToRootAsync();
        }
    }
}