using Milk.Data;
using Milk.Models;
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

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class fruit : ContentPage
    {
        private List<Produce> fruits;
        private List<Produce> selectedFruits; // To store selected fruits

        public int UserId { get; set; }


        public fruit()
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
                        new Produce { Name = "Apples", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Apples", loggedInUserId), ImagePath = "apple.png" },
                        new Produce { Name = "Avocadoes", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Avocadoes", loggedInUserId), ImagePath = "avocado.png" },
                        new Produce { Name = "Bananas", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Bananas", loggedInUserId), ImagePath = "banana.png" },
                        new Produce { Name = "Berries", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Berries", loggedInUserId), ImagePath = "berries.png" },
                        new Produce { Name = "Citrus fruits", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Citrus fruits", loggedInUserId), ImagePath = "citrus.png" },
                        new Produce { Name = "Coconuts", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Coconuts", loggedInUserId), ImagePath = "coconut.png" },
                        new Produce { Name = "Currants and gooseberries", Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Currants and gooseberries", loggedInUserId), ImagePath = "currants.png" },
                        new Produce { Name = "Dates and figs", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Dates and figs", loggedInUserId), ImagePath = "dates.png" },
                        new Produce { Name = "Dragon fruits", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Dragon fruits", loggedInUserId), ImagePath = "dragonfruit.png" },
                        new Produce { Name = "Exotic fruits", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Exotic fruits", loggedInUserId), ImagePath = "exotic.png" },
                        new Produce { Name = "Grapes", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Grapes", loggedInUserId), ImagePath = "grape.png" },
                        new Produce { Name = "Guavas", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Guavas", loggedInUserId), ImagePath = "guava.png" },
                        new Produce { Name = "Kiwi", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Kiwi", loggedInUserId), ImagePath = "kiwi.png" },
                        new Produce { Name = "Lemons and limes", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Lemons and limes", loggedInUserId), ImagePath = "lemon.png" },
                        new Produce { Name = "Mangos", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Mangos", loggedInUserId), ImagePath = "mango.png" },
                        new Produce { Name = "Melons", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Melons", loggedInUserId), ImagePath = "melon.png" },
                        new Produce { Name = "Oranges", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Oranges", loggedInUserId), ImagePath = "orange.png" },
                        new Produce { Name = "Papayas", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Papayas", loggedInUserId), ImagePath = "papaya.png" },
                        new Produce { Name = "Pears", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Pears", loggedInUserId), ImagePath = "pear.png" },
                        new Produce { Name = "Pineapples", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Pineapples", loggedInUserId), ImagePath = "pineapple.png" },
                        new Produce { Name = "Pomegranates", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Pomegranates", loggedInUserId), ImagePath = "pomegranate.png" },
                        new Produce { Name = "Stone fruits", Type = ProduceType.Fruit, Quantity = await dbContext.GetFruitQuantityByNameAndUserId("Stone fruits", loggedInUserId), ImagePath = "stonefruit.png" }
                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeFruits();
            originalItems = new ObservableCollection<Produce>(fruits);
            fruitListView.ItemsSource = originalItems;
            Title = "Fruit";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var fruit = (Produce)button.BindingContext;

            fruit.Quantity++;
            if (!selectedFruits.Contains(fruit))
            {
                selectedFruits.Add(fruit);
            }


            // Update the database
            await UpdateDatabase(fruit, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var fruit = (Produce)button.BindingContext;

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
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = fruits;  // Set it back

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
                fruitListView.ItemsSource = originalItems;
            }
            else
            {
                fruitListView.ItemsSource = originalItems.Where(item => item.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
        }






    }

}