using Milk.Data;
using Milk.Models;
using Milk.Views.add.Pantryfood;
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

namespace Milk.Views.add.Pantryfood
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CannedFoodInstandMealsPage : ContentPage
    {
        private List<Produce> bakings;
        private List<Produce> selectedBakings; // To store selected fruits

        public int UserId { get; set; }


        public CannedFoodInstandMealsPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedBakings = new List<Produce>();
        }


        private async Task InitializeBakings()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            bakings = new List<Produce>
                {
                    new Produce { Name = "Canned Vegetables", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Vegetables", loggedInUserId), ImagePath = "cannedvegetables.png" },
                    new Produce { Name = "Canned Fruits", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Fruits", loggedInUserId), ImagePath = "cannedfruits.png" },
                    new Produce { Name = "Canned Beans", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Beans", loggedInUserId), ImagePath = "cannedbeans.png" },
                    new Produce { Name = "Canned Soups", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Soups", loggedInUserId), ImagePath = "cannedsoups.png" },
                    new Produce { Name = "Canned Tuna and Fish", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Tuna and Fish", loggedInUserId), ImagePath = "cannedtuna.png" },
                    new Produce { Name = "Canned Pasta and Sauces", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Pasta and Sauces", loggedInUserId), ImagePath = "cannedpasta.png" },
                    new Produce { Name = "Canned Soup", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Soup", loggedInUserId), ImagePath = "cannedsoup.png" },
                    new Produce { Name = "Canned Tomatoes and Tomato Paste", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Tomatoes and Tomato Paste", loggedInUserId), ImagePath = "cannedtomatoes.png" },
                    new Produce { Name = "Canned Meat", Type = ProduceType.Baking   , Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Meat", loggedInUserId), ImagePath = "cannedmeat.png" },
                    new Produce { Name = "Canned or Bottled Salsas", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned or Bottled Salsas", loggedInUserId), ImagePath = "cannedsalsas.png" },
                    new Produce { Name = "Instant Noodles", Type = ProduceType.Baking   , Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Instant Noodles", loggedInUserId), ImagePath = "instantnoodles.png" },
                    new Produce { Name = "Microwaveable Meals", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Microwaveable Meals", loggedInUserId), ImagePath = "microwaveablemeals.png" },
                    new Produce { Name = "Canned or Instant Soup", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned or Instant Soup", loggedInUserId), ImagePath = "cannedorsoup.png" },
                    new Produce { Name = "Instant Rice", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Instant Rice", loggedInUserId), ImagePath = "instantrice.png" },
                    new Produce { Name = "Instant Pasta", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Instant Pasta", loggedInUserId), ImagePath = "instantpasta.png" },
                    new Produce { Name = "Instant Oatmeal", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Instant Oatmeal", loggedInUserId), ImagePath = "instantoatmeal.png" },
                    new Produce { Name = "Instant Mashed Potatoes", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Instant Mashed Potatoes", loggedInUserId), ImagePath = "instantmashedpotatoes.png" },
                    new Produce { Name = "Canned Chili", Type = ProduceType.Baking  , Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Chili", loggedInUserId), ImagePath = "cannedchili.png" },
                    new Produce { Name = "Canned Spaghetti", Type = ProduceType.Baking  , Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Spaghetti", loggedInUserId), ImagePath = "cannedspaghetti.png" },
                    new Produce { Name = "Instant Mac and Cheese", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Instant Mac and Cheese", loggedInUserId), ImagePath = "instantmacandcheese.png" },
                    new Produce { Name = "Canned Vegetarian or Vegan Meals", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Vegetarian or Vegan Meals", loggedInUserId), ImagePath = "cannedvegetarian.png" },
                    new Produce { Name = "Instant Breakfast Drinks", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Instant Breakfast Drinks", loggedInUserId), ImagePath = "instantbreakfastdrinks.png" },
                    new Produce { Name = "Canned or Jarred Stews", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned or Jarred Stews", loggedInUserId), ImagePath = "cannedstews.png" },
                    new Produce { Name = "Instant Soup Mixes", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Instant Soup Mixes", loggedInUserId), ImagePath = "instantsoupmixes.png" },
                    new Produce { Name = "Canned Gravy", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned Gravy", loggedInUserId), ImagePath = "cannedgravy.png" },
                    new Produce { Name = "Canned or Jarred Pasta Sauces", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned or Jarred Pasta Sauces", loggedInUserId), ImagePath = "cannedpastasauces.png" },
                    new Produce { Name = "Canned or Instant Grains", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Canned or Instant Grains", loggedInUserId), ImagePath = "cannedgrains.png" }


                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeBakings();
            originalItems = new ObservableCollection<Produce>(bakings);
            fruitListView.ItemsSource = originalItems;
            Title = "Canned Food & Instant Meals";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var baking = (Produce)button.BindingContext;

            baking.Quantity++;
            if (!selectedBakings.Contains(baking))
            {
                selectedBakings.Add(baking);
            }


            // Update the database
            await UpdateDatabase(baking, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var baking = (Produce)button.BindingContext;

            baking.Quantity--;

            if (baking.Quantity < 0)
                baking.Quantity = 0;

            await UpdateDatabase(baking, -1);
        }

        private async Task UpdateDatabase(Produce baking, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingBaking = await dbContext.GetBakingSupplyByNameAndUserId(baking.Name, loggedInUserId);
                    if (existingBaking != null)
                    {
                        existingBaking.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingBaking.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingBaking);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingBaking);
                        }
                    }
                    else
                    {
                        baking.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(baking);
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
            var baking = (Produce)button.BindingContext;

            if (baking.Quantity > 0)
            {
                baking.Quantity = 0;
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = bakings;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedBakings.Contains(baking))
                {
                    selectedBakings.Remove(baking);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingBaking = await dbContext.GetBakingSupplyByNameAndUserId(baking.Name, App.LoggedInUserId);
                if (existingBaking != null)
                {
                    existingBaking.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingBaking);
                }
            }
        }
        private ObservableCollection<Produce> originalItems;


        void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = searchBar.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                fruitListView.ItemsSource = originalItems;
            }
            else
            {
                fruitListView.ItemsSource = originalItems.Where(item => item.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
        }






    }

}