using Milk.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Milk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        public async void ButtonIsClicked(object sender, EventArgs args)
        {
            try
            {
                // Check if passwords match
                if (psw.Text != confirmPsw.Text)
                {
                    await DisplayAlert("Mismatch", "Passwords do not match", "OK");
                    return; // Exit the method early
                }

                // Check if a user with the same email already exists
                var existingUsers = await App.DbContext.GetUsersByEmail(eml.Text);

                if (existingUsers.Count > 0)
                {
                    // User with the same email already exists
                    await DisplayAlert("Registration Failed", "User already exists", "OK");
                }
                else
                {
                    // Additional validation checks here
                    // e.g., check if password meets minimum length requirement

                    var user = new GroceryUsers
                    {
                        username = usr.Text,
                        email = eml.Text,
                        password = psw.Text  // Ensure that in production, you use hashed and salted passwords
                    };

                    await App.DbContext.InsertUser(user);
                    App.LoggedInUser = user; // Setting the App's LoggedInUser property

                    await DisplayAlert("Success", "User successfully registered", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
            }
        }



        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Login());
        }
        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}
