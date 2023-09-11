using Milk.Data;
using Milk.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Produce;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SaladPage : ContentPage
    {
        private List<Produce> salads;
        private List<Produce> selectedSalads; // To store selected fruits

        public int UserId { get; set; }


        public SaladPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedSalads = new List<Produce>();
        }


        private async Task InitializeSalads()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            salads = new List<Produce>
            {       
                new Produce { Name = "Asian Style Salad", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Asian Style Salad", loggedInUserId), ImagePath = "asian.png" },
                new Produce { Name = "Caesar Salad", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Caesar Salad", loggedInUserId), ImagePath = "caesar.png" },
                new Produce { Name = "Chicken Salad", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Chicken Salad", loggedInUserId), ImagePath = "chicken.png" },
                new Produce { Name = "Coleslaw", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Coleslaw", loggedInUserId), ImagePath = "coleslaw.png" },
                new Produce { Name = "Garden Salad", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Garden Salad", loggedInUserId), ImagePath = "garden.png" },
                new Produce { Name = "Grain-based Salad", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Grain-based Salad", loggedInUserId), ImagePath = "grain.png" },
                new Produce { Name = "Greek Salad", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Greek Salad", loggedInUserId), ImagePath = "greek.png" },
                new Produce { Name = "Pasta Salad", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Pasta Salad", loggedInUserId), ImagePath = "pasta.png" },
                new Produce { Name = "Roast Vegetable Salad", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Roast Vegetable Salad", loggedInUserId), ImagePath = "roastveg.png" },
                new Produce { Name = "Seafood Salad", Type = ProduceType.Salad, Quantity = await dbContext.GetSaladQuantityByNameAndUserId("Seafood Salad", loggedInUserId), ImagePath = "seafood.png" }
                // ... Continue for other salads if needed
            };


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeSalads();
            originalItems = new ObservableCollection<Produce>(salads);
            saladListView.ItemsSource = originalItems;
        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var salad = (Produce)button.BindingContext;

            salad.Quantity++;
            if (!selectedSalads.Contains(salad))
            {
                selectedSalads.Add(salad);
            }


            // Update the database
            await UpdateDatabase(salad, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var salad = (Produce)button.BindingContext;

            salad.Quantity--;

            if (salad.Quantity < 0)
                salad.Quantity = 0;

            await UpdateDatabase(salad, -1);
        }

        private async Task UpdateDatabase(Produce salad, int adjustment)
        {
            try
            {
                var dbContext = new AppDbContext(App.DatabasePath);
                var loggedInUserId = App.LoggedInUserId;

                var existingSalad = await dbContext.GetFruitByNameAndUserId(salad.Name, loggedInUserId);
                if (existingSalad != null)
                {
                    // Adjust quantity for the existing fruit by either 1 or -1, depending on the "adjustment" parameter
                    existingSalad.Quantity += adjustment;
                    await dbContext.Database.UpdateAsync(existingSalad);
                    MessagingCenter.Send<object>(this, "UpdateList");

                }
                else
                {
                    // New fruit, so insert it
                    salad.UserId = loggedInUserId;
                    await dbContext.Database.InsertAsync(salad);
                    MessagingCenter.Send<object>(this, "UpdateList");

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save fruit. Error: {ex.Message}", "OK");
            }
        }
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var salad = (Produce)button.BindingContext;

            if (salad.Quantity > 0)
            {
                salad.Quantity = 0;
                saladListView.ItemsSource = null;  // Reset the ItemsSource
                saladListView.ItemsSource = salads;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedSalads.Contains(salad))
                {
                    selectedSalads.Remove(salad);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingSalad = await dbContext.GetFruitByNameAndUserId(salad.Name, App.LoggedInUserId);
                if (existingSalad != null)
                {
                    existingSalad.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingSalad);
                }
            }
        }
        private ObservableCollection<Produce> originalItems;


        void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = searchBar.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                saladListView.ItemsSource = originalItems;
            }
            else
            {
                saladListView.ItemsSource = originalItems.Where(item => item.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
        }






    }

}