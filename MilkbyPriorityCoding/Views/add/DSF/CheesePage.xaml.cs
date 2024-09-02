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
    public partial class CheesePage : ContentPage
    {
        private List<Produce> cheeses;
        private List<Produce> selectedCheeses; // To store selected fruits

        public int UserId { get; set; }


        public CheesePage()
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
                    new Produce { Name = "Brie", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Brie", loggedInUserId), ImagePath = "brie.png" },
                    new Produce { Name = "Camembert", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Camembert", loggedInUserId), ImagePath = "camembert.png" },
                    new Produce { Name = "Cheddar", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Cheddar", loggedInUserId), ImagePath = "cheddar.png" },
                    new Produce { Name = "Cottage Cheese", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Cottage Cheese", loggedInUserId), ImagePath = "cottagecheese.png" },
                    new Produce { Name = "Feta", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Feta", loggedInUserId), ImagePath = "feta.png" },
                    new Produce { Name = "Gouda", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Gouda", loggedInUserId), ImagePath = "gouda.png" },
                    new Produce { Name = "Havarti", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Havarti", loggedInUserId), ImagePath = "havarti.png" },
                    new Produce { Name = "Mozzarella", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Mozzarella", loggedInUserId), ImagePath = "mozzarella.png" },
                    new Produce { Name = "Parmesan", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Parmesan", loggedInUserId), ImagePath = "parmesan.png" },
                    new Produce { Name = "Pepper Jack", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Pepper Jack", loggedInUserId), ImagePath = "pepperjack.png" },
                    new Produce { Name = "Provolone", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Provolone", loggedInUserId), ImagePath = "provolone.png" },
                    new Produce { Name = "Ricotta", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Ricotta", loggedInUserId), ImagePath = "ricotta.png" },
                    new Produce { Name = "Swiss", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Swiss", loggedInUserId), ImagePath = "swiss.png" }

                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeCheeses();
            originalItems = new ObservableCollection<Produce>(cheeses);
            CheeseListView.ItemsSource = originalItems;
            Title = "Cheese";

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