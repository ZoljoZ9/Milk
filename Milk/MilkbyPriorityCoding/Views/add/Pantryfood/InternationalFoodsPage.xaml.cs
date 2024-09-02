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
using Microsoft.Maui.Controls.Xaml;
using static Produce;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add.Pantryfood
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternationalFoodsPage : ContentPage
    {
        private List<Produce> bakings;
        private List<Produce> selectedBakings; // To store selected fruits

        public int UserId { get; set; }


        public InternationalFoodsPage()
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
                    new Produce { Name = "Italian Pasta", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Italian Pasta", loggedInUserId), ImagePath = "italianpasta.png" },
                    new Produce { Name = "Rice", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Rice", loggedInUserId), ImagePath = "rice.png" },
                    new Produce { Name = "Noodles", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Noodles", loggedInUserId), ImagePath = "noodles.png" },
                    new Produce { Name = "Couscous", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Couscous", loggedInUserId), ImagePath = "couscous.png" },
                    new Produce { Name = "Quinoa", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Quinoa", loggedInUserId), ImagePath = "quinoa.png" },
                    new Produce { Name = "Sushi Rice", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Sushi Rice", loggedInUserId), ImagePath = "sushirice.png" },
                    new Produce { Name = "Lentils", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Lentils", loggedInUserId), ImagePath = "lentils.png" },
                    new Produce { Name = "Couscous", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Couscous", loggedInUserId), ImagePath = "couscous.png" },
                    new Produce { Name = "Chickpeas", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Chickpeas", loggedInUserId), ImagePath = "chickpeas.png" },
                    new Produce { Name = "Coconut Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Coconut Milk", loggedInUserId), ImagePath = "coconutmilk.png" },
                    new Produce { Name = "Curry Paste", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Curry Paste", loggedInUserId), ImagePath = "currypaste.png" },
                    new Produce { Name = "Tortillas", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Tortillas", loggedInUserId), ImagePath = "tortillas.png" },
                    new Produce { Name = "Sushi Nori", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Sushi Nori", loggedInUserId), ImagePath = "sushinori.png" },
                    new Produce { Name = "Rice Vinegar", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Rice Vinegar", loggedInUserId), ImagePath = "ricevinegar.png" },
                    new Produce { Name = "Soy Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Soy Sauce", loggedInUserId), ImagePath = "soysauce.png" },
                    new Produce { Name = "Taco Seasoning", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Taco Seasoning", loggedInUserId), ImagePath = "tacoseasoning.png" },
                    new Produce { Name = "Tikka Masala Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Tikka Masala Sauce", loggedInUserId), ImagePath = "tikkamasalasauce.png" },
                    new Produce { Name = "Teriyaki Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Teriyaki Sauce", loggedInUserId), ImagePath = "teriyakisauce.png" },
                    new Produce { Name = "Miso Soup", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Miso Soup", loggedInUserId), ImagePath = "misosoup.png" },
                    new Produce { Name = "Tahini Paste", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Tahini Paste", loggedInUserId), ImagePath = "tahinipaste.png" },
                    new Produce { Name = "Sriracha Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Sriracha Sauce", loggedInUserId), ImagePath = "srirachasauce.png" },
                    new Produce { Name = "Salsa", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Salsa", loggedInUserId), ImagePath = "salsa.png" },
                    new Produce { Name = "Kimchi", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Kimchi", loggedInUserId), ImagePath = "kimchi.png" },
                    new Produce { Name = "Coconut Cream", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Coconut Cream", loggedInUserId), ImagePath = "coconutcream.png" },
                    new Produce { Name = "Garam Masala", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Garam Masala", loggedInUserId), ImagePath = "garammasala.png" },
                    new Produce { Name = "Fajita Mix", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Fajita Mix", loggedInUserId), ImagePath = "fajitamix.png" },
                    new Produce { Name = "Wasabi Paste", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Wasabi Paste", loggedInUserId), ImagePath = "wasabipaste.png" },
                    new Produce { Name = "Miso Paste", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Miso Paste", loggedInUserId), ImagePath = "misopaste.png" },
                    new Produce { Name = "Enchilada Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Enchilada Sauce", loggedInUserId), ImagePath = "enchiladasauce.png" }
                    // Add more international foods here as needed




                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeBakings();
            originalItems = new ObservableCollection<Produce>(bakings);
            fruitListView.ItemsSource = originalItems;
            Title = "International Foods";

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