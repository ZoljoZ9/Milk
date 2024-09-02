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
using Microsoft.Maui.Controls.Xaml;
using static Produce;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SaladPage : ContentPage
    {
        private List<Produce> fruits;
        private List<Produce> selectedFruits; // To store selected fruits

        public int UserId { get; set; }


        public SaladPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedFruits = new List<Produce>();
        }


        private async Task InitializeFruits()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            fruits = new List<Produce>
            {       
                new Produce { Name = "Asian Style Salad", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Asian Style Salad", loggedInUserId), ImagePath = "asian.png" },
                new Produce { Name = "Caesar Salad", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Caesar Salad", loggedInUserId), ImagePath = "caesar.png" },
                new Produce { Name = "Chicken Salad", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Chicken Salad", loggedInUserId), ImagePath = "chicken.png" },
                new Produce { Name = "Coleslaw", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Coleslaw", loggedInUserId), ImagePath = "coleslaw.png" },
                new Produce { Name = "Garden Salad", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Garden Salad", loggedInUserId), ImagePath = "garden.png" },
                new Produce { Name = "Grain-based Salad", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Grain-based Salad", loggedInUserId), ImagePath = "grain.png" },
                new Produce { Name = "Greek Salad", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Greek Salad", loggedInUserId), ImagePath = "greek.png" },
                new Produce { Name = "Pasta Salad", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Pasta Salad", loggedInUserId), ImagePath = "pasta.png" },
                new Produce { Name = "Roast Vegetable Salad", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Roast Vegetable Salad", loggedInUserId), ImagePath = "roastveg.png" },
                new Produce { Name = "Seafood Salad", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Seafood Salad", loggedInUserId), ImagePath = "seafood.png" }
                // ... Continue for other salads if needed
            };


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeFruits();
            originalItems = new ObservableCollection<Produce>(fruits);
            saladListView.ItemsSource = originalItems;
            Title = "Salad";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var fruit = (Produce)button.BindingContext;

            fruit.Quantity++;
            if (!selectedFruits.Contains(fruit))
            {
                selectedFruits.Add(fruit);
            }


            // Update the database
            await UpdateDatabase(fruit, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var fruit = (Produce)button.BindingContext;

            fruit.Quantity--;

            if (fruit.Quantity < 0)
                fruit.Quantity = 0;

            await UpdateDatabase(fruit, -1);
        }

        private async Task UpdateDatabase(Produce fruit, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingFruit = await dbContext.GetFruitByNameAndUserId(fruit.Name, loggedInUserId);
                    if (existingFruit != null)
                    {
                        existingFruit.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingFruit.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingFruit);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingFruit);
                        }
                    }
                    else
                    {
                        fruit.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(fruit);
                    }
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
            var fruit = (Produce)button.BindingContext;

            if (fruit.Quantity > 0)
            {
                fruit.Quantity = 0;
                saladListView.ItemsSource = null;  // Reset the ItemsSource
                saladListView.ItemsSource = fruits;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedFruits.Contains(fruit))
                {
                    selectedFruits.Remove(fruit);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingFruit = await dbContext.GetFruitByNameAndUserId(fruit.Name, App.LoggedInUserId);
                if (existingFruit != null)
                {
                    existingFruit.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingFruit);
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