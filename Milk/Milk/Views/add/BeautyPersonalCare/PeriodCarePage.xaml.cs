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

namespace Milk.Views.add.BeautyPersonalCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PeriodCarePage : ContentPage
    {
        private List<Produce> personals;
        private List<Produce> selectedPersonals; // To store selected fruits

        public int UserId { get; set; }


        public PeriodCarePage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedPersonals = new List<Produce>();
        }


        private async Task InitializePersonals()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            personals = new List<Produce>
                {
                    new Produce { Name = "Daily Panty Liners", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Daily Panty Liners", loggedInUserId), ImagePath = "pantyliners.png" },
                    new Produce { Name = "Menstrual Cup (Small)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Menstrual Cup (Small)", loggedInUserId), ImagePath = "menstrualcupsmall.png" },
                    new Produce { Name = "Menstrual Cup (Large)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Menstrual Cup (Large)", loggedInUserId), ImagePath = "menstrualcuplarge.png" },
                    new Produce { Name = "Organic Cotton Tampons (Regular)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Organic Cotton Tampons (Regular)", loggedInUserId), ImagePath = "tamponsregular.png" },
                    new Produce { Name = "Organic Cotton Tampons (Super)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Organic Cotton Tampons (Super)", loggedInUserId), ImagePath = "tamponssuper.png" },
                    new Produce { Name = "Sanitary Pads (Day Use)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Sanitary Pads (Day Use)", loggedInUserId), ImagePath = "padsday.png" },
                    new Produce { Name = "Sanitary Pads (Night Use)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Sanitary Pads (Night Use)", loggedInUserId), ImagePath = "padsnight.png" },
                    new Produce { Name = "Reusable Cloth Pads", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Reusable Cloth Pads", loggedInUserId), ImagePath = "clothpads.png" },
                    new Produce { Name = "Period Panties", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Period Panties", loggedInUserId), ImagePath = "periodpanties.png" }                     


                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializePersonals();
            originalItems = new ObservableCollection<Produce>(personals);
            fruitListView.ItemsSource = originalItems;
            Title = "Period Care";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var personal = (Produce)button.BindingContext;

            personal.Quantity++;
            if (!selectedPersonals.Contains(personal))
            {
                selectedPersonals.Add(personal);
            }


            // Update the database
            await UpdateDatabase(personal, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var personal = (Produce)button.BindingContext;

            personal.Quantity--;

            if (personal.Quantity < 0)
                personal.Quantity = 0;

            await UpdateDatabase(personal, -1);
        }

        private async Task UpdateDatabase(Produce personal, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingPersonal = await dbContext.GetPersonalByNameAndUserId(personal.Name, loggedInUserId);
                    if (existingPersonal != null)
                    {
                        existingPersonal.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingPersonal.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingPersonal);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingPersonal);
                        }
                    }
                    else
                    {
                        personal.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(personal);
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
            var personal = (Produce)button.BindingContext;

            if (personal.Quantity > 0)
            {
                personal.Quantity = 0;
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = personals;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedPersonals.Contains(personal))
                {
                    selectedPersonals.Remove(personal);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingPersonal = await dbContext.GetPersonalByNameAndUserId(personal.Name, App.LoggedInUserId);
                if (existingPersonal != null)
                {
                    existingPersonal.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingPersonal);
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