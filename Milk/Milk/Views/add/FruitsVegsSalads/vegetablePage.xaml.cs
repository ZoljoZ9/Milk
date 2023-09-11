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
    public partial class vegetablePage : ContentPage
    {
        private List<Produce> vegetables;
        private List<Produce> selectedVegetables; // To store selected fruits

        public int UserId { get; set; }

        public vegetablePage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedVegetables = new List<Produce>();
        }
        private async Task InitializeVegetables()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            vegetables = new List<Produce>
                {
                    new Produce { Name = "Asparagus", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Asparagus", loggedInUserId), ImagePath = "asparagus.png" },
                    new Produce  { Name = "Beets", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Beets", loggedInUserId), ImagePath = "beets.png" },
                    new Produce { Name = "Bell Peppers", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Bell Peppers", loggedInUserId), ImagePath = "bellpeppers.png" },
                    new Produce { Name = "Broccoli", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Broccoli", loggedInUserId), ImagePath = "broccoli.png" },
                    new Produce { Name = "Brussels Sprouts", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Brussels Sprouts", loggedInUserId), ImagePath = "brusselssprouts.png" },
                    new Produce { Name = "Cabbage", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Cabbage", loggedInUserId), ImagePath = "cabbage.png" },
                    new Produce { Name = "Carrots", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Carrots", loggedInUserId), ImagePath = "carrots.png" },
                    new Produce { Name = "Cauliflower", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Cauliflower", loggedInUserId), ImagePath = "cauliflower.png" },
                    new Produce { Name = "Celery", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Celery", loggedInUserId), ImagePath = "celery.png" },
                    new Produce { Name = "Cucumbers", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Cucumbers", loggedInUserId), ImagePath = "cucumber.png" },
                    new Produce { Name = "Eggplant", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Eggplant", loggedInUserId), ImagePath = "eggplant.png" },
                    new Produce { Name = "Garlic", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Garlic", loggedInUserId), ImagePath = "garlic.png" },
                    new Produce { Name = "Green Beans", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Green Beans", loggedInUserId), ImagePath = "greenbeans.png" },
                    new Produce { Name = "Kale", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Kale", loggedInUserId), ImagePath = "kale.png" },
                    new Produce { Name = "Lettuce", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Lettuce", loggedInUserId), ImagePath = "lettuce.png" },
                    new Produce { Name = "Mushrooms", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Mushrooms", loggedInUserId), ImagePath = "mushrooms.png" },
                    new Produce { Name = "Onions", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Onions", loggedInUserId), ImagePath = "onions.png" },
                    new Produce { Name = "Peas", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Peas", loggedInUserId), ImagePath = "peas.png" },
                    new Produce { Name = "Potatoes", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Potatoes", loggedInUserId), ImagePath = "potatoes.png" },
                    new Produce { Name = "Radishes", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Radishes", loggedInUserId), ImagePath = "radishes.png" },
                    new Produce { Name = "Spinach", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Spinach", loggedInUserId), ImagePath = "spinach.png" },
                    new Produce { Name = "Squash", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Squash", loggedInUserId), ImagePath = "squash.png" },
                    new Produce { Name = "Sweet Potatoes", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Sweet Potatoes", loggedInUserId), ImagePath = "sweetpotatoes.png" },
                    new Produce { Name = "Tomatoes", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Tomatoes", loggedInUserId), ImagePath = "tomatoes.png" },
                    new Produce { Name = "Zucchini", Type = ProduceType.Vegetable, Quantity = await dbContext.GetVegetableQuantityByNameAndUserId("Zucchini", loggedInUserId), ImagePath = "zucchini.png" }


                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeVegetables();
            originalItems = new ObservableCollection<Produce>(vegetables);
            vegetableListView.ItemsSource = originalItems;
        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var vegetable = (Produce)button.BindingContext;

                vegetable.Quantity++;
                if (!selectedVegetables.Contains(vegetable))
                {
                    selectedVegetables.Add(vegetable);
                }

                await UpdateDatabase(vegetable, 1);
            }
            catch (Exception ex)
            {
                // Log the error or display a message
                Console.WriteLine(ex.Message);
            }
        }




        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var vegetable = (Produce)button.BindingContext;

            await UpdateDatabase(vegetable, 1);
            vegetableListView.ItemsSource = null;  // Reset the ItemsSource
            vegetableListView.ItemsSource = vegetables;  // Set it back

            vegetable.Quantity--;
            if (vegetable.Quantity < 0)
                vegetable.Quantity = 0;

            await UpdateDatabase(vegetable, -1);
        }


        private async Task UpdateDatabase(Produce vegetable, int adjustment)
        {
            try
            {
                var dbContext = new AppDbContext(App.DatabasePath);
                var loggedInUserId = App.LoggedInUserId;

                var existingVegetable = await dbContext.GetVegetableByNameAndUserId(vegetable.Name, loggedInUserId);
                if (existingVegetable != null)
                {
                    // Adjust quantity for the existing fruit by either 1 or -1, depending on the "adjustment" parameter
                    existingVegetable.Quantity += adjustment;
                    await dbContext.Database.UpdateAsync(existingVegetable);
                }
                else
                {
                    // New fruit, so insert it
                    vegetable.UserId = loggedInUserId;
                    await dbContext.Database.InsertAsync(vegetable);
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
            var vegetable = (Produce)button.BindingContext;

            if (vegetable.Quantity > 0)
            {
                vegetable.Quantity = 0;
                vegetableListView.ItemsSource = null;  // Reset the ItemsSource
                vegetableListView.ItemsSource = vegetables;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedVegetables.Contains(vegetable))
                {
                    selectedVegetables.Remove(vegetable);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingVegetable = await dbContext.GetVegetableByNameAndUserId(vegetable.Name, App.LoggedInUserId);
                if (existingVegetable != null)
                {
                    existingVegetable.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingVegetable);
                }
            }
        }
        private ObservableCollection<Produce> originalItems;


        void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = searchBar.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                vegetableListView.ItemsSource = originalItems;
            }
            else
            {
                vegetableListView.ItemsSource = originalItems.Where(item => item.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
        }






    }

}