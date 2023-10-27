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
    public partial class CookingSaucesPage : ContentPage
    {
        private List<Produce> bakings;
        private List<Produce> selectedBakings; // To store selected fruits

        public int UserId { get; set; }


        public CookingSaucesPage()
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
                        new Produce { Name = "Ketchup", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Ketchup", loggedInUserId), ImagePath = "ketchup.png" },
                        new Produce { Name = "Mustard", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Mustard", loggedInUserId), ImagePath = "mustard.png" },
                        new Produce { Name = "Mayonnaise", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Mayonnaise", loggedInUserId), ImagePath = "mayonnaise.png" },
                        new Produce { Name = "Relish", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Relish", loggedInUserId), ImagePath = "relish.png" },
                        new Produce { Name = "Hot Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Hot Sauce", loggedInUserId), ImagePath = "hotsauce.png" },
                        new Produce { Name = "Soy Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Soy Sauce", loggedInUserId), ImagePath = "soysauce.png" },
                        new Produce { Name = "Barbecue Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Barbecue Sauce", loggedInUserId), ImagePath = "barbecuesauce.png" },
                        new Produce { Name = "Worcestershire Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Worcestershire Sauce", loggedInUserId), ImagePath = "worcestershiresauce.png" },
                        new Produce { Name = "Teriyaki Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Teriyaki Sauce", loggedInUserId), ImagePath = "teriyakisauce.png" },
                        new Produce { Name = "Sesame Oil", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Sesame Oil", loggedInUserId), ImagePath = "sesameoil.png" },
                        new Produce { Name = "Marinara", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Marinara", loggedInUserId), ImagePath = "marinara.png" },
                        new Produce { Name = "Bolognese", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Bolognese", loggedInUserId), ImagePath = "bolognese.png" },
                        new Produce { Name = "Alfredo", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Alfredo", loggedInUserId), ImagePath = "alfredo.png" },
                        new Produce { Name = "Pesto", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Pesto", loggedInUserId), ImagePath = "pesto.png" },
                        new Produce { Name = "Carbonara", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Carbonara", loggedInUserId), ImagePath = "carbonara.png" },
                        new Produce { Name = "Arrabbiata", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Arrabbiata", loggedInUserId), ImagePath = "arrabbiata.png" },
                        new Produce { Name = "Puttanesca", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Puttanesca", loggedInUserId), ImagePath = "puttanesca.png" },
                        new Produce { Name = "Vodka Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Vodka Sauce", loggedInUserId), ImagePath = "vodkasauce.png" },
                        new Produce { Name = "Primavera", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Primavera", loggedInUserId), ImagePath = "primavera.png" },
                        new Produce { Name = "Aglio e Olio", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Aglio e Olio", loggedInUserId), ImagePath = "aglioeolio.png" }
                        
                        // Add more sauces here as needed


                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeBakings();
            originalItems = new ObservableCollection<Produce>(bakings);
            fruitListView.ItemsSource = originalItems;
            Title = "Cooking Sauces";

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