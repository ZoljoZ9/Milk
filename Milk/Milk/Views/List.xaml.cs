using Milk.Data;
using Milk.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Milk.Views.add;
using Milk.ViewModels;
using static Milk.ViewModels.ListViewModel;

namespace Milk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class List : ContentPage
    {
        private ListViewModel ViewModel => BindingContext as ListViewModel;

        public List()
        {
            InitializeComponent();
            BindingContext = new ListViewModel(App.LoggedInUserId);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel == null)
            {
                System.Diagnostics.Debug.WriteLine("ViewModel is null");
                return;  // This will halt the function early if ViewModel is null.
            }

            await ViewModel.LoadSelectedItems(App.LoggedInUserId);
        }




        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<object>(this, "UpdateList");
        }

        private void DeleteProduceButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var produce = button.BindingContext as Produce;
                if (produce != null)
                {
                    ViewModel.DeleteProduce(produce);
                }
            }
        }
    }
}
