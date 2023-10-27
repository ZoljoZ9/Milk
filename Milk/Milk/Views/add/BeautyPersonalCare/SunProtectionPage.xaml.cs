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
    public partial class SunProtectionPage : ContentPage
    {
        private List<Produce> personals;
        private List<Produce> selectedPersonals; // To store selected fruits

        public int UserId { get; set; }


        public SunProtectionPage()
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
                    new Produce { Name = "Acne Spot Treatment", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Acne Spot Treatment", loggedInUserId), ImagePath = "acnespottreatment.png" },
                    new Produce { Name = "Anti-Aging Serum", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Anti-Aging Serum", loggedInUserId), ImagePath = "antiagingserum.png" },
                    new Produce { Name = "Daily Moisturizer (SPF 30)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Daily Moisturizer (SPF 30)", loggedInUserId), ImagePath = "dailymoisturizer.png" },
                    new Produce { Name = "Exfoliating Face Scrub", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Exfoliating Face Scrub", loggedInUserId), ImagePath = "facescrub.png" },
                    new Produce { Name = "Face Mask (Clay)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Face Mask (Clay)", loggedInUserId), ImagePath = "clayfacemask.png" },
                    new Produce { Name = "Hydrating Night Cream", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Hydrating Night Cream", loggedInUserId), ImagePath = "nightcream.png" },
                    new Produce { Name = "Lip Scrub (Vanilla)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Lip Scrub (Vanilla)", loggedInUserId), ImagePath = "lipscrub.png" },
                    new Produce { Name = "Micellar Water Makeup Remover", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Micellar Water Makeup Remover", loggedInUserId), ImagePath = "micellarwater.png" },
                    new Produce { Name = "Vitamin C Serum", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Vitamin C Serum", loggedInUserId), ImagePath = "vitamincserum.png" }

                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializePersonals();
            originalItems = new ObservableCollection<Produce>(personals);
            fruitListView.ItemsSource = originalItems;
            Title = "Sun Protection";

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