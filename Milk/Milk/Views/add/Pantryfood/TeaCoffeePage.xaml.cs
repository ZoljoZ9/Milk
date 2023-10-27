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
    public partial class TeaCoffeePage : ContentPage
    {
        private List<Produce> bakings;
        private List<Produce> selectedBakings; // To store selected fruits

        public int UserId { get; set; }


        public TeaCoffeePage()
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
                    new Produce { Name = "Black Tea", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Black Tea", loggedInUserId), ImagePath = "blacktea.png" },
                    new Produce { Name = "Green Tea", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Green Tea", loggedInUserId), ImagePath = "greentea.png" },
                    new Produce { Name = "Herbal Tea", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Herbal Tea", loggedInUserId), ImagePath = "herbaltea.png" },
                    new Produce { Name = "Chai Tea", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Chai Tea", loggedInUserId), ImagePath = "chaitea.png" },
                    new Produce { Name = "Earl Grey Tea", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Earl Grey Tea", loggedInUserId), ImagePath = "earlgreytea.png" },
                    new Produce { Name = "Oolong Tea", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Oolong Tea", loggedInUserId), ImagePath = "oolongtea.png" },
                    new Produce { Name = "White Tea", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("White Tea", loggedInUserId), ImagePath = "whitetea.png" },
                    new Produce { Name = "Decaf Coffee", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Decaf Coffee", loggedInUserId), ImagePath = "decafcoffee.png" },
                    new Produce { Name = "Instant Coffee", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Instant Coffee", loggedInUserId), ImagePath = "instantcoffee.png" },
                    new Produce { Name = "Ground Coffee", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Ground Coffee", loggedInUserId), ImagePath = "groundcoffee.png" },
                    new Produce { Name = "Espresso Beans", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Espresso Beans", loggedInUserId), ImagePath = "espressobeans.png" },
                    new Produce { Name = "Coffee Pods", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Coffee Pods", loggedInUserId), ImagePath = "coffeepods.png" },
                    // Add more tea and coffee products here as needed









                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeBakings();
            originalItems = new ObservableCollection<Produce>(bakings);
            fruitListView.ItemsSource = originalItems;
            Title = "Tea & Coffee";

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