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

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class deliPage : ContentPage
    {
        private List<Produce> delis;
        private List<Produce> selectedDelis; // To store selected
                                                // s

        public int UserId { get; set; }

        public deliPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedDelis = new List<Produce>();
        }




        private async Task InitializeDelis()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            delis = new List<Produce>
                {
                    new Produce { Name = "Antipasto", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Antipasto", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Chicken Salad", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Chicken Salad", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Chorizo", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Chorizo", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Coleslaw", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Coleslaw", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Corned Beef", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Corned Beef", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Feta Cheese", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Feta Cheese", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Hummus", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Hummus", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Mortadella", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Mortadella", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Olive Mix", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Olive Mix", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Pastrami", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Pastrami", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Pepperoni", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Pepperoni", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Prosciutto", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Prosciutto", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Roast Beef", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Roast Beef", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Salami", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Salami", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Smoked Salmon", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Smoked Salmon", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Swiss Cheese", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Swiss Cheese", loggedInUserId), ImagePath = "deli.jpg" },
                    new Produce { Name = "Turkey Breast", Type = ProduceType.Deli, Quantity = await dbContext.GetDelisQuantityByNameAndUserId("Turkey Breast", loggedInUserId), ImagePath = "deli.jpg" }

                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeDelis();
            originalItems = new ObservableCollection<Produce>(delis);
            DeliListView.ItemsSource = originalItems;
            Title = "Deli";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var deli = (Produce)button.BindingContext;

                deli.Quantity++;
                if (!selectedDelis.Contains(deli))
                {
                    selectedDelis.Add(deli);
                }

                await UpdateDatabase(deli, 1);
            }
            catch (Exception ex)
            {
                // Log the error or display a message
                Console.WriteLine(ex.Message);
            }
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var deli = (Produce)button.BindingContext;

            deli.Quantity--;

            if (deli.Quantity < 0)
                deli.Quantity = 0;

            await UpdateDatabase(deli, -1);
        }

        private async Task UpdateDatabase(Produce deli, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingDeli = await dbContext.GetBakeriesByNameAndUserId(deli.Name, loggedInUserId);
                    if (existingDeli != null)
                    {
                        existingDeli.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingDeli.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingDeli);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingDeli);
                        }
                    }
                    else
                    {
                        deli.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(deli);
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
            var deli = (Produce)button.BindingContext;

            if (deli.Quantity > 0)
            {
                deli.Quantity = 0;
                DeliListView.ItemsSource = null;  // Reset the ItemsSource
                DeliListView.ItemsSource = delis;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedDelis.Contains(deli))
                {
                    selectedDelis.Remove(deli);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingBakery = await dbContext.GetBakeriesByNameAndUserId(deli.Name, App.LoggedInUserId);
                if (existingBakery != null)
                {
                    existingBakery.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingBakery);
                }
            }
        }
        private ObservableCollection<Produce> originalItems;


        void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = searchBar.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                DeliListView.ItemsSource = originalItems;
            }
            else
            {
                DeliListView.ItemsSource = originalItems.Where(item => item.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
        }






    }
}