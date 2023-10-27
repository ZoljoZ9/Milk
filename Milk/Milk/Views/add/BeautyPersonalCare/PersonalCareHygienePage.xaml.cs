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
    public partial class PersonalCareHygienePage : ContentPage
    {
        private List<Produce> personals;
        private List<Produce> selectedPersonals; // To store selected fruits

        public int UserId { get; set; }


        public PersonalCareHygienePage()
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
                    new Produce { Name = "Antibacterial Hand Gel", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Antibacterial Hand Gel", loggedInUserId), ImagePath = "handgel.png" },
                    new Produce { Name = "Bath Soap (Lavender)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Bath Soap (Lavender)", loggedInUserId), ImagePath = "lavendersoap.png" },
                    new Produce { Name = "Body Wash (Citrus)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Body Wash (Citrus)", loggedInUserId), ImagePath = "citrusbodywash.png" },
                    new Produce { Name = "Cotton Buds", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Cotton Buds", loggedInUserId), ImagePath = "cottonbuds.png" },
                    new Produce { Name = "Cotton Pads", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Cotton Pads", loggedInUserId), ImagePath = "cottonpads.png" },
                    new Produce { Name = "Face Wash (Exfoliating)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Face Wash (Exfoliating)", loggedInUserId), ImagePath = "exfoliatingfacewash.png" },
                    new Produce { Name = "Facial Tissue", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Facial Tissue", loggedInUserId), ImagePath = "facialtissue.png" },
                    new Produce { Name = "Foot Powder", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Foot Powder", loggedInUserId), ImagePath = "footpowder.png" },
                    new Produce { Name = "Hand Cream", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Hand Cream", loggedInUserId), ImagePath = "handcream.png" },
                    new Produce { Name = "Lip Balm", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Lip Balm", loggedInUserId), ImagePath = "lipbalm.png" },
                    new Produce { Name = "Toothpaste (Whitening)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Toothpaste (Whitening)", loggedInUserId), ImagePath = "whiteningtoothpaste.png" },
                    new Produce { Name = "Wet Wipes", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Wet Wipes", loggedInUserId), ImagePath = "wetwipes.png" }                                    


                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializePersonals();
            originalItems = new ObservableCollection<Produce>(personals);
            fruitListView.ItemsSource = originalItems;
            Title = "Personal Care & Hygiene";

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