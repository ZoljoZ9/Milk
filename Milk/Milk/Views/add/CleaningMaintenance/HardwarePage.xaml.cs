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
    public partial class HardwarePage : ContentPage
    {
        private List<Produce> cleanings;
        private List<Produce> selectedCleanings; // To store selected fruits

        public int UserId { get; set; }


        public HardwarePage()
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
                    new Produce { Name = "Batteries", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Batteries", loggedInUserId), ImagePath = "batteries.png" },
                    new Produce { Name = "Cable Ties", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Cable Ties", loggedInUserId), ImagePath = "cableties.png" },
                    new Produce { Name = "Duct Tape", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Duct Tape", loggedInUserId), ImagePath = "ducttape.png" },
                    new Produce { Name = "Glue", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Glue", loggedInUserId), ImagePath = "glue.png" },
                    new Produce { Name = "Hammer", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Hammer", loggedInUserId), ImagePath = "hammer.png" },
                    new Produce { Name = "Nails", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Nails", loggedInUserId), ImagePath = "nails.png" },
                    new Produce { Name = "Screws", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Screws", loggedInUserId), ImagePath = "screws.png" },
                    new Produce { Name = "Tape Measure", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Tape Measure", loggedInUserId), ImagePath = "tapemeasure.png" },
                    new Produce { Name = "Wrench Set", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Wrench Set", loggedInUserId), ImagePath = "wrenchset.png" }


            // ... Continue for other fruits
        };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeCleanings();
            originalItems = new ObservableCollection<Produce>(cleanings);
            fruitListView.ItemsSource = originalItems;
            Title = "Hardware";

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