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
using Microsoft.Maui.Controls.Xaml;
using static Produce;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add.SNC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BiscutCrackersPage : ContentPage
    {
        private List<Produce> chips;
        private List<Produce> selectedChips; // To store selected fruits

        public int UserId { get; set; }


        public BiscutCrackersPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedChips = new List<Produce>();
        }


        private async Task InitializeChips()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            chips = new List<Produce>
                {
            // Biscuits and crackers at Woolworths
            
                // Biscuits
                new Produce { Name = "Arnott's Tim Tams", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Arnott's Tim Tams", loggedInUserId), ImagePath = "arnotts_tim_tams.png" },
                new Produce { Name = "McVitie's Digestive Biscuits", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("McVitie's Digestive Biscuits", loggedInUserId), ImagePath = "mcvities_digestive_biscuits.png" },
                new Produce { Name = "Arnott's Iced VoVo", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Arnott's Iced VoVo", loggedInUserId), ImagePath = "arnotts_iced_vovo.png" },
                // ... Add more biscuits

                // Crackers
                new Produce { Name = "Ritz Crackers", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Ritz Crackers", loggedInUserId), ImagePath = "ritz_crackers.png" },
                new Produce { Name = "Jatz Crackers", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Jatz Crackers", loggedInUserId), ImagePath = "jatz_crackers.png" },
                new Produce { Name = "Savoiardi Ladyfingers", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Savoiardi Ladyfingers", loggedInUserId), ImagePath = "savoiardi_ladyfingers.png" },
                // ... Add more crackers

                // Shortbread
                new Produce { Name = "Walkers Shortbread", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Walkers Shortbread", loggedInUserId), ImagePath = "walkers_shortbread.png" },
                // ... Add more shortbread

                // Wheat Thins
                new Produce { Name = "Nabisco Wheat Thins", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Nabisco Wheat Thins", loggedInUserId), ImagePath = "nabisco_wheat_thins.png" },
                // ... Add more Wheat Thins

                // Oatcakes
                new Produce { Name = "Nairn's Oatcakes", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Nairn's Oatcakes", loggedInUserId), ImagePath = "nairns_oatcakes.png" },
                // ... Add more oatcakes

                // Animal Crackers
                new Produce { Name = "Stauffer's Animal Crackers", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Stauffer's Animal Crackers", loggedInUserId), ImagePath = "stauffers_animal_crackers.png" },
                // ... Add more animal crackers

                // Rice Cakes
                new Produce { Name = "Quaker Rice Cakes", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Quaker Rice Cakes", loggedInUserId), ImagePath = "quaker_rice_cakes.png" },
                // ... Add more rice cakes

                // Graham Crackers
                new Produce { Name = "Honey Maid Graham Crackers", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Honey Maid Graham Crackers", loggedInUserId), ImagePath = "honey_maid_graham_crackers.png" },
                // ... Add more graham crackers

                // Cheese Biscuits
                new Produce { Name = "Jacob's Cream Crackers", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Jacob's Cream Crackers", loggedInUserId), ImagePath = "jacobs_cream_crackers.png" },
                // ... Add more cheese biscuits
            };



        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeChips();
            originalItems = new ObservableCollection<Produce>(chips);
            fruitListView.ItemsSource = originalItems;
            Title = "Biscut & Crackers";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var chip = (Produce)button.BindingContext;

            chip.Quantity++;
            if (!selectedChips.Contains(chip))
            {
                selectedChips.Add(chip);
            }


            // Update the database
            await UpdateDatabase(chip, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var chip = (Produce)button.BindingContext;

            chip.Quantity--;

            if (chip.Quantity < 0)
                chip.Quantity = 0;

            await UpdateDatabase(chip, -1);
        }

        private async Task UpdateDatabase(Produce chip, int adjustment)
        {
            try
            {
                var dbContext = new AppDbContext(App.DatabasePath);
                var loggedInUserId = App.LoggedInUserId;

                var existingChip = await dbContext.GetChipByNameAndUserId(chip.Name, loggedInUserId);
                if (existingChip != null)
                {
                    // Adjust quantity for the existing fruit by either 1 or -1, depending on the "adjustment" parameter
                    existingChip.Quantity += adjustment;
                    await dbContext.Database.UpdateAsync(existingChip);
                    MessagingCenter.Send<object>(this, "UpdateList");

                }
                else
                {
                    // New fruit, so insert it
                    chip.UserId = loggedInUserId;
                    await dbContext.Database.InsertAsync(chip);
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
            var chip = (Produce)button.BindingContext;

            if (chip.Quantity > 0)
            {
                chip.Quantity = 0;
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = chips;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedChips.Contains(chip))
                {
                    selectedChips.Remove(chip);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingChip = await dbContext.GetChipByNameAndUserId(chip.Name, App.LoggedInUserId);
                if (existingChip != null)
                {
                    existingChip.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingChip);
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