using Milk.Data;
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

namespace Milk.Views.add.Drinking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CordialsJuiceIcedTeasPage : ContentPage
    {
        private List<Produce> drinks;
        private List<Produce> selectedDrinks; // To store selected fruits

        public int UserId { get; set; }


        public CordialsJuiceIcedTeasPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedDrinks = new List<Produce>();
        }


        private async Task InitializeDrinks()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            drinks = new List<Produce>
                {
                    new Produce { Name = "Orange Juice", Type = ProduceType.Drink, Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Orange Juice", loggedInUserId), ImagePath = "orangejuice.png" },
                    new Produce { Name = "Apple Juice", Type = ProduceType.Drink, Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Apple Juice", loggedInUserId), ImagePath = "applejuice.png" },
                    new Produce { Name = "Grape Juice", Type = ProduceType.Drink, Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Grape Juice", loggedInUserId), ImagePath = "grapejuice.png" },
                    new Produce { Name = "Cranberry Juice", Type = ProduceType.Drink, Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Cranberry Juice", loggedInUserId), ImagePath = "cranberryjuice.png" },
                    new Produce { Name = "Lemonade", Type = ProduceType.Drink, Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Lemonade", loggedInUserId), ImagePath = "lemonade.png" },
                    new Produce { Name = "Iced Tea (Lemon)", Type = ProduceType.Drink, Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Iced Tea (Lemon)", loggedInUserId), ImagePath = "icedtealemon.png" },
                    new Produce { Name = "Iced Tea (Peach)", Type = ProduceType.Drink   , Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Iced Tea (Peach)", loggedInUserId), ImagePath = "icedteapeach.png" },
                    new Produce { Name = "Cordial (Raspberry)", Type = ProduceType.Drink, Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Cordial (Raspberry)", loggedInUserId), ImagePath = "cordialraspberry.png" },
                    new Produce { Name = "Cordial (Orange)", Type = ProduceType.Drink, Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Cordial (Orange)", loggedInUserId), ImagePath = "cordialorange.png" },
                    new Produce { Name = "Cordial (Blackcurrant)", Type = ProduceType.Drink, Quantity = await dbContext.GetDrinkQuantityByNameAndUserId("Cordial (Blackcurrant)", loggedInUserId), ImagePath = "cordialblackcurrant.png" },



                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeDrinks();
            originalItems = new ObservableCollection<Produce>(drinks);
            fruitListView.ItemsSource = originalItems;

            Title = "Cordials, Juice, & Ice Teas";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var drink = (Produce)button.BindingContext;

            drink.Quantity++;
            if (!selectedDrinks.Contains(drink))
            {
                selectedDrinks.Add(drink);
            }


            // Update the database
            await UpdateDatabase(drink, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var drink = (Produce)button.BindingContext;

            drink.Quantity--;

            if (drink.Quantity < 0)
                drink.Quantity = 0;

            await UpdateDatabase(drink, -1);
        }

        private async Task UpdateDatabase(Produce drink, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingDrinks = await dbContext.GetDrinkByNameAndUserId(drink.Name, loggedInUserId);
                    if (existingDrinks != null)
                    {
                        existingDrinks.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingDrinks.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingDrinks);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingDrinks);
                        }
                    }
                    else
                    {
                        drink.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(drink);
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
            var drink = (Produce)button.BindingContext;

            if (drink.Quantity > 0)
            {
                drink.Quantity = 0;
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = drinks;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedDrinks.Contains(drink))
                {
                    selectedDrinks.Remove(drink);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingDrink = await dbContext.GetDrinkByNameAndUserId(drink.Name, App.LoggedInUserId);
                if (existingDrink != null)
                {
                    existingDrink.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingDrink);
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