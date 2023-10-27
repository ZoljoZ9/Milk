using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace App6.iOS
{
    public class MyTabbedRenderer : TabbedRenderer
    {
        private TabbedPage tabbed;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
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

            try
            {
                var tabbarController = (UITabBarController)this.ViewController;
                if (null != tabbarController)
                {
                    tabbarController.ViewControllerSelected += OnTabbarControllerItemSelected;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void OnTabbarControllerItemSelected(object sender, UITabBarSelectionEventArgs eventArgs)
        {
            if (tabbed?.CurrentPage?.Navigation != null && tabbed.CurrentPage.Navigation.NavigationStack.Count > 0)
            {
                await tabbed.CurrentPage.Navigation.PopToRootAsync();
            }
        }
    }
}