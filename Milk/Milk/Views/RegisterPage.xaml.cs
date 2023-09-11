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
            var user = new GroceryUsers
            {
                username = usr.Text,
                email = eml.Text,
                password = psw.Text
            };

            await App.DbContext.InsertUser(user);
            App.LoggedInUser = user; // Setting the App's LoggedInUser property
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Login());
        }
    }
}
