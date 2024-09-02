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
    public partial class CreamCustardDessertsPage : ContentPage
    {
        private List<Produce> cheeses;
        private List<Produce> selectedCheeses; // To store selected fruits

        public int UserId { get; set; }


        public CreamCustardDessertsPage()
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
                    new Produce { Name = "Fresh double cream", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Fresh double cream", loggedInUserId), ImagePath = "fresh_double_cream.png" },
                    new Produce { Name = "Whipping cream", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Whipping cream", loggedInUserId), ImagePath = "whipping_cream.png" },
                    new Produce { Name = "Thickened cream", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Thickened cream", loggedInUserId), ImagePath = "thickened_cream.png" },
                    new Produce { Name = "Light cream", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Light cream", loggedInUserId), ImagePath = "light_cream.png" },
                    new Produce { Name = "Sour cream", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Sour cream", loggedInUserId), ImagePath = "sour_cream.png" },
                    new Produce { Name = "Clotted cream", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Clotted cream", loggedInUserId), ImagePath = "clotted_cream.png" },
                    new Produce { Name = "UHT/long-life cream", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("UHT/long-life cream", loggedInUserId), ImagePath = "uht_long_life_cream.png" },
                    new Produce { Name = "Ready-made custard", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Ready-made custard", loggedInUserId), ImagePath = "ready_made_custard.png" },
                    new Produce { Name = "Custard powder", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Custard powder", loggedInUserId), ImagePath = "custard_powder.png" },
                    new Produce { Name = "Creme Anglaise", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Creme Anglaise", loggedInUserId), ImagePath = "creme_anglaise.png" },
                    new Produce { Name = "Egg custard tarts", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Egg custard tarts", loggedInUserId), ImagePath = "egg_custard_tarts.png" },
                    new Produce { Name = "Frozen custard-based desserts", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Frozen custard-based desserts", loggedInUserId), ImagePath = "frozen_custard_based_desserts.png" },
                    new Produce { Name = "Panna cotta", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Panna cotta", loggedInUserId), ImagePath = "panna_cotta.png" },
                    new Produce { Name = "Tiramisu", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Tiramisu", loggedInUserId), ImagePath = "tiramisu.png" },
                    new Produce { Name = "Mousse", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Mousse", loggedInUserId), ImagePath = "mousse.png" },
                    new Produce { Name = "Trifle", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Trifle", loggedInUserId), ImagePath = "trifle.png" },
                    new Produce { Name = "Cheesecake", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Cheesecake", loggedInUserId), ImagePath = "cheesecake.png" },
                    new Produce { Name = "Gelato", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Gelato", loggedInUserId), ImagePath = "gelato.png" },
                    new Produce { Name = "Ice creams", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Ice creams", loggedInUserId), ImagePath = "ice_creams.png" },
                    new Produce { Name = "Chocolate lava cake", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Chocolate lava cake", loggedInUserId), ImagePath = "chocolate_lava_cake.png" },
                    new Produce { Name = "Fruit pies", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Fruit pies", loggedInUserId), ImagePath = "fruit_pies.png" },
                    new Produce { Name = "Lamingtons", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Lamingtons", loggedInUserId), ImagePath = "lamingtons.png" },
                    new Produce { Name = "Pavlova", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Pavlova", loggedInUserId), ImagePath = "pavlova.png" }
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeCheeses();
            originalItems = new ObservableCollection<Produce>(cheeses);
            CheeseListView.ItemsSource = originalItems;
            Title = "Cream, Custard, & Desserts";

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