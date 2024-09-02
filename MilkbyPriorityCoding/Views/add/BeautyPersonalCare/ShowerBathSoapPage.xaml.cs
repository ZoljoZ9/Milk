using Milk.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using static Produce;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add.BeautyPersonalCare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowerBathSoapPage : ContentPage
    {
        private List<Produce> personals;
        private List<Produce> selectedPersonals; // To store selected fruits

        public int UserId { get; set; }


        public ShowerBathSoapPage()
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
                    new Produce { Name = "Bath Bomb (Lavender)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Bath Bomb (Lavender)", loggedInUserId), ImagePath = "lavenderbathbomb.png" },
                    new Produce { Name = "Bath Oil (Eucalyptus)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Bath Oil (Eucalyptus)", loggedInUserId), ImagePath = "eucalyptusbathoil.png" },
                    new Produce { Name = "Bath Salts (Rose)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Bath Salts (Rose)", loggedInUserId), ImagePath = "rosebathsalts.png" },
                    new Produce { Name = "Body Scrub (Coconut)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Body Scrub (Coconut)", loggedInUserId), ImagePath = "coconutbodyscrub.png" },
                    new Produce { Name = "Body Wash (Mango)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Body Wash (Mango)", loggedInUserId), ImagePath = "mangobodywash.png" },
                    new Produce { Name = "Hand Soap (Aloe Vera)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Hand Soap (Aloe Vera)", loggedInUserId), ImagePath = "aloeverahandsoap.png" },
                    new Produce { Name = "Shampoo Bar (Herbal)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Shampoo Bar (Herbal)", loggedInUserId), ImagePath = "herbalshampoobar.png" },
                    new Produce { Name = "Shower Gel (Citrus)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Shower Gel (Citrus)", loggedInUserId), ImagePath = "citrusshowergel.png" },
                    new Produce { Name = "Soap Bar (Oatmeal)", Type = ProduceType.Personal, Quantity = await dbContext.GetPersonalQuantityByNameAndUserId("Soap Bar (Oatmeal)", loggedInUserId), ImagePath = "oatmealsoapbar.png" }

                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializePersonals();
            originalItems = new ObservableCollection<Produce>(personals);
            fruitListView.ItemsSource = originalItems;
            Title = "Shower, Bath & Soap";

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