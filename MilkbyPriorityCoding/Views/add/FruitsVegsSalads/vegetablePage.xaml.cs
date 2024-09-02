using Milk.Data;
using Milk.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using static Produce;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class vegetablePage : ContentPage
    {
        private List<Produce> fruits;
        private List<Produce> selectedFruits; // To store selected fruits

        public int UserId { get; set; }

        public vegetablePage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedFruits = new List<Produce>();
        }
        private async Task InitializeFruits()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            fruits = new List<Produce>
                {
                    new Produce { Name = "Asparagus", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Asparagus", loggedInUserId), ImagePath = "asparagus.png" },
                    new Produce  { Name = "Beets", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Beets", loggedInUserId), ImagePath = "beets.png" },
                    new Produce { Name = "Bell Peppers", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Bell Peppers", loggedInUserId), ImagePath = "bellpeppers.png" },
                    new Produce { Name = "Broccoli", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Broccoli", loggedInUserId), ImagePath = "broccoli.png" },
                    new Produce { Name = "Brussels Sprouts", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Brussels Sprouts", loggedInUserId), ImagePath = "brusselssprouts.png" },
                    new Produce { Name = "Cabbage", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Cabbage", loggedInUserId), ImagePath = "cabbage.png" },
                    new Produce { Name = "Carrots", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Carrots", loggedInUserId), ImagePath = "carrots.png" },
                    new Produce { Name = "Cauliflower", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Cauliflower", loggedInUserId), ImagePath = "cauliflower.png" },
                    new Produce { Name = "Celery", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Celery", loggedInUserId), ImagePath = "celery.png" },
                    new Produce { Name = "Cucumbers", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Cucumbers", loggedInUserId), ImagePath = "cucumber.png" },
                    new Produce { Name = "Eggplant", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Eggplant", loggedInUserId), ImagePath = "eggplant.png" },
                    new Produce { Name = "Garlic", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Garlic", loggedInUserId), ImagePath = "garlic.png" },
                    new Produce { Name = "Green Beans", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Green Beans", loggedInUserId), ImagePath = "greenbeans.png" },
                    new Produce { Name = "Kale", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Kale", loggedInUserId), ImagePath = "kale.png" },
                    new Produce { Name = "Lettuce", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Lettuce", loggedInUserId), ImagePath = "lettuce.png" },
                    new Produce { Name = "Mushrooms", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Mushrooms", loggedInUserId), ImagePath = "mushrooms.png" },
                    new Produce { Name = "Onions", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Onions", loggedInUserId), ImagePath = "onions.png" },
                    new Produce { Name = "Peas", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Peas", loggedInUserId), ImagePath = "peas.png" },
                    new Produce { Name = "Potatoes", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Potatoes", loggedInUserId), ImagePath = "potatoes.png" },
                    new Produce { Name = "Radishes", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Radishes", loggedInUserId), ImagePath = "radishes.png" },
                    new Produce { Name = "Spinach", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Spinach", loggedInUserId), ImagePath = "spinach.png" },
                    new Produce { Name = "Squash", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Squash", loggedInUserId), ImagePath = "squash.png" },
                    new Produce { Name = "Sweet Potatoes", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Sweet Potatoes", loggedInUserId), ImagePath = "sweetpotatoes.png" },
                    new Produce { Name = "Tomatoes", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Tomatoes", loggedInUserId), ImagePath = "tomatoes.png" },
                    new Produce { Name = "Zucchini", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Zucchini", loggedInUserId), ImagePath = "zucchini.png" }


                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeFruits();
            originalItems = new ObservableCollection<Produce>(fruits);
            vegetableListView.ItemsSource = originalItems;
            Title = "Vegetable";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var fruit = (Produce)button.BindingContext;

                fruit.Quantity++;
                if (!selectedFruits.Contains(fruit))
                {
                    selectedFruits.Add(fruit);
                }

                await UpdateDatabase(fruit, 1);
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
            var fruit = (Produce)button.BindingContext;

            await UpdateDatabase(fruit, 1);
            vegetableListView.ItemsSource = null;  // Reset the ItemsSource
            vegetableListView.ItemsSource = fruits;  // Set it back

            fruit.Quantity--;
            if (fruit.Quantity < 0)
                fruit.Quantity = 0;

            await UpdateDatabase(fruit, -1);
        }


        private async Task UpdateDatabase(Produce fruit, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingFruit = await dbContext.GetFruitByNameAndUserId(fruit.Name, loggedInUserId);
                    if (existingFruit != null)
                    {
                        existingFruit.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingFruit.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingFruit);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingFruit);
                        }
                    }
                    else
                    {
                        fruit.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(fruit);
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
            var fruit = (Produce)button.BindingContext;

            if (fruit.Quantity > 0)
            {
                fruit.Quantity = 0;
                vegetableListView.ItemsSource = null;  // Reset the ItemsSource
                vegetableListView.ItemsSource = fruits;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedFruits.Contains(fruit))
                {
                    selectedFruits.Remove(fruit);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingFruit = await dbContext.GetFruitByNameAndUserId(fruit.Name, App.LoggedInUserId);
                if (existingFruit != null)
                {
                    existingFruit.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingFruit);
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