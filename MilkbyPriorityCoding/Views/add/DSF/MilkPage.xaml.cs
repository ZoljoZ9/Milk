using Milk.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using static Produce;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add.DSF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MilkPage : ContentPage
    {
        private List<Produce> cheeses;
        private List<Produce> selectedCheeses; // To store selected fruits

        public int UserId { get; set; }


        public MilkPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedCheeses = new List<Produce>();
        }


        private async Task InitializeCheeses()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            cheeses = new List<Produce>
                {
                    new Produce { Name = "Full Cream Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Full Cream Milk", loggedInUserId), ImagePath = "full_cream_milk.png" },
                    new Produce { Name = "Skimmed Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Skimmed Milk", loggedInUserId), ImagePath = "skimmed_milk.png" },
                    new Produce { Name = "Semi-Skimmed Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Semi-Skimmed Milk", loggedInUserId), ImagePath = "semi_skimmed_milk.png" },
                    new Produce { Name = "Lactose-Free Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Lactose-Free Milk", loggedInUserId), ImagePath = "lactose_free_milk.png" },
                    new Produce { Name = "Almond Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Almond Milk", loggedInUserId), ImagePath = "almond_milk.png" },
                    new Produce { Name = "Soy Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Soy Milk", loggedInUserId), ImagePath = "soy_milk.png" },
                    new Produce { Name = "Oat Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Oat Milk", loggedInUserId), ImagePath = "oat_milk.png" },
                    new Produce { Name = "Rice Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Rice Milk", loggedInUserId), ImagePath = "rice_milk.png" },
                    new Produce { Name = "Coconut Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Coconut Milk", loggedInUserId), ImagePath = "coconut_milk.png" },
                    new Produce { Name = "Organic Full Cream Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Organic Full Cream Milk", loggedInUserId), ImagePath = "organic_full_cream_milk.png" },
                    new Produce { Name = "Goat Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Goat Milk", loggedInUserId), ImagePath = "goat_milk.png" },
                    new Produce { Name = "Long Life Milk", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Long Life Milk", loggedInUserId), ImagePath = "long_life_milk.png" }


                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeCheeses();
            originalItems = new ObservableCollection<Produce>(cheeses);
            CheeseListView.ItemsSource = originalItems;
            Title = "Milk";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cheese = (Produce)button.BindingContext;

            cheese.Quantity++;
            if (!selectedCheeses.Contains(cheese))
            {
                selectedCheeses.Add(cheese);
            }


            // Update the database
            await UpdateDatabase(cheese, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var cheese = (Produce)button.BindingContext;

            cheese.Quantity--;

            if (cheese.Quantity < 0)
                cheese.Quantity = 0;

            await UpdateDatabase(cheese, -1);
        }

        private async Task UpdateDatabase(Produce cheese, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingCheese = await dbContext.GetCheesesByNameAndUserId(cheese.Name, loggedInUserId);
                    if (existingCheese != null)
                    {
                        existingCheese.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingCheese.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingCheese);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingCheese);
                        }
                    }
                    else
                    {
                        cheese.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(cheese);
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
            var cheese = (Produce)button.BindingContext;

            if (cheese.Quantity > 0)
            {
                cheese.Quantity = 0;
                CheeseListView.ItemsSource = null;  // Reset the ItemsSource
                CheeseListView.ItemsSource = cheeses;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedCheeses.Contains(cheese))
                {
                    selectedCheeses.Remove(cheese);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingCheese = await dbContext.GetRedMeatByNameAndUserId(cheese.Name, App.LoggedInUserId);
                if (existingCheese != null)
                {
                    existingCheese.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingCheese);
                }
            }
        }
        private ObservableCollection<Produce> originalItems;


        void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = searchBar.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                CheeseListView.ItemsSource = originalItems;
            }
            else
            {
                CheeseListView.ItemsSource = originalItems.Where(item => item.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
        }
    }
}