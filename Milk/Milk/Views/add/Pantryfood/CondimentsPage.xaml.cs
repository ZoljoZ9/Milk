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
    public partial class CondimentsPage : ContentPage
    {
        private List<Produce> bakings;
        private List<Produce> selectedBakings; // To store selected fruits

        public int UserId { get; set; }


        public CondimentsPage()
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
                    new Produce { Name = "Vinegar", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Vinegar", loggedInUserId), ImagePath = "vinegar.png" },
                    new Produce { Name = "Salsa", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Salsa", loggedInUserId), ImagePath = "salsa.png" },
                    new Produce { Name = "Pickle", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Pickle", loggedInUserId), ImagePath = "pickle.png" },
                    new Produce { Name = "Horseradish", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Horseradish", loggedInUserId), ImagePath = "horseradish.png" },
                    new Produce { Name = "Tahini Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Tahini Sauce", loggedInUserId), ImagePath = "tahinisauce.png" },
                    new Produce { Name = "Sriracha Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Sriracha Sauce", loggedInUserId), ImagePath = "srirachasauce.png" },
                    new Produce { Name = "Pesto Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Pesto Sauce", loggedInUserId), ImagePath = "pestosauce.png" },
                    new Produce { Name = "Soybean Paste", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Soybean Paste", loggedInUserId), ImagePath = "soybeanpaste.png" },
                    new Produce { Name = "Fish Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Fish Sauce", loggedInUserId), ImagePath = "fishsauce.png" },
                    new Produce { Name = "Oyster Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Oyster Sauce", loggedInUserId), ImagePath = "oystersauce.png" },
                    new Produce { Name = "Tabasco Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Tabasco Sauce", loggedInUserId), ImagePath = "tabascosauce.png" },
                    new Produce { Name = "Sambal Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Sambal Sauce", loggedInUserId), ImagePath = "sambalsauce.png" },
                    new Produce { Name = "Miso Paste", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Miso Paste", loggedInUserId), ImagePath = "misopaste.png" },
                    new Produce { Name = "Sweet Chili Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Sweet Chili Sauce", loggedInUserId), ImagePath = "sweetchilisauce.png" },
                    new Produce { Name = "Hoisin Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Hoisin Sauce", loggedInUserId), ImagePath = "hoisinsauce.png" },
                    new Produce { Name = "Chutney", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Chutney", loggedInUserId), ImagePath = "chutney.png" },
                    new Produce { Name = "BBQ Rubs", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("BBQ Rubs", loggedInUserId), ImagePath = "bbqrubs.png" },
                    new Produce { Name = "Gravy Granules", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Gravy Granules", loggedInUserId), ImagePath = "gravygranules.png" },
                    new Produce { Name = "Soy Ginger Dressing", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Soy Ginger Dressing", loggedInUserId), ImagePath = "soygingerdressing.png" },
                    new Produce { Name = "Szechuan Sauce", Type = ProduceType.Baking, Quantity = await dbContext.GetBakingSupplyQuantityByNameAndUserId("Szechuan Sauce", loggedInUserId), ImagePath = "szechuansauce.png" }

                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeBakings();
            originalItems = new ObservableCollection<Produce>(bakings);
            fruitListView.ItemsSource = originalItems;
            Title = "Condiments";

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