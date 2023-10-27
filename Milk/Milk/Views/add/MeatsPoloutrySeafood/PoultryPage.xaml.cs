using Milk.Data;
using Milk;
using static Produce;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Milk.Models;
using SQLite;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Milk.Views.add;
using Milk.Views.add.MeatsPoloutrySeafood;

namespace Milk.Views.add.MeatsPoloutrySeafood
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PoultryPage : ContentPage
    {
        private List<Produce> redmeats;
        private List<Produce> selectedRedMeats; // To store selected fruits

        public int UserId { get; set; }


        public PoultryPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedRedMeats = new List<Produce>();
        }


        private async Task InitializeRedMeats()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            redmeats = new List<Produce>
                {
                    new Produce { Name = "Chicken Breast", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Chicken Breast", loggedInUserId), ImagePath = "chicken_breast.png" },
                    new Produce { Name = "Chicken Drumstick", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Chicken Drumstick", loggedInUserId), ImagePath = "chicken_drumstick.png" },
                    new Produce { Name = "Chicken Leg", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Chicken Leg", loggedInUserId), ImagePath = "chicken_leg.png" },
                    new Produce { Name = "Chicken Thigh", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Chicken Thigh", loggedInUserId), ImagePath = "chicken_thigh.png" },
                    new Produce { Name = "Chicken Wing", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Chicken Wing", loggedInUserId), ImagePath = "chicken_wing.png" },
                    new Produce { Name = "Whole Chicken", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Whole Chicken", loggedInUserId), ImagePath = "whole_chicken.png" },
                    new Produce { Name = "Duck Breast", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Duck Breast", loggedInUserId), ImagePath = "duck_breast.png" },
                    new Produce { Name = "Whole Duck", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Whole Duck", loggedInUserId), ImagePath = "whole_duck.png" },
                    new Produce { Name = "Turkey Breast", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Turkey Breast", loggedInUserId), ImagePath = "turkey_breast.png" },
                    new Produce { Name = "Whole Turkey", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Whole Turkey", loggedInUserId), ImagePath = "whole_turkey.png" }

                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeRedMeats();
            originalItems = new ObservableCollection<Produce>(redmeats);
            RedMeatListView.ItemsSource = originalItems;
            Title = "Poultry";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var redmeat = (Produce)button.BindingContext;

            redmeat.Quantity++;
            if (!selectedRedMeats.Contains(redmeat))
            {
                selectedRedMeats.Add(redmeat);
            }


            // Update the database
            await UpdateDatabase(redmeat, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var redmeat = (Produce)button.BindingContext;

            redmeat.Quantity--;

            if (redmeat.Quantity < 0)
                redmeat.Quantity = 0;

            await UpdateDatabase(redmeat, -1);
        }

        private async Task UpdateDatabase(Produce redmeat, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingRedMeats = await dbContext.GetRedMeatByNameAndUserId(redmeat.Name, loggedInUserId);
                    if (existingRedMeats != null)
                    {
                        existingRedMeats.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingRedMeats.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingRedMeats);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingRedMeats);
                        }
                    }
                    else
                    {
                        redmeat.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(redmeat);
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
            var redmeat = (Produce)button.BindingContext;

            if (redmeat.Quantity > 0)
            {
                redmeat.Quantity = 0;
                RedMeatListView.ItemsSource = null;  // Reset the ItemsSource
                RedMeatListView.ItemsSource = redmeats;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedRedMeats.Contains(redmeat))
                {
                    selectedRedMeats.Remove(redmeat);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingRedMeat = await dbContext.GetRedMeatByNameAndUserId(redmeat.Name, App.LoggedInUserId);
                if (existingRedMeat != null)
                {
                    existingRedMeat.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingRedMeat);
                }
            }
        }
        private ObservableCollection<Produce> originalItems;


        void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = searchBar.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                RedMeatListView.ItemsSource = originalItems;
            }
            else
            {
                RedMeatListView.ItemsSource = originalItems.Where(item => item.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
        }
    }
}