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

namespace Milk.Views.add.FrozenFood
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrozenFruitPage : ContentPage
    {
        private List<Produce> frozens;
        private List<Produce> selectedFrozens; // To store selected fruits

        public int UserId { get; set; }


        public FrozenFruitPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedFrozens = new List<Produce>();
        }


        private async Task InitializeFrozens()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            frozens = new List<Produce>
                {
                    new Produce { Name = "Mixed Berries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Mixed Berries", loggedInUserId), ImagePath = "mixedberries.png" },
                    new Produce { Name = "Sliced Strawberries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Sliced Strawberries", loggedInUserId), ImagePath = "slicedstrawberries.png" },
                    new Produce { Name = "Blueberries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Blueberries", loggedInUserId), ImagePath = "blueberries.png" },
                    new Produce { Name = "Mango Chunks", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Mango Chunks", loggedInUserId), ImagePath = "mangochunks.png" },
                    new Produce { Name = "Raspberries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Raspberries", loggedInUserId), ImagePath = "raspberries.png" },
                    new Produce { Name = "Pineapple Chunks", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Pineapple Chunks", loggedInUserId), ImagePath = "pineapplechunks.png" },
                    new Produce { Name = "Peach Slices", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Peach Slices", loggedInUserId), ImagePath = "peachslices.png" },
                    new Produce { Name = "Cherries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Cherries", loggedInUserId), ImagePath = "cherries.png" },



                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeFrozens();
            originalItems = new ObservableCollection<Produce>(frozens);
            fruitListView.ItemsSource = originalItems;
            Title = "Frozen Fruit";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var frozen = (Produce)button.BindingContext;

            frozen.Quantity++;
            if (!selectedFrozens.Contains(frozen))
            {
                selectedFrozens.Add(frozen);
            }


            // Update the database
            await UpdateDatabase(frozen, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var frozen = (Produce)button.BindingContext;

            frozen.Quantity--;

            if (frozen.Quantity < 0)
                frozen.Quantity = 0;

            await UpdateDatabase(frozen, -1);
        }

        private async Task UpdateDatabase(Produce frozen, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingFrozen = await dbContext.GetFrozenByNameAndUserId(frozen.Name, loggedInUserId);
                    if (existingFrozen != null)
                    {
                        existingFrozen.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingFrozen.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingFrozen);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingFrozen);
                        }
                    }
                    else
                    {
                        frozen.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(frozen);
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
            var frozen = (Produce)button.BindingContext;

            if (frozen.Quantity > 0)
            {
                frozen.Quantity = 0;
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = frozens;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedFrozens.Contains(frozen))
                {
                    selectedFrozens.Remove(frozen);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingFrozen = await dbContext.GetFrozenByNameAndUserId(frozen.Name, App.LoggedInUserId);
                if (existingFrozen != null)
                {
                    existingFrozen.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingFrozen);
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