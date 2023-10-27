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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Produce;

namespace Milk.Views.add.SNC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChipsPage : ContentPage
    {
        private List<Produce> chips;
        private List<Produce> selectedChips; // To store selected fruits

        public int UserId { get; set; }


        public ChipsPage()
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
                  // Smith's Chips
                    new Produce { Name = "Smith's Chips Original", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Smith's Chips Original", loggedInUserId), ImagePath = "smiths_original.png" },
                    new Produce { Name = "Smith's Chips Salt & Vinegar", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Smith's Chips Salt & Vinegar", loggedInUserId), ImagePath = "smiths_salt_vinegar.png" },
                    new Produce { Name = "Smith's Chips BBQ", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Smith's Chips BBQ", loggedInUserId), ImagePath = "smiths_bbq.png" },
                    new Produce { Name = "Smith's Chips Chicken", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Smith's Chips Chicken", loggedInUserId), ImagePath = "smiths_chicken.png" },
                    new Produce { Name = "Smith's Chips Cheese & Onion", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Smith's Chips Cheese & Onion", loggedInUserId), ImagePath = "smiths_cheese_onion.png" },
                    new Produce { Name = "Smith's Chips Sour Cream & Chives", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Smith's Chips Sour Cream & Chives", loggedInUserId), ImagePath = "smiths_sour_cream_chives.png" },

                    // Doritos
                    new Produce { Name = "Doritos Nacho Cheese", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Doritos Nacho Cheese", loggedInUserId), ImagePath = "doritos_nacho_cheese.png" },
                    new Produce { Name = "Doritos Cool Ranch", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Doritos Cool Ranch", loggedInUserId), ImagePath = "doritos_cool_ranch.png" },
                    new Produce { Name = "Doritos Spicy Sweet Chili", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Doritos Spicy Sweet Chili", loggedInUserId), ImagePath = "doritos_spicy_sweet_chili.png" },
                    new Produce { Name = "Doritos Original Corn Chips", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Doritos Original Corn Chips", loggedInUserId), ImagePath = "doritos_original_corn_chips.png" },

                    // Red Rock Deli
                    new Produce { Name = "Red Rock Deli Sea Salt", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Red Rock Deli Sea Salt", loggedInUserId), ImagePath = "red_rock_deli_sea_salt.png" },
                    new Produce { Name = "Red Rock Deli Honey Soy Chicken", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Red Rock Deli Honey Soy Chicken", loggedInUserId), ImagePath = "red_rock_deli_honey_soy_chicken.png" },
                    new Produce { Name = "Red Rock Deli Sweet Chilli & Sour Cream", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Red Rock Deli Sweet Chilli & Sour Cream", loggedInUserId), ImagePath = "red_rock_deli_sweet_chilli_sour_cream.png" },
                    new Produce { Name = "Red Rock Deli Lime & Cracked Pepper", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Red Rock Deli Lime & Cracked Pepper", loggedInUserId), ImagePath = "red_rock_deli_lime_cracked_pepper.png" },
                    new Produce { Name = "Red Rock Deli Roasted Garlic & Rosemary", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Red Rock Deli Roasted Garlic & Rosemary", loggedInUserId), ImagePath = "red_rock_deli_roasted_garlic_rosemary.png" },

                    // Pringles
                    new Produce { Name = "Pringles Original", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Pringles Original", loggedInUserId), ImagePath = "pringles_original.png" },
                    new Produce { Name = "Pringles Sour Cream & Onion", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Pringles Sour Cream & Onion", loggedInUserId), ImagePath = "pringles_sour_cream_onion.png" },
                    new Produce { Name = "Pringles Salt & Vinegar", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Pringles Salt & Vinegar", loggedInUserId), ImagePath = "pringles_salt_vinegar.png" },
                    new Produce { Name = "Pringles BBQ", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Pringles BBQ", loggedInUserId), ImagePath = "pringles_bbq.png" },

                    // Thins
                    new Produce { Name = "Thins Original", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Thins Original", loggedInUserId), ImagePath = "thins_original.png" },
                    new Produce { Name = "Thins Light & Tangy", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Thins Light & Tangy", loggedInUserId), ImagePath = "thins_light_tangy.png" },
                    new Produce { Name = "Thins Chicken", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Thins Chicken", loggedInUserId), ImagePath = "thins_chicken.png" },
                    new Produce { Name = "Thins Salt & Vinegar", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Thins Salt & Vinegar", loggedInUserId), ImagePath = "thins_salt_vinegar.png" },

                    // Kettle Chips
                    new Produce { Name = "Kettle Chips Sea Salt", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Kettle Chips Sea Salt", loggedInUserId), ImagePath = "kettle_sea_salt.png" },
                    new Produce { Name = "Kettle Chips Honey Soy Chicken", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Kettle Chips Honey Soy Chicken", loggedInUserId), ImagePath = "kettle_honey_soy_chicken.png" },
                    new Produce { Name = "Kettle Chips Salt & Vinegar", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Kettle Chips Salt & Vinegar", loggedInUserId), ImagePath = "kettle_salt_vinegar.png" },
                    new Produce { Name = "Kettle Chips Sour Cream & Chives", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Kettle Chips Sour Cream & Chives", loggedInUserId), ImagePath = "kettle_sour_cream_chives.png" },
                    new Produce { Name = "Kettle Chips Rosemary & Sea Salt", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Kettle Chips Rosemary & Sea Salt", loggedInUserId), ImagePath = "kettle_rosemary_sea_salt.png" },

                    // Grain Waves
                    new Produce { Name = "Grain Waves Original", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Grain Waves Original", loggedInUserId), ImagePath = "grain_waves_original.png" },
                    new Produce { Name = "Grain Waves Sour Cream", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Grain Waves Sour Cream", loggedInUserId), ImagePath = "grain_waves_sour_cream.png" },
                    new Produce { Name = "Grain Waves Cheddar Cheese", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Grain Waves Cheddar Cheese", loggedInUserId), ImagePath = "grain_waves_cheddar_cheese.png" },

                    // CC's
                    new Produce { Name = "CC's Original", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("CC's Original", loggedInUserId), ImagePath = "ccs_original.png" },
                    new Produce { Name = "CC's Tasty Cheese", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("CC's Tasty Cheese", loggedInUserId), ImagePath = "ccs_tasty_cheese.png" },
                    new Produce { Name = "CC's Sour Cream & Chives", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("CC's Sour Cream & Chives", loggedInUserId), ImagePath = "ccs_sour_cream_chives.png" },

                    // Tyrrell's
                    new Produce { Name = "Tyrrell's Sea Salt & Cider Vinegar", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Tyrrell's Sea Salt & Cider Vinegar", loggedInUserId), ImagePath = "tyrrells_sea_salt_cider_vinegar.png" },
                    new Produce { Name = "Tyrrell's Mature Cheddar & Chive", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Tyrrell's Mature Cheddar & Chive", loggedInUserId), ImagePath = "tyrrells_mature_cheddar_chive.png" },
                    new Produce { Name = "Tyrrell's Sweet Chili & Red Pepper", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Tyrrell's Sweet Chili & Red Pepper", loggedInUserId), ImagePath = "tyrrells_sweet_chili_red_pepper.png" },

                    // Twisties
                    new Produce { Name = "Twisties Cheese", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Twisties Cheese", loggedInUserId), ImagePath = "twisties_cheese.png" },
                    new Produce { Name = "Twisties Chicken", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Twisties Chicken", loggedInUserId), ImagePath = "twisties_chicken.png" }
            };



        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeChips();
            originalItems = new ObservableCollection<Produce>(chips);
            fruitListView.ItemsSource = originalItems;
            Title = "Chips";

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