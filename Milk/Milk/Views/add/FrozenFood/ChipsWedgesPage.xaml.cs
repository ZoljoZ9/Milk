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
    public partial class ChipsWedgesPage : ContentPage
    {
        private List<Produce> frozens;
        private List<Produce> selectedFrozens; // To store selected fruits

        public int UserId { get; set; }


        public ChipsWedgesPage()
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
                    new Produce { Name = "Straight Cut Fries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Straight Cut Fries", loggedInUserId), ImagePath = "straightcutfries.png" },
                    new Produce { Name = "Crinkle Cut Fries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Crinkle Cut Fries", loggedInUserId), ImagePath = "crinklecutfries.png" },
                    new Produce { Name = "Shoestring Fries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Shoestring Fries", loggedInUserId), ImagePath = "shoestringfries.png" },
                    new Produce { Name = "Steak Fries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Steak Fries", loggedInUserId), ImagePath = "steakfries.png" },
                    new Produce { Name = "Sweet Potato Fries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Sweet Potato Fries", loggedInUserId), ImagePath = "sweetpotatofries.png" },
                    new Produce { Name = "Waffle Fries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Waffle Fries", loggedInUserId), ImagePath = "wafflefries.png" },
                    new Produce { Name = "Seasoned Fries", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Seasoned Fries", loggedInUserId), ImagePath = "seasonedfries.png" },
                    new Produce { Name = "Potato Wedges", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Potato Wedges", loggedInUserId), ImagePath = "potatowedges.png" }

                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeFrozens();
            originalItems = new ObservableCollection<Produce>(frozens);
            fruitListView.ItemsSource = originalItems;
            Title = "Chips & Wedges";

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