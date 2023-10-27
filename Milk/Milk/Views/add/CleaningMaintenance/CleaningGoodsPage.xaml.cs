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
    public partial class CleaningGoodsPage : ContentPage
    {
        private List<Produce> cleanings;
        private List<Produce> selectedCleanings; // To store selected fruits

        public int UserId { get; set; }


        public CleaningGoodsPage()
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
                    new Produce { Name = "All-Purpose Cleaner", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("All-Purpose Cleaner", loggedInUserId), ImagePath = "allpurposecleaner.png" },
                    new Produce { Name = "Bleach", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Bleach", loggedInUserId), ImagePath = "bleach.png" },
                    new Produce { Name = "Dishwashing Liquid", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Dishwashing Liquid", loggedInUserId), ImagePath = "dishwashingliquid.png" },
                    new Produce { Name = "Disinfectant Wipes", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Disinfectant Wipes", loggedInUserId), ImagePath = "disinfectantwipes.png" },
                    new Produce { Name = "Floor Cleaner", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Floor Cleaner", loggedInUserId), ImagePath = "floorcleaner.png" },
                    new Produce { Name = "Glass Cleaner Spray", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Glass Cleaner Spray", loggedInUserId), ImagePath = "glasscleaner.png" },
                    new Produce { Name = "Laundry Detergent", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Laundry Detergent", loggedInUserId), ImagePath = "laundrydetergent.png" },
                    new Produce { Name = "Oven Cleaner", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Oven Cleaner", loggedInUserId), ImagePath = "ovencleaner.png" },
                    new Produce { Name = "Toilet Bowl Cleaner", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Toilet Bowl Cleaner", loggedInUserId), ImagePath = "toiletcleaner.png" }



            // ... Continue for other fruits
        };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeCleanings();
            originalItems = new ObservableCollection<Produce>(cleanings);
            fruitListView.ItemsSource = originalItems;
            Title = "Cleaning Goods";

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