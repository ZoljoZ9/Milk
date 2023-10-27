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

namespace Milk.Views.add.CatDog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BirdFishSmallPets : ContentPage
    {
        private List<Produce> pets;
        private List<Produce> selectedPets; // To store selected fruits

        public int UserId { get; set; }


        public BirdFishSmallPets()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedPets = new List<Produce>();
        }


        private async Task InitializePets()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            pets = new List<Produce>
                {

                    new Produce { Name = "Bird Seed Mix", Type = ProduceType.Pet, Quantity = await dbContext.GetPetQuantityByNameAndUserId("Bird Seed Mix", loggedInUserId), ImagePath = "birdseed.png" },
                    new Produce { Name = "Bird Toys", Type = ProduceType.Pet, Quantity = await dbContext.GetPetQuantityByNameAndUserId("Bird Toys", loggedInUserId), ImagePath = "birdtoys.png" },
                    new Produce { Name = "Fish Tank Filters", Type = ProduceType.Pet, Quantity = await dbContext.GetPetQuantityByNameAndUserId("Fish Tank Filters", loggedInUserId), ImagePath = "fishtankfilter.png" },
                    new Produce { Name = "Fish Food (Flakes)", Type = ProduceType.Pet, Quantity = await dbContext.GetPetQuantityByNameAndUserId("Fish Food (Flakes)", loggedInUserId), ImagePath = "fishfoodflakes.png" },
                    new Produce { Name = "Hamster Wheel", Type = ProduceType.Pet, Quantity = await dbContext.GetPetQuantityByNameAndUserId("Hamster Wheel", loggedInUserId), ImagePath = "hamsterwheel.png" },
                    new Produce { Name = "Rabbit Pellets", Type = ProduceType.Pet, Quantity = await dbContext.GetPetQuantityByNameAndUserId("Rabbit Pellets", loggedInUserId), ImagePath = "rabbitpellets.png" },
                    new Produce { Name = "Small Animal Bedding", Type = ProduceType.Pet, Quantity = await dbContext.GetPetQuantityByNameAndUserId("Small Animal Bedding", loggedInUserId), ImagePath = "smallanimalbedding.png" },
                    new Produce { Name = "Water Conditioner for Fish", Type = ProduceType.Pet, Quantity = await dbContext.GetPetQuantityByNameAndUserId("Water Conditioner for Fish", loggedInUserId), ImagePath = "waterconditioner.png" }





            // ... Continue for other fruits
        };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializePets();
            originalItems = new ObservableCollection<Produce>(pets);
            fruitListView.ItemsSource = originalItems;
            Title = "Bird, Fish, & Small Pets";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var pet = (Produce)button.BindingContext;

            pet.Quantity++;
            if (!selectedPets.Contains(pet))
            {
                selectedPets.Add(pet);
            }


            // Update the database
            await UpdateDatabase(pet, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var pet = (Produce)button.BindingContext;

            pet.Quantity--;

            if (pet.Quantity < 0)
                pet.Quantity = 0;

            await UpdateDatabase(pet, -1);
        }

        private async Task UpdateDatabase(Produce pet, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingPet = await dbContext.GetPetByNameAndUserId(pet.Name, loggedInUserId);
                    if (existingPet != null)
                    {
                        existingPet.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingPet.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingPet);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingPet);
                        }
                    }
                    else
                    {
                        pet.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(pet);
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
            var pet = (Produce)button.BindingContext;

            if (pet.Quantity > 0)
            {
                pet.Quantity = 0;
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = pets;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedPets.Contains(pet))
                {
                    selectedPets.Remove(pet);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingPet = await dbContext.GetPersonalByNameAndUserId(pet.Name, App.LoggedInUserId);
                if (existingPet != null)
                {
                    existingPet.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingPet);
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