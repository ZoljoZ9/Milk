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
    public partial class LongLifeMilkPage : ContentPage
    {
        private List<Produce> bakings;
        private List<Produce> selectedBakings; // To store selected fruits

        public int UserId { get; set; }


        public LongLifeMilkPage()
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
                    new Produce { Name = "Long-Life Full Cream Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Full Cream Milk", loggedInUserId), ImagePath = "longlife_full_cream_milk.png" },
                    new Produce { Name = "Long-Life Low Fat Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Low Fat Milk", loggedInUserId), ImagePath = "longlife_low_fat_milk.png" },
                    new Produce { Name = "Long-Life Skim Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Skim Milk", loggedInUserId), ImagePath = "longlife_skim_milk.png" },
                    new Produce { Name = "Long-Life Lactose-Free Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Lactose-Free Milk", loggedInUserId), ImagePath = "longlife_lactose_free_milk.png" },
                    new Produce { Name = "Long-Life Almond Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Almond Milk", loggedInUserId), ImagePath = "longlife_almond_milk.png" },
                    new Produce { Name = "Long-Life Soy Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Oat Milk", loggedInUserId), ImagePath = "longlife_oat_milk.png" },
                    new Produce { Name = "Long-Life Rice Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Rice Milk", loggedInUserId), ImagePath = "longlife_rice_milk.png" },
                    new Produce { Name = "Long-Life Coconut Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Coconut Milk", loggedInUserId), ImagePath = "longlife_coconut_milk.png" },
                    new Produce { Name = "Long-Life Goat Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Goat Milk", loggedInUserId), ImagePath = "longlife_goat_milk.png" },
                    new Produce { Name = "Long-Life Sheep Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life Sheep Milk", loggedInUserId), ImagePath = "longlife_sheep_milk.png" },
                    new Produce { Name = "Long-Life A2 Milk", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Long-Life A2 Milk", loggedInUserId), ImagePath = "longlife_a2_milk.png" }
                    // Add more long-life milk products here as needed





                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeBakings();
            originalItems = new ObservableCollection<Produce>(bakings);
            fruitListView.ItemsSource = originalItems;
            Title = "Long Life Milk";

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