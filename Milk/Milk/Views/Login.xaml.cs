using Milk.Models;
using Milk.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Milk.Views;
using System.ComponentModel;

namespace Milk
{
    [DesignTimeVisible(false)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var users = await App.DbContext.GetUsersByEmail(usr.Text);

            if (users.Count == 0)
            {
                await DisplayAlert("Wrong credentials", "Either Password Or Username Are Wrong", "ok");
            }
            else
            {
                GroceryUsers loggedInUser = users[0];
                App.LoggedInUserId = loggedInUser.Id;
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
