using Milk.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Produce;

namespace Milk.Views.add.CleaningMaintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToiletPaperTissuesPaperTowelsPage : ContentPage
    {
        private List<Produce> cleanings;
        private List<Produce> selectedCleanings; // To store selected fruits

        public int UserId { get; set; }


        public ToiletPaperTissuesPaperTowelsPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedCleanings = new List<Produce>();
        }


        private async Task InitializeCleanings()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            cleanings = new List<Produce>
                {
                    new Produce { Name = "2-Ply Toilet Paper (12 Rolls)", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("2-Ply Toilet Paper (12 Rolls)", loggedInUserId), ImagePath = "2plytoiletpaper.png" },
                    new Produce { Name = "3-Ply Toilet Paper (6 Rolls)", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("3-Ply Toilet Paper (6 Rolls)", loggedInUserId), ImagePath = "3plytoiletpaper.png" },
                    new Produce { Name = "Boxed Facial Tissues", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Boxed Facial Tissues", loggedInUserId), ImagePath = "boxtissues.png" },
                    new Produce { Name = "Kitchen Paper Towels (2 Rolls)", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Kitchen Paper Towels (2 Rolls)", loggedInUserId), ImagePath = "papertowels.png" },
                    new Produce { Name = "Pocket Tissues (10 Packets)", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Pocket Tissues (10 Packets)", loggedInUserId), ImagePath = "pockettissues.png" },
                    new Produce { Name = "Recycled Toilet Paper (12 Rolls)", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Recycled Toilet Paper (12 Rolls)", loggedInUserId), ImagePath = "recycledtoiletpaper.png" },
                    new Produce { Name = "Wet Wipes (Sensitive)", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Wet Wipes (Sensitive)", loggedInUserId), ImagePath = "wetwipessensitive.png" },
                    new Produce { Name = "Wet Wipes (Antibacterial)", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Wet Wipes (Antibacterial)", loggedInUserId), ImagePath = "wetwipesantibacterial.png" }

            // ... Continue for other fruits
        };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeCleanings();
            originalItems = new ObservableCollection<Produce>(cleanings);
            fruitListView.ItemsSource = originalItems;
            Title = "Toilet Paper, Tissues, & Paper Towels";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cleaning = (Produce)button.BindingContext;

            cleaning.Quantity++;
            if (!selectedCleanings.Contains(cleaning))
            {
                selectedCleanings.Add(cleaning);
            }


            // Update the database
            await UpdateDatabase(cleaning, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var cleaning = (Produce)button.BindingContext;

            cleaning.Quantity--;

            if (cleaning.Quantity < 0)
                cleaning.Quantity = 0;

            await UpdateDatabase(cleaning, -1);
        }

        private async Task UpdateDatabase(Produce cleaning, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingCleanings = await dbContext.GetCleaningByNameAndUserId(cleaning.Name, loggedInUserId);
                    if (existingCleanings != null)
                    {
                        existingCleanings.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingCleanings.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingCleanings);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingCleanings);
                        }
                    }
                    else
                    {
                        cleaning.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(cleaning);
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
            var cleaning = (Produce)button.BindingContext;

            if (cleaning.Quantity > 0)
            {
                cleaning.Quantity = 0;
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = cleanings;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedCleanings.Contains(cleaning))
                {
                    selectedCleanings.Remove(cleaning);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingDrink = await dbContext.GetDrinkByNameAndUserId(cleaning.Name, App.LoggedInUserId);
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