using Milk.Models;
using Milk.ViewModels;
using Microsoft.Maui.Controls.Xaml;
using System;
using Milk.Views;
using System.ComponentModel;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk
{
    [DesignTimeVisible(false)]
    public partial class Login : ContentPage
    {
        private const string AdminUsername = "admin";
        private const string AdminPassword = "admin";

        public Login()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var users = await App.DbContext.GetUsersByEmail(usr.Text);

            if (users.Count == 0)
            {
                // If you wish to hard-code an admin for testing
                if (usr.Text == "admin" && psw.Text == "admin")
                {
                    var adminUser = new GroceryUsers
                    {
                        username = "admin",
                        email = "admin@example.com",
                        password = "admin",
                        IsAdmin = true
                    };
                    App.LoggedInUser = adminUser;
                    App.Current.MainPage = new MainPage(adminUser);
                    return;
                }

                await DisplayAlert("User doesn't exist or wrong credentials", "Either Password Or Username Are Wrong", "ok");
            }
            else
            {
                GroceryUsers loggedInUser = users[0];
                App.LoggedInUserId = loggedInUser.Id;
                App.LoggedInUser = loggedInUser;

                System.Diagnostics.Debug.WriteLine($"LoggedInUser after Login: {loggedInUser?.username}");

                // Navigate to the MainPage and pass the logged-in user.
                App.Current.MainPage = new MainPage(loggedInUser);
            }
        }



        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterPage());
        }


    }
}
