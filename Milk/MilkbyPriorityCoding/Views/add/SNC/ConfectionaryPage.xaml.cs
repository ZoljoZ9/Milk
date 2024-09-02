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
    public partial class ConfectionaryPage : ContentPage
    {
        private List<Produce> chips;
        private List<Produce> selectedChips; // To store selected fruits

        public int UserId { get; set; }


        public ConfectionaryPage()
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

                // Chocolates
                new Produce { Name = "Cadbury Dairy Milk Chocolate", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Cadbury Dairy Milk Chocolate", loggedInUserId), ImagePath = "cadbury_dairy_milk_chocolate.png" },
                new Produce { Name = "Nestle Kit Kat", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Nestle Kit Kat", loggedInUserId), ImagePath = "nestle_kit_kat.png" },
                new Produce { Name = "Mars Bar", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Mars Bar", loggedInUserId), ImagePath = "mars_bar.png" },
                // ... Add more chocolates

                // Gummies and Candies
                new Produce { Name = "Haribo Gummy Bears", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Haribo Gummy Bears", loggedInUserId), ImagePath = "haribo_gummy_bears.png" },
                new Produce { Name = "Skittles", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Skittles", loggedInUserId), ImagePath = "skittles.png" },
                new Produce { Name = "Sour Patch Kids", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Sour Patch Kids", loggedInUserId), ImagePath = "sour_patch_kids.png" },
                // ... Add more gummies and candies

                // Licorice
                new Produce { Name = "Red Vines Licorice", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Red Vines Licorice", loggedInUserId), ImagePath = "red_vines_licorice.png" },
                new Produce { Name = "Twizzlers Licorice", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Twizzlers Licorice", loggedInUserId), ImagePath = "twizzlers_licorice.png" },
                // ... Add more licorice products

                // Chewing Gum
                new Produce { Name = "Wrigley's Spearmint Gum", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Wrigley's Spearmint Gum", loggedInUserId), ImagePath = "wrigleys_spearmint_gum.png" },
                new Produce { Name = "Orbit Peppermint Gum", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Orbit Peppermint Gum", loggedInUserId), ImagePath = "orbit_peppermint_gum.png" },
                // ... Add more chewing gum

                // Mints
                new Produce { Name = "Altoids Peppermint Mints", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Altoids Peppermint Mints", loggedInUserId), ImagePath = "altoids_peppermint_mints.png" },
                new Produce { Name = "Lifesavers Wint-O-Green Mints", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Lifesavers Wint-O-Green Mints", loggedInUserId), ImagePath = "lifesavers_wintogreen_mints.png" },
                // ... Add more mints

                // Lollipops
                new Produce { Name = "Tootsie Pop", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Tootsie Pop", loggedInUserId), ImagePath = "tootsie_pop.png" },
                new Produce { Name = "Chupa Chups", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Chupa Chups", loggedInUserId), ImagePath = "chupa_chups.png" },
                // ... Add more lollipops

                // Chocolate Bars
                new Produce { Name = "Hershey's Cookies 'n' Creme Bar", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Hershey's Cookies 'n' Creme Bar", loggedInUserId), ImagePath = "hersheys_cookies_n_creme_bar.png" },
                new Produce { Name = "Milka Chocolate Bar", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Milka Chocolate Bar", loggedInUserId), ImagePath = "milka_chocolate_bar.png" },
                // ... Add more chocolate bars

                // Taffy
                new Produce { Name = "Laffy Taffy", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Laffy Taffy", loggedInUserId), ImagePath = "laffy_taffy.png" },
                new Produce { Name = "Airheads", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Airheads", loggedInUserId), ImagePath = "airheads.png" },
                // ... Add more taffy

                // Turkish Delight
                new Produce { Name = "Lokum Turkish Delight", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Lokum Turkish Delight", loggedInUserId), ImagePath = "lokum_turkish_delight.png" },
                // ... Add more Turkish Delight

                // Licorice Allsorts
                new Produce { Name = "Bassett's Licorice Allsorts", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Bassett's Licorice Allsorts", loggedInUserId), ImagePath = "bassetts_licorice_allsorts.png" },
                // ... Add more Licorice Allsorts

                // Jelly Beans
                new Produce { Name = "Jelly Belly Jelly Beans", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Jelly Belly Jelly Beans", loggedInUserId), ImagePath = "jelly_belly_jelly_beans.png" },
                // ... Add more jelly beans

                // Caramels
                new Produce { Name = "Werther's Original Caramels", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Werther's Original Caramels", loggedInUserId), ImagePath = "werthers_original_caramels.png" },
                // ... Add more caramels

                // Marshmallows
                new Produce { Name = "Jet-Puffed Marshmallows", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Jet-Puffed Marshmallows", loggedInUserId), ImagePath = "jet_puffed_marshmallows.png" },
                // ... Add more marshmallows

                // Liqueur Chocolates
                new Produce { Name = "Anthon Berg Liqueur Chocolates", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Anthon Berg Liqueur Chocolates", loggedInUserId), ImagePath = "anthon_berg_liqueur_chocolates.png" },
                // ... Add more liqueur chocolates
            };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeChips();
            originalItems = new ObservableCollection<Produce>(chips);
            fruitListView.ItemsSource = originalItems;
            Title = "Confectionary";

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