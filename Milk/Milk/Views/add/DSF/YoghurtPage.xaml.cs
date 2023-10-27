using Milk.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Produce;

namespace Milk.Views.add.DSF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YoghurtPage : ContentPage
    {
        private List<Produce> cheeses;
        private List<Produce> selectedCheeses; // To store selected fruits

        public int UserId { get; set; }


        public YoghurtPage()
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
                    new Produce { Name = "Plain Greek Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Plain Greek Yoghurt", loggedInUserId), ImagePath = "plain_greek_yoghurt.png" },
                    new Produce { Name = "Low-Fat Greek Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Low-Fat Greek Yoghurt", loggedInUserId), ImagePath = "low_fat_greek_yoghurt.png" },
                    new Produce { Name = "Full Cream Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Full Cream Yoghurt", loggedInUserId), ImagePath = "full_cream_yoghurt.png" },
                    new Produce { Name = "Strawberry Flavoured Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Strawberry Flavoured Yoghurt", loggedInUserId), ImagePath = "strawberry_yoghurt.png" },
                    new Produce { Name = "Vanilla Flavoured Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Vanilla Flavoured Yoghurt", loggedInUserId), ImagePath = "vanilla_yoghurt.png" },
                    new Produce { Name = "Blueberry Flavoured Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Blueberry Flavoured Yoghurt", loggedInUserId), ImagePath = "blueberry_yoghurt.png" },
                    new Produce { Name = "Mango Flavoured Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Mango Flavoured Yoghurt", loggedInUserId), ImagePath = "mango_yoghurt.png" },
                    new Produce { Name = "Kids' Yoghurt Pouches", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Kids' Yoghurt Pouches", loggedInUserId), ImagePath = "kids_yoghurt_pouches.png" },
                    new Produce { Name = "Organic Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Organic Yoghurt", loggedInUserId), ImagePath = "organic_yoghurt.png" },
                    new Produce { Name = "Probiotic Drinkable Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Probiotic Drinkable Yoghurt", loggedInUserId), ImagePath = "drinkable_yoghurt.png" },
                    new Produce { Name = "Lactose-Free Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Lactose-Free Yoghurt", loggedInUserId), ImagePath = "lactose_free_yoghurt.png" },
                    new Produce { Name = "Coconut Yoghurt", Type = ProduceType.Cheese, Quantity = await dbContext.GetCheesesQuantityByNameAndUserId("Coconut Yoghurt", loggedInUserId), ImagePath = "coconut_yoghurt.png" }


                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeCheeses();
            originalItems = new ObservableCollection<Produce>(cheeses);
            CheeseListView.ItemsSource = originalItems;
            Title = "Yoghurt";

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