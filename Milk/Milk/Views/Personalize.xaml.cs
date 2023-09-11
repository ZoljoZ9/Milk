using Milk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milk.Persistence;
using Milk.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Milk.ViewModels;
using System.Collections.ObjectModel;
using System.Data.Common;
using SQLite;
using Milk.Data;


namespace Milk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Personalize : ContentPage
    {
        public PersonalizeViewModel viewModel { get; set; }

        public Personalize()
        {
            InitializeComponent();
            LoadUserDataAsync(); // Call an async method to load user data
        }

        private async void LoadUserDataAsync()
        {
            int userId = App.LoggedInUserId;
            var dbContext = new AppDbContext(App.DatabasePath);
            var user = await dbContext.Database.Table<GroceryUsers>().Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (user != null)
            {
                viewModel = new PersonalizeViewModel
                {
                    Username = user.username
                    //... [other properties]
                };
            }
            else
            {
                viewModel = new PersonalizeViewModel();
            }

            BindingContext = viewModel;
        }


        private async void LogoutClicked(object sender, EventArgs e)
        {
            // Clear session data (if any).
            // This might be user preferences, access tokens, etc.
            App.Current.Properties.Clear(); // Clear Application properties
            await App.Current.SavePropertiesAsync(); // Save changes

            // Navigate back to the login page.
            // Assuming you have a LoginPage in your Views, otherwise change this to your actual login page.
            Application.Current.MainPage = new NavigationPage(new Login());
        }

    }
}

  
