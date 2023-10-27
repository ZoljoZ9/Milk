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
    public partial class GumsMintsLozPage : ContentPage
    {
        private List<Produce> chips;
        private List<Produce> selectedChips; // To store selected fruits

        public int UserId { get; set; }


        public GumsMintsLozPage()
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
                    // Chewing Gum
                    new Produce { Name = "Wrigley's Spearmint Gum", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Wrigley's Spearmint Gum", loggedInUserId), ImagePath = "wrigleys_spearmint_gum.png" },
                    new Produce { Name = "Orbit Peppermint Gum", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Orbit Peppermint Gum", loggedInUserId), ImagePath = "orbit_peppermint_gum.png" },
                    new Produce { Name = "Extra Sugar-Free Gum", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Extra Sugar-Free Gum", loggedInUserId), ImagePath = "extra_sugar_free_gum.png" },
                    // ... Add more chewing gum

                    // Breath Mints
                    new Produce { Name = "Altoids Peppermint Mints", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Altoids Peppermint Mints", loggedInUserId), ImagePath = "altoids_peppermint_mints.png" },
                    new Produce { Name = "Ice Breakers Coolmint Mints", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Ice Breakers Coolmint Mints", loggedInUserId), ImagePath = "ice_breakers_coolmint_mints.png" },
                    new Produce { Name = "Halls Soothing Menthol Cough Drops", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Halls Soothing Menthol Cough Drops", loggedInUserId), ImagePath = "halls_soothing_menthol_cough_drops.png" },
                    // ... Add more breath mints and lozenges

                    // Nicotine Gum
                    new Produce { Name = "Nicorette Nicotine Gum", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Nicorette Nicotine Gum", loggedInUserId), ImagePath = "nicorette_nicotine_gum.png" },
                    // ... Add more nicotine gum

                    // Herbal Lozenges
                    new Produce { Name = "Ricola Herbal Cough Drops", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Ricola Herbal Cough Drops", loggedInUserId), ImagePath = "ricola_herbal_cough_drops.png" },
                    // ... Add more herbal lozenges

                    // Vitamin C Drops
                    new Produce { Name = "Halls Defense Vitamin C Drops", Type = ProduceType.Chip, Quantity = await dbContext.GetChipQuantityByNameAndUserId("Halls Defense Vitamin C Drops", loggedInUserId), ImagePath = "halls_defense_vitamin_c_drops.png" },
                    // ... Add more vitamin C drops
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeChips();
            originalItems = new ObservableCollection<Produce>(chips);
            fruitListView.ItemsSource = originalItems;
            Title = "Gums, Mints, & Lozingers";

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