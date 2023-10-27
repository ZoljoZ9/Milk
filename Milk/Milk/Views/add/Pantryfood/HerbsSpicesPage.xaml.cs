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
    public partial class HerbsSpicesPage : ContentPage
    {
        private List<Produce> bakings;
        private List<Produce> selectedBakings; // To store selected fruits

        public int UserId { get; set; }


        public HerbsSpicesPage()
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
                    new Produce { Name = "Basil", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Basil", loggedInUserId), ImagePath = "basil.png" },
                    new Produce { Name = "Oregano", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Oregano", loggedInUserId), ImagePath = "oregano.png" },
                    new Produce { Name = "Thyme", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Thyme", loggedInUserId), ImagePath = "thyme.png" },
                    new Produce { Name = "Rosemary", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Rosemary", loggedInUserId), ImagePath = "rosemary.png" },
                    new Produce { Name = "Cilantro", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Cilantro", loggedInUserId), ImagePath = "cilantro.png" },
                    new Produce { Name = "Parsley", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Parsley", loggedInUserId), ImagePath = "parsley.png" },
                    new Produce { Name = "Dill", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Dill", loggedInUserId), ImagePath = "dill.png" },
                    new Produce { Name = "Chives", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Chives", loggedInUserId), ImagePath = "chives.png" },
                    new Produce { Name = "Sage", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Sage", loggedInUserId), ImagePath = "sage.png" },
                    new Produce { Name = "Mint", Type = ProduceType.Baking  , Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Mint", loggedInUserId), ImagePath = "mint.png" },
                    new Produce { Name = "Paprika", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Paprika", loggedInUserId), ImagePath = "paprika.png" },
                    new Produce { Name = "Cumin", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Cumin", loggedInUserId), ImagePath = "cumin.png" },
                    new Produce { Name = "Coriander", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Coriander", loggedInUserId), ImagePath = "coriander.png" },
                    new Produce { Name = "Turmeric", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Turmeric", loggedInUserId), ImagePath = "turmeric.png" },
                    new Produce { Name = "Ginger", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Ginger", loggedInUserId), ImagePath = "ginger.png" },
                    new Produce { Name = "Garlic Powder", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Garlic Powder", loggedInUserId), ImagePath = "garlicpowder.png" },
                    new Produce { Name = "Onion Powder", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Onion Powder", loggedInUserId), ImagePath = "onionpowder.png" },
                    new Produce { Name = "Red Pepper Flakes", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Red Pepper Flakes", loggedInUserId), ImagePath = "redpepperflakes.png" },
                    new Produce { Name = "Black Pepper", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Black Pepper", loggedInUserId), ImagePath = "blackpepper.png" },
                    new Produce { Name = "Salt", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Salt", loggedInUserId), ImagePath = "salt.png" }
                    // Add more herbs and spices here as needed



                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeBakings();
            originalItems = new ObservableCollection<Produce>(bakings);
            fruitListView.ItemsSource = originalItems;
            Title = "Herbs & Spices";

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