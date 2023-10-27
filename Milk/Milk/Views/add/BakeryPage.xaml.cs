using Milk.Data;
using Milk.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Produce;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BakeryPage : ContentPage
    {
        private List<Produce> bakeries;
        private List<Produce> selectedBakeries; // To store selected
                                                // s

        public int UserId { get; set; }

        public BakeryPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedBakeries = new List<Produce>();
        }




        private async Task InitializeBakeries()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            bakeries = new List<Produce>
                {
                    new Produce { Name = "Bagels", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Bagels", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Baguette", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Baguette", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Bread Rolls", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Bread Rolls", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Brown Bread", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Brown Bread", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Croissants", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Croissants", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Danish Pastries", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Danish Pastries", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Doughnuts", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Doughnuts", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Flatbread", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Flatbread", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Focaccia", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Focaccia", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Muffins", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Muffins", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Pies", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Pies", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Pretzels", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Pretzels", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Rye Bread", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Rye Bread", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Scones", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Scones", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Sourdough Bread", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Sourdough Bread", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Sweet Buns", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Sweet Buns", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Tarts", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Tarts", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "White Bread", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("White Bread", loggedInUserId), ImagePath = "bread.png" },
                    new Produce { Name = "Whole Wheat Bread", Type = ProduceType.Bakery, Quantity = await dbContext.GetBakeriesQuantityByNameAndUserId("Whole Wheat Bread", loggedInUserId), ImagePath = "bread.png" }

                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeBakeries();
            originalItems = new ObservableCollection<Produce>(bakeries);
            fruitListView.ItemsSource = originalItems;
            Title = "Bakery";
        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var bakery = (Produce)button.BindingContext;

                bakery.Quantity++;
                if (!selectedBakeries.Contains(bakery))
                {
                    selectedBakeries.Add(bakery);
                }

                await UpdateDatabase(bakery, 1);
            }
            catch (Exception ex)
            {
                // Log the error or display a message
                Console.WriteLine(ex.Message);
            }
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var bakery = (Produce)button.BindingContext;

            bakery.Quantity--;

            if (bakery.Quantity < 0)
                bakery.Quantity = 0;

            await UpdateDatabase(bakery, -1);
        }

        private async Task UpdateDatabase(Produce bakery, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingBakery = await dbContext.GetBakeriesByNameAndUserId(bakery.Name, loggedInUserId);
                    if (existingBakery != null)
                    {
                        existingBakery.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingBakery.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingBakery);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingBakery);
                        }
                    }
                    else
                    {
                        bakery.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(bakery);
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
            var bakery = (Produce)button.BindingContext;

            if (bakery.Quantity > 0)
            {
                bakery.Quantity = 0;
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = bakeries;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedBakeries.Contains(bakery))
                {
                    selectedBakeries.Remove(bakery);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingBakery = await dbContext.GetBakeriesByNameAndUserId(bakery.Name, App.LoggedInUserId);
                if (existingBakery != null)
                {
                    existingBakery.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingBakery);
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