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
    public partial class EggsButterMagerinePage : ContentPage
    {
        private List<Produce> cheeses;
        private List<Produce> selectedCheeses; // To store selected fruits

        public int UserId { get; set; }


        public EggsButterMagerinePage()
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
                    new Produce { Name = "Free-range large eggs", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Free-range large eggs", loggedInUserId), ImagePath = "free_range_large_eggs.png" },
                    new Produce { Name = "Organic brown eggs", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Organic brown eggs", loggedInUserId), ImagePath = "organic_brown_eggs.png" },
                    new Produce { Name = "Caged eggs", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Caged eggs", loggedInUserId), ImagePath = "caged_eggs.png" },
                    new Produce { Name = "Omega-3 enriched eggs", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Omega-3 enriched eggs", loggedInUserId), ImagePath = "omega3_enriched_eggs.png" },
                    new Produce { Name = "Salted butter", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Salted butter", loggedInUserId), ImagePath = "salted_butter.png" },
                    new Produce { Name = "Unsalted butter", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Unsalted butter", loggedInUserId), ImagePath = "unsalted_butter.png" },
                    new Produce { Name = "Organic butter", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Organic butter", loggedInUserId), ImagePath = "organic_butter.png" },
                    new Produce { Name = "Light butter", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Light butter", loggedInUserId), ImagePath = "light_butter.png" },
                    new Produce { Name = "Regular margarine", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Regular margarine", loggedInUserId), ImagePath = "regular_margarine.png" },
                    new Produce { Name = "Low-fat margarine", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Low-fat margarine", loggedInUserId), ImagePath = "low_fat_margarine.png" },
                    new Produce { Name = "Olive oil margarine", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Olive oil margarine", loggedInUserId), ImagePath = "olive_oil_margarine.png" },
                    new Produce { Name = "Canola oil margarine", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Canola oil margarine", loggedInUserId), ImagePath = "canola_oil_margarine.png" }

                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeCheeses();
            originalItems = new ObservableCollection<Produce>(cheeses);
            CheeseListView.ItemsSource = originalItems;
            Title = "Eggs, Butter, & Magerine";

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