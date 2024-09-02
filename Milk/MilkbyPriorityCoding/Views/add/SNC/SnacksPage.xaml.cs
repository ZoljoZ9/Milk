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
    public partial class SnacksPage : ContentPage
    {
        private List<Produce> chips;
        private List<Produce> selectedChips; // To store selected fruits

        public int UserId { get; set; }


        public SnacksPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedChips = new List<Produce>();
        }


        private async Task InitializeChips()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            // Example list of confectionery products at Woolworths
            chips = new List<Produce>
                {
                    // Potato Chips
                    new Produce { Name = "Smith's Original Chips", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Smith's Original Chips", loggedInUserId), ImagePath = "smiths_original_chips.png" },
                    new Produce { Name = "Smith's Salt & Vinegar Chips", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Smith's Salt & Vinegar Chips", loggedInUserId), ImagePath = "smiths_salt_and_vinegar_chips.png" },
                    new Produce { Name = "Smith's BBQ Chips", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Smith's BBQ Chips", loggedInUserId), ImagePath = "smiths_bbq_chips.png" },
                    // ... Add more potato chips

                    // Popcorn
                    new Produce { Name = "Orville Redenbacher's Movie Theater Butter Popcorn", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Orville Redenbacher's Movie Theater Butter Popcorn", loggedInUserId), ImagePath = "orville_redenbachers_popcorn.png" },
                    new Produce { Name = "Cobs Sea Salt Popcorn", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Cobs Sea Salt Popcorn", loggedInUserId), ImagePath = "cobs_sea_salt_popcorn.png" },
                    new Produce { Name = "Kettle Sea Salt Popcorn", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Kettle Sea Salt Popcorn", loggedInUserId), ImagePath = "kettle_sea_salt_popcorn.png" },
                    // ... Add more popcorn

                    // Trail Mix
                    new Produce { Name = "Tasti Nut & Honey Trail Mix", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Tasti Nut & Honey Trail Mix", loggedInUserId), ImagePath = "tasti_nut_and_honey_trail_mix.png" },
                    new Produce { Name = "Be Natural Deluxe Almond, Cashew & Coconut Trail Mix", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Be Natural Deluxe Almond, Cashew & Coconut Trail Mix", loggedInUserId), ImagePath = "be_natural_deluxe_trail_mix.png" },
                    // ... Add more trail mix

                    // Crackers
                    new Produce { Name = "Jatz Original Crackers", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Jatz Original Crackers", loggedInUserId), ImagePath = "jatz_original_crackers.png" },
                    new Produce { Name = "Sakata Rice Crackers Original", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Sakata Rice Crackers Original", loggedInUserId), ImagePath = "sakata_rice_crackers.png" },
                    // ... Add more crackers

                    // Dips
                    new Produce { Name = "Obela Smooth Classic Hommus", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Obela Smooth Classic Hommus", loggedInUserId), ImagePath = "obela_hommus.png" },
                    new Produce { Name = "Tzatziki Dip", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Tzatziki Dip", loggedInUserId), ImagePath = "tzatziki_dip.png" },
                    // ... Add more dips
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeChips();
            originalItems = new ObservableCollection<Produce>(chips);
            fruitListView.ItemsSource = originalItems;
            Title = "Snacks";

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