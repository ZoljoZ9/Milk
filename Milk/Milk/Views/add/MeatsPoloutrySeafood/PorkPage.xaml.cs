using Milk.Data;
using Milk;
using static Produce;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Milk.Models;
using SQLite;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Milk.Views.add;
using Milk.Views.add.MeatsPoloutrySeafood;

namespace Milk.Views.add.MeatsPoloutrySeafood
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PorkPage : ContentPage
    {
        private List<Produce> redmeats;
        private List<Produce> selectedRedMeats; // To store selected fruits

        public int UserId { get; set; }


        public PorkPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedRedMeats = new List<Produce>();
        }


        private async Task InitializeRedMeats()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            redmeats = new List<Produce>
                {
                    new Produce { Name = "Bacon", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Bacon", loggedInUserId), ImagePath = "bacon.png" },
                    new Produce { Name = "Chorizo sausage", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Chorizo sausage", loggedInUserId), ImagePath = "chorizo_sausage.png" },
                    new Produce { Name = "Ham", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Ham", loggedInUserId), ImagePath = "ham.png" },
                    new Produce { Name = "Pork belly", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Pork belly", loggedInUserId), ImagePath = "pork_belly.png" },
                    new Produce { Name = "Pork chops", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Pork chops", loggedInUserId), ImagePath = "pork_chops.png" },
                    new Produce { Name = "Pork loin roast", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Pork loin roast", loggedInUserId), ImagePath = "pork_loin_roast.png" },
                    new Produce { Name = "Pork mince", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Pork mince", loggedInUserId), ImagePath = "pork_mince.png" },
                    new Produce { Name = "Pork ribs", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Pork ribs", loggedInUserId), ImagePath = "pork_ribs.png" },
                    new Produce { Name = "Pork tenderloin", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Pork tenderloin", loggedInUserId), ImagePath = "pork_tenderloin.png" },
                    new Produce { Name = "Sausages", Type = ProduceType.RedMeat, Quantity = await dbContext.GetRedMeatQuantityByNameAndUserId("Sausages", loggedInUserId), ImagePath = "pork_sausages.png" }


                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeRedMeats();
            originalItems = new ObservableCollection<Produce>(redmeats);
            RedMeatListView.ItemsSource = originalItems;
            Title = "Pork";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var redmeat = (Produce)button.BindingContext;

            redmeat.Quantity++;
            if (!selectedRedMeats.Contains(redmeat))
            {
                selectedRedMeats.Add(redmeat);
            }


            // Update the database
            await UpdateDatabase(redmeat, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var redmeat = (Produce)button.BindingContext;

            redmeat.Quantity--;

            if (redmeat.Quantity < 0)
                redmeat.Quantity = 0;

            await UpdateDatabase(redmeat, -1);
        }

        private async Task UpdateDatabase(Produce redmeat, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingRedMeats = await dbContext.GetRedMeatByNameAndUserId(redmeat.Name, loggedInUserId);
                    if (existingRedMeats != null)
                    {
                        existingRedMeats.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingRedMeats.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingRedMeats);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingRedMeats);
                        }
                    }
                    else
                    {
                        redmeat.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(redmeat);
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
            var redmeat = (Produce)button.BindingContext;

            if (redmeat.Quantity > 0)
            {
                redmeat.Quantity = 0;
                RedMeatListView.ItemsSource = null;  // Reset the ItemsSource
                RedMeatListView.ItemsSource = redmeats;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedRedMeats.Contains(redmeat))
                {
                    selectedRedMeats.Remove(redmeat);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingRedMeat = await dbContext.GetRedMeatByNameAndUserId(redmeat.Name, App.LoggedInUserId);
                if (existingRedMeat != null)
                {
                    existingRedMeat.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingRedMeat);
                }
            }
        }
        private ObservableCollection<Produce> originalItems;


        void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchTerm = searchBar.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                RedMeatListView.ItemsSource = originalItems;
            }
            else
            {
                RedMeatListView.ItemsSource = originalItems.Where(item => item.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
        }
    }
}