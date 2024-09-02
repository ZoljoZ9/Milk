using Milk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milk.Persistence;
using Milk.Views;
using Microsoft.Maui.Controls.Xaml;
using Milk.ViewModels;
using System.Collections.ObjectModel;
using System.Data.Common;
using SQLite;
using Milk.Data;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;

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
                TextColor = Colors.Blue,
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
                var stackLayout = scrollView.Content as Microsoft.Maui.Controls.StackLayout;
                if (stackLayout != null)
                {
                    stackLayout.Children.Add(feedbackLabel);
                }
            }
        }

        private async void DeleteProfileClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete your profile?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    // Assuming App.LoggedInUser contains the current user's details
                    int userId = App.LoggedInUser.Id;

                    // Delete the user from the database
                    await App.DbContext.DeleteUserById(userId);

                    // Clear the logged-in user
                    App.LoggedInUser = null;

                    await DisplayAlert("Deleted", "Your profile has been deleted", "OK");

                    // Navigate back to login or home page
                    await Navigation.PushModalAsync(new Login());
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
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

                await Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(() => Email.ComposeAsync(message));
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
            // Clear the logged-in user
            App.LoggedInUser = null;

            // Optionally, you might also want to clear user data from local storage or database here

            // Navigate back to the login page or another appropriate page
            await Navigation.PushModalAsync(new Login());
        }
    }
}
