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
    public partial class LaundryPage : ContentPage
    {
        private List<Produce> cleanings;
        private List<Produce> selectedCleanings; // To store selected fruits

        public int UserId { get; set; }


        public LaundryPage()
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
                    new Produce { Name = "Bleach", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Bleach", loggedInUserId), ImagePath = "bleach.png" },
                    new Produce { Name = "Clothespins", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Clothespins", loggedInUserId), ImagePath = "clothespins.png" },
                    new Produce { Name = "Detergent (Liquid)", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Detergent (Liquid)", loggedInUserId), ImagePath = "liquiddetergent.png" },
                    new Produce { Name = "Detergent (Powder)", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Detergent (Powder)", loggedInUserId), ImagePath = "powderdetergent.png" },
                    new Produce { Name = "Fabric Softener", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Fabric Softener", loggedInUserId), ImagePath = "fabricsoftener.png" },
                    new Produce { Name = "Laundry Basket", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Laundry Basket", loggedInUserId), ImagePath = "laundrybasket.png" },
                    new Produce { Name = "Laundry Sanitizer", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Laundry Sanitizer", loggedInUserId), ImagePath = "laundrysanitizer.png" },
                    new Produce { Name = "Stain Remover", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Stain Remover", loggedInUserId), ImagePath = "stainremover.png" },
                    new Produce { Name = "Washing Machine Cleaner", Type = ProduceType.Cleaning, Quantity = await dbContext.GetCleaningQuantityByNameAndUserId("Washing Machine Cleaner", loggedInUserId), ImagePath = "washingmachinecleaner.png" }



            // ... Continue for other fruits
        };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeCleanings();
            originalItems = new ObservableCollection<Produce>(cleanings);
            fruitListView.ItemsSource = originalItems;
            Title = "Laundry";

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