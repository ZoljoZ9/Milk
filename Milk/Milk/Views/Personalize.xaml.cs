using Xamarin.Essentials;
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
using Xamarin.Forms.PlatformConfiguration;
using System.Diagnostics;

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

            // Add feedback label
            AddFeedbackLabel();
        }

        private void AddFeedbackLabel()
        {
            Label feedbackLabel = new Label
            {
                Text = "Got feedback? Email me at matthew@zoljan.com.",
                TextColor = Color.Blue,
                TextDecorations = TextDecorations.Underline,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => { SendFeedbackEmail(); };
            feedbackLabel.GestureRecognizers.Add(tapGestureRecognizer);

            var scrollView = this.Content as ScrollView;
            if (scrollView != null)
            {
                var stackLayout = scrollView.Content as StackLayout;
                if (stackLayout != null)
                {
                    stackLayout.Children.Add(feedbackLabel);
                }
            }
        }


        private async void SendFeedbackEmail()
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "Feedback on Milk App",
                    Body = "Dear Matthew, \n\n", // Initial body. User can continue the email from here.
                    To = new List<string> { "matthew@zoljan.com" }
                };

                await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(() => Email.ComposeAsync(message));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EmailError: {ex.Message}");
                await DisplayAlert("Error", "Unable to send email. Check if you have an email client installed.", "OK");
            }
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
            App.Current.Properties.Clear(); // Clear Application properties
            await App.Current.SavePropertiesAsync(); // Save changes

            // Navigate back to the login page.
            Application.Current.MainPage = new NavigationPage(new Login());
        }
    }
}
