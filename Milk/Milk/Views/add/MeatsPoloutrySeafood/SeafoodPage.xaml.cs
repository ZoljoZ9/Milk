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
    public partial class SeafoodPage : ContentPage
    {
        private List<Produce> redmeats;
        private List<Produce> selectedRedMeats; // To store selected fruits

        public int UserId { get; set; }


        public SeafoodPage()
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
                    new Produce { Name = "Anchovy", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Anchovy", loggedInUserId), ImagePath = "anchovy.png" },
                    new Produce { Name = "Clams", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Clams", loggedInUserId), ImagePath = "clams.png" },
                    new Produce { Name = "Cod Fillet", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Cod Fillet", loggedInUserId), ImagePath = "cod_fillet.png" },
                    new Produce { Name = "Crab Meat", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Crab Meat", loggedInUserId), ImagePath = "crab_meat.png" },
                    new Produce { Name = "Lobster Tail", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lobster Tail", loggedInUserId), ImagePath = "lobster_tail.png" },
                    new Produce { Name = "Mackerel", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Mackerel", loggedInUserId), ImagePath = "mackerel.png" },
                    new Produce { Name = "Oysters", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Oysters", loggedInUserId), ImagePath = "oysters.png" },
                    new Produce { Name = "Prawns", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Prawns", loggedInUserId), ImagePath = "prawns.png" },
                    new Produce { Name = "Salmon Fillet", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Salmon Fillet", loggedInUserId), ImagePath = "salmon_fillet.png" },
                    new Produce { Name = "Shrimps", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Shrimps", loggedInUserId), ImagePath = "shrimps.png" },
                    new Produce { Name = "Tilapia", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Tilapia", loggedInUserId), ImagePath = "tilapia.png" },
                    new Produce { Name = "Tuna Steak", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Tuna Steak", loggedInUserId), ImagePath = "tuna_steak.png" }

                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeRedMeats();
            originalItems = new ObservableCollection<Produce>(redmeats);
            RedMeatListView.ItemsSource = originalItems;
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
                var dbContext = new AppDbContext(App.DatabasePath);
                var loggedInUserId = App.LoggedInUserId;

                var existingRedMeat = await dbContext.GetRedMeatByNameAndUserId(redmeat.Name, loggedInUserId);
                if (existingRedMeat != null)
                {
                    // Adjust quantity for the existing fruit by either 1 or -1, depending on the "adjustment" parameter
                    existingRedMeat.Quantity += adjustment;
                    await dbContext.Database.UpdateAsync(existingRedMeat);
                    MessagingCenter.Send<object>(this, "UpdateList");

                }
                else
                {
                    // New fruit, so insert it
                    redmeat.UserId = loggedInUserId;
                    await dbContext.Database.InsertAsync(redmeat);
                    MessagingCenter.Send<object>(this, "UpdateList");

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