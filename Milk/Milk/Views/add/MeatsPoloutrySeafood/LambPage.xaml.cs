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
    public partial class LambPage : ContentPage
    {
        private List<Produce> redmeats;
        private List<Produce> selectedRedMeats; // To store selected fruits

        public int UserId { get; set; }


        public LambPage()
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
                    new Produce { Name = "Lamb Chops", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lamb Chops", loggedInUserId), ImagePath = "lamb_chops.png" },
                    new Produce { Name = "Lamb Leg Roast", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lamb Leg Roast", loggedInUserId), ImagePath = "lamb_leg_roast.png" },
                    new Produce { Name = "Lamb Loin", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lamb Loin", loggedInUserId), ImagePath = "lamb_loin.png" },
                    new Produce { Name = "Lamb Mince", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lamb Mince", loggedInUserId), ImagePath = "lamb_mince.png" },
                    new Produce { Name = "Lamb Rack", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lamb Rack", loggedInUserId), ImagePath = "lamb_rack.png" },
                    new Produce { Name = "Lamb Ribs", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lamb Ribs", loggedInUserId), ImagePath = "lamb_ribs.png" },
                    new Produce { Name = "Lamb Shank", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lamb Shank", loggedInUserId), ImagePath = "lamb_shank.png" },
                    new Produce { Name = "Lamb Shoulder", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lamb Shoulder", loggedInUserId), ImagePath = "lamb_shoulder.png" },
                    new Produce { Name = "Lamb Tenderloin", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Lamb Tenderloin", loggedInUserId), ImagePath = "lamb_tenderloin.png" }


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