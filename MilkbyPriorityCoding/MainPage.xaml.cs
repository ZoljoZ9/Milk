using Milk.Models;
using Milk.Views;
using Milk.Views.add;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.VisualBasic;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk
{
    public partial class MainPage : Microsoft.Maui.Controls.TabbedPage
    {
        private Browse browsePage;
        private Microsoft.Maui.Controls.NavigationPage browseNavigationPage; // Declare the navigation page

        public MainPage(GroceryUsers user)
        {
            InitializeComponent();

            On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            var listPage = new Microsoft.Maui.Controls.NavigationPage(new List())
            {
                Title = "List",
                IconImageSource = "page1.png"
            };
            listPage.BarBackgroundColor = Color.FromArgb("#D70040");
            listPage.BackgroundColor = Colors.White;

            browsePage = new Browse();
            browseNavigationPage = new Microsoft.Maui.Controls.NavigationPage(browsePage)
            {
                Title = "Browse",
                IconImageSource = "browse.png",
                BackgroundColor = Colors.White,
                BarBackgroundColor = Color.FromArgb("#D70040")
            };

            // ...

            var manualPage = new Microsoft.Maui.Controls.NavigationPage(new ManualAdd())
            {
                Title = "Add",
                IconImageSource = "page2.png"
            };
            manualPage.BarBackgroundColor = Color.FromArgb("#D70040");
            manualPage.BackgroundColor = Colors.White;

            var personalizePage = new Microsoft.Maui.Controls.NavigationPage(new Personalize())
            {
                Title = "Logout",
                IconImageSource = "page4.png"
            };
            personalizePage.BarBackgroundColor = Color.FromArgb("#D70040");
            personalizePage.BackgroundColor = Colors.White;

            this.BarBackgroundColor = Color.FromArgb("#D70040");

            Children.Add(listPage);
            Children.Add(browseNavigationPage); // Add the navigation page instead of the Browse page
            Children.Add(manualPage);
            Children.Add(personalizePage);

            // Handle the OnCurrentPageChanged event for the tabbed page
            CurrentPageChanged += MainPage_CurrentPageChanged;
        }

        private async void OnCurrentPageChanged()
        {
            if (CurrentPage == browseNavigationPage && browseNavigationPage.Navigation.NavigationStack.Count > 1)
            {
                Console.WriteLine("CurrentPageChanged event triggered.");
                await browseNavigationPage.PopToRootAsync();
            }
        }

        private async void MainPage_CurrentPageChanged(object sender, EventArgs e)
        {
            if (CurrentPage == browseNavigationPage && browseNavigationPage.Navigation.NavigationStack.Count > 1)
            {
                Console.WriteLine("CurrentPageChanged event triggered.");
                await browseNavigationPage.PopToRootAsync();
            }
        }

    }
}
