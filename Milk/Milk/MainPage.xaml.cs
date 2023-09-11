using Milk.Models;
using Milk.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Milk
{
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        private Browse browsePage;
        private Xamarin.Forms.NavigationPage browseNavigationPage; // Declare the navigation page

        public MainPage(GroceryUsers user)
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            var listPage = new Xamarin.Forms.NavigationPage(new List())
            {
                Title = "List",
                IconImageSource = "page1.png"
            };
            listPage.BarBackgroundColor = Color.FromHex("#D70040");
            listPage.BackgroundColor = Color.White;

            browsePage = new Browse();
            browseNavigationPage = new Xamarin.Forms.NavigationPage(browsePage) // Initialize the navigation page
            {
                Title = "Browse",
                IconImageSource = "browse.png",
                BackgroundColor = Color.White,
                BarBackgroundColor = Color.FromHex("#D70040")
            };

            var manualPage = new Xamarin.Forms.NavigationPage(new ManualAdd())
            {
                Title = "Add",
                IconImageSource = "page2.png"
            };
            manualPage.BarBackgroundColor = Color.FromHex("#D70040");
            manualPage.BackgroundColor = Color.FromHex("#CCFF00");

            var personalizePage = new Xamarin.Forms.NavigationPage(new Personalize())
            {
                Title = "Logout",
                IconImageSource = "page4.png"
            };
            personalizePage.BarBackgroundColor = Color.FromHex("#D70040");
            personalizePage.BackgroundColor = Color.FromHex("#FF1493");

            this.BarBackgroundColor = Color.FromHex("#D70040");

            Children.Add(listPage);
            Children.Add(browseNavigationPage); // Add the navigation page instead of the Browse page
            Children.Add(manualPage);
            Children.Add(personalizePage);

            // Handle the OnCurrentPageChanged event for the tabbed page
            this.CurrentPageChanged += OnCurrentPageChanged;
        }

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            if (CurrentPage == browseNavigationPage)
            {
                // Reset the navigation stack of the Browse page when switching to the Browse tab
                browsePage.Navigation.PopToRootAsync();
            }
        }

    }
}
