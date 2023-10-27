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
    public partial class BreakfastSpreadPage : ContentPage
    {
        private List<Produce> bakings;
        private List<Produce> selectedBakings; // To store selected fruits

        public int UserId { get; set; }


        public BreakfastSpreadPage()
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
                    new Produce { Name = "Cereal", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Cereal", loggedInUserId), ImagePath = "cereal.png" },
                    new Produce { Name = "Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Milk", loggedInUserId), ImagePath = "milk.png" },
                    new Produce { Name = "Yogurt", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Yogurt", loggedInUserId), ImagePath = "yogurt.png" },
                    new Produce { Name = "Bread", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Bread", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Eggs", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Eggs", loggedInUserId), ImagePath = "eggs.png" },
                    new Produce { Name = "Oatmeal", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Oatmeal", loggedInUserId), ImagePath = "oatmeal.png" },
                    new Produce { Name = "Pancake and Waffle Mix", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Pancake and Waffle Mix", loggedInUserId), ImagePath = "pancakewaffle.png" },
                    new Produce { Name = "Pancake Syrup", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Pancake Syrup", loggedInUserId), ImagePath = "pancakesyrup.png" },
                    new Produce { Name = "Peanut Butter", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Peanut Butter", loggedInUserId), ImagePath = "peanutbutter.png" },
                    new Produce { Name = "Jam and Jelly", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Jam and Jelly", loggedInUserId), ImagePath = "jamjelly.png" },
                    new Produce { Name = "Honey", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Honey", loggedInUserId), ImagePath = "honey.png" },
                    new Produce { Name = "Fresh Fruit", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Fresh Fruit", loggedInUserId), ImagePath = "freshfruit.png" },
                    new Produce { Name = "Frozen Breakfast Items", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Frozen Breakfast Items", loggedInUserId), ImagePath = "frozenbreakfast.png" },
                    new Produce { Name = "Nutella", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Nutella", loggedInUserId), ImagePath = "nutella.png" },
                    new Produce { Name = "Muesli Bars and Granola Bars", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Muesli Bars and Granola Bars", loggedInUserId), ImagePath = "mueslibars.png" },
                    new Produce { Name = "Butter", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Butter", loggedInUserId), ImagePath = "butter.png" },
                    new Produce { Name = "Cream Cheese", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Cream Cheese", loggedInUserId), ImagePath = "creamcheese.png" },
                    new Produce { Name = "Margarine", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Margarine", loggedInUserId), ImagePath = "margarine.png" },
                    new Produce { Name = "Marmalade", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Marmalade", loggedInUserId), ImagePath = "marmalade.png" },
                    new Produce { Name = "Peanut Butter", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Peanut Butter", loggedInUserId), ImagePath = "peanutbutter2.png" },
                    new Produce { Name = "Nut Butters", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Nut Butters", loggedInUserId), ImagePath = "nutbutter.png" },
                    new Produce { Name = "Vegemite", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Vegemite", loggedInUserId), ImagePath = "vegemite.png" },
                    new Produce { Name = "Honey", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Honey", loggedInUserId), ImagePath = "honey2.png" },
                    new Produce { Name = "Jam and Jelly", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Jam and Jelly", loggedInUserId), ImagePath = "jamjelly2.png" },
                    new Produce { Name = "Syrup", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Syrup", loggedInUserId), ImagePath = "syrup.png" },
                    new Produce { Name = "Vegan Spreads", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Vegan Spreads", loggedInUserId), ImagePath = "veganspreads.png" },
                    new Produce { Name = "Vegan Butter", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Vegan Butter", loggedInUserId), ImagePath = "veganbutter.png" },
                    new Produce { Name = "Coconut Spread", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Coconut Spread", loggedInUserId), ImagePath = "coconutspread.png" },
                    new Produce { Name = "Tahini", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Tahini", loggedInUserId), ImagePath = "tahini.png" },
                    new Produce { Name = "Hazelnut Spreads", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Hazelnut Spreads", loggedInUserId), ImagePath = "hazelnutspreads.png" },

                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeBakings();
            originalItems = new ObservableCollection<Produce>(bakings);
            fruitListView.ItemsSource = originalItems;
            Title = "Breakfast & Spread";

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