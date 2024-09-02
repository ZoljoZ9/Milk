using Milk.Data;
using Milk.Models;
using Milk.ViewModel;
using Milk.Views.add;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.ViewModels
{
    public class ListViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;  // Declare the private field

        public ObservableCollection<Grouping<string, Produce>> GroupedItems { get; private set; }
        public ObservableCollection<Produce> SelectedFruits { get; set; }
        public ObservableCollection<Produce> SelectedVegetables { get; set; }
        public ObservableCollection<Produce> SelectedBakeries { get; set; }
        public ObservableCollection<Produce> SelectedSalads { get; set; }
        public ObservableCollection<Produce> SelectedRedMeats { get; set; }
        public ObservableCollection<Produce> SelectedDelis { get; set; }
        public ObservableCollection<Produce> SelectedBakings { get; set; }
        public ObservableCollection<Produce> SelectedChips { get; set; }
        public ObservableCollection<Produce> SelectedFrozens { get; set; }
        public ObservableCollection<Produce> SelectedDrinks { get; set; }
        public ObservableCollection<Produce> SelectedCheeses { get; set; }
        public ObservableCollection<Produce> SelectedPersonals { get; set; }
        public ObservableCollection<Produce> SelectedCleanings { get; set; }
        public ObservableCollection<Produce> SelectedPets { get; set; }







        SQLiteAsyncConnection _dbAsyncConnection = new SQLiteAsyncConnection(App.DatabasePath);

        public class Grouping<K, T> : ObservableCollection<T>
        {
            public K Key { get; private set; }

            public Grouping(K key, IEnumerable<T> items)
            {
                Key = key;
                foreach (var item in items)
                    this.Items.Add(item);
            }
        }



        public ListViewModel(int userId)
        {
            _dbContext = new AppDbContext(App.DatabasePath);  // Initialize the _dbContext

            GroupedItems = new ObservableCollection<Grouping<string, Produce>>();
            SelectedFruits = new ObservableCollection<Produce>();
            SelectedVegetables = new ObservableCollection<Produce>();
            SelectedBakeries = new ObservableCollection<Produce>();
            SelectedSalads = new ObservableCollection<Produce>();
            SelectedRedMeats = new ObservableCollection<Produce>();
            SelectedDelis = new ObservableCollection<Produce>();
            SelectedCheeses = new ObservableCollection<Produce>();
            SelectedBakings = new ObservableCollection<Produce>();
            SelectedChips = new ObservableCollection<Produce>();
            SelectedFrozens = new ObservableCollection<Produce>();
            SelectedDrinks = new ObservableCollection<Produce>();
            SelectedPersonals = new ObservableCollection<Produce>();
            SelectedCleanings = new ObservableCollection<Produce>();
            SelectedPets = new ObservableCollection<Produce>();

          




            LoadSelectedItems(userId);


            MessagingCenter.Subscribe<object>(this, "UpdateList", (sender) => {
                LoadSelectedItems(userId);
            });
        }

        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>
        {
            "Fresh Produce",
            "Bakery Items",
            "Meat",
            "Deli",
            "Dairy, Eggs and Fridge",
            "Pantry",
            "Snacks",
            "Frozen Foods",
            "Beverages",
            "Beauty & Personal Care",
            "Pets",
            "Cleaning & Maintenance"
        };

        public async Task LoadSelectedItems(int userId)
        {
            // Load fruits for the user
            var fruits = await _dbContext.GetAllFruitForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of fruits returned: {fruits.Count}");
            SelectedFruits.Clear();
            foreach (var fruit in fruits)
            if (fruit.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedFruits.Add(fruit);
            }

            // Load vegetables for the user


            var bakeries = await _dbContext.GetAllBakeriesForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {bakeries.Count}");
            SelectedBakeries.Clear();
            foreach (var bakery in bakeries)
            if (bakery.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedBakeries.Add(bakery);
            }



            var redmeats = await _dbContext.GetAllRedMeatForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {redmeats.Count}");
            SelectedRedMeats.Clear();
            foreach (var redmeat in redmeats)
            if (redmeat.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedRedMeats.Add(redmeat);
            }

            var delis = await _dbContext.GetAllDelisForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {delis.Count}");
            SelectedDelis.Clear();
            foreach (var deli in delis)
            if (deli.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedDelis.Add(deli);
            }

            var cheeses = await _dbContext.GetAllCheesesForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {cheeses.Count}");
            SelectedCheeses.Clear();
            foreach (var cheese in cheeses)
            if (cheese.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedCheeses.Add(cheese);
            }

            var bakings = await _dbContext.GetAllBakingSupplyForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {bakings.Count}");
            SelectedBakings.Clear();
            foreach (var baking in bakings)
            if (baking.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedBakings.Add(baking);
            }

            var chips = await _dbContext.GetAllChipForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {chips.Count}");
            SelectedChips.Clear();
            foreach (var chip in chips)
            if (chip.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedChips.Add(chip);
            }

            var frozens = await _dbContext.GetAllFrozenForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {frozens.Count}");
            SelectedFrozens.Clear();
            foreach (var frozen in frozens)
            if (frozen.Quantity > 0)
            {
                SelectedFrozens.Add(frozen);
            }

            var drinks = await _dbContext.GetAllDrinkForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {drinks.Count}");
            SelectedDrinks.Clear();
            foreach (var drink in drinks)
            if (drink.Quantity > 0)
            {
                SelectedDrinks.Add(drink);
            }

            var personals = await _dbContext.GetAllPersonalForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {personals.Count}");
            SelectedPersonals.Clear();
            foreach (var personal in personals)
            if (personal.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedPersonals.Add(personal);
            }

            var cleanings = await _dbContext.GetAllCleaningForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {cleanings.Count}");
            SelectedCleanings.Clear();
            foreach (var cleaning in cleanings)
            if (cleaning.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedCleanings.Add(cleaning);
            }

            var pets = await _dbContext.GetAllPetForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {pets.Count}");
            SelectedPets.Clear();
            foreach (var pet in pets)
            if (pet.Quantity > 0)  // Only add items with a quantity greater than 0

            {
                    SelectedPets.Add(pet);
            }


            // Add fruits and vegetables to GroupedItems only if they have items
            GroupedItems.Clear();
                if (SelectedFruits.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Fresh Produce", SelectedFruits));
                if (SelectedBakeries.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Bakery Items", SelectedBakeries));
                if (SelectedRedMeats.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Meat", SelectedRedMeats));
                if (SelectedDelis.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Deli", SelectedDelis));
                if (SelectedCheeses.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Dairy, Eggs and Fridge", SelectedCheeses));
                if (SelectedBakings.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Pantry", SelectedBakings));
                if (SelectedChips.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Snacks", SelectedChips));
                if (SelectedFrozens.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Frozen Foods", SelectedFrozens));
                if (SelectedDrinks.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Beverages", SelectedDrinks));
                if (SelectedPersonals.Any()) // Only add the group if there are items
                    GroupedItems.Add(new Grouping<string, Produce>("Beauty & Personal Care", SelectedPersonals));
            if (SelectedPets.Any()) // Only add the group if there are items
                GroupedItems.Add(new Grouping<string, Produce>("Pets", SelectedPets));
            if (SelectedCleanings.Any()) // Only add the group if there are items
                GroupedItems.Add(new Grouping<string, Produce>("Cleaning & Maintenance", SelectedCleanings));
            // Notify UI of the change
            OnPropertyChanged(nameof(GroupedItems));


            
        }


        public void UpdateGroupedItems()
        {
            GroupedItems.Clear();

            foreach (var category in Categories)
            {
                IEnumerable<Produce> selectedItems = Enumerable.Empty<Produce>();

                switch (category)
                {
                    case "Fresh Produce":
                        selectedItems = SelectedFruits;
                        break;
                    case "Bakery Items":
                        selectedItems = SelectedBakeries;
                        break;
                    case "Meat":
                        selectedItems = SelectedRedMeats;
                        break;
                    // ... Add more cases for each category

                    default:
                        break;
                }

                if (selectedItems.Any())
                {
                    GroupedItems.Add(new Grouping<string, Produce>(category, selectedItems));
                }
                // If you want to add empty groups too, you would do it here
                // else
                // {
                //     GroupedItems.Add(new Grouping<string, Produce>(category, Enumerable.Empty<Produce>()));
                // }
            }

            // Notify UI of the change
            OnPropertyChanged(nameof(GroupedItems));
        }


        public ICommand DeleteCommand => new Command<Produce>(DeleteProduce);

        public async void DeleteProduce(Produce produce)
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var existingProduce = await (produce.Type == Produce.ProduceType.Fruit
                ? dbContext.GetFruitByNameAndUserId(produce.Name, App.LoggedInUserId)
                : dbContext.GetRedMeatByNameAndUserId(produce.Name, App.LoggedInUserId)); ;

            

            if (existingProduce != null)
            {
                var deleteResult = await dbContext.Database.DeleteAsync(existingProduce);
                if (deleteResult == 0)
                {
                    // Handle delete failure
                }
                else
                {
                    MessagingCenter.Send<object>(this, "UpdateList");
                }
            }
        }


        public ICommand ClearAllCommand => new Command(ClearAllFruits);

        private async void ClearAllFruits()
        {
            var group = GroupedItems.FirstOrDefault(g => g.Key == "Fruits");
            if (group != null)
            {
                foreach (var fruit in group)
                {
                    fruit.Quantity = 0;
                }

                // Clear the group
                GroupedItems.Remove(group);
            }

            // You may also want to update the database to reflect these changes.
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string SelectedCategory { get; set; }

    }
}



