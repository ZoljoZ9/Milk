using Milk.Data;
using Milk.Models;
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
using Xamarin.Forms;

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




            LoadSelectedItems(userId);


            MessagingCenter.Subscribe<object>(this, "UpdateList", (sender) => {
                LoadSelectedItems(userId);
            });
        }



        public async Task LoadSelectedItems(int userId)
        {
            // Load fruits for the user
            var fruits = await _dbContext.GetAllFruitForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of fruits returned: {fruits.Count}");
            SelectedFruits.Clear();
            foreach (var fruit in fruits)
            {
                SelectedFruits.Add(fruit);
            }

            // Load vegetables for the user
            var vegetables = await _dbContext.GetAllVegetablesForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of vegetables returned: {vegetables.Count}");
            SelectedVegetables.Clear();
            foreach (var vegetable in vegetables)
            {
                SelectedVegetables.Add(vegetable);
            }

            var bakeries = await _dbContext.GetAllBakeriesForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {bakeries.Count}");
            SelectedBakeries.Clear();
            foreach (var bakery in bakeries)
            {
                SelectedBakeries.Add(bakery);
            }

            var salads = await _dbContext.GetAllSaladForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {salads.Count}");
            SelectedSalads.Clear();
            foreach (var salad in salads)
            {
                SelectedBakeries.Add(salad);
            }

            var redmeats = await _dbContext.GetAllRedMeatForUser(userId);
            System.Diagnostics.Debug.WriteLine($"Number of bakeries returned: {redmeats.Count}");
            SelectedRedMeats.Clear();
            foreach (var redmeat in redmeats)
            {
                SelectedRedMeats.Add(redmeat);
            }

            // Add fruits and vegetables to GroupedItems only if they have items
            GroupedItems.Clear();
            if (SelectedFruits.Any()) // Only add the group if there are items
                GroupedItems.Add(new Grouping<string, Produce>("Fruits", SelectedFruits));

            if (SelectedVegetables.Any()) // Only add the group if there are items
                GroupedItems.Add(new Grouping<string, Produce>("Vegetables", SelectedVegetables));

            if (SelectedBakeries.Any()) // Only add the group if there are items
                GroupedItems.Add(new Grouping<string, Produce>("Bakery Items", SelectedBakeries));

            if (SelectedSalads.Any()) // Only add the group if there are items
                GroupedItems.Add(new Grouping<string, Produce>("Salads", SelectedSalads));

            if (SelectedRedMeats.Any()) // Only add the group if there are items
                GroupedItems.Add(new Grouping<string, Produce>("Meat", SelectedRedMeats));
            // Notify UI of the change
            OnPropertyChanged(nameof(GroupedItems));
        }






        public ICommand DeleteCommand => new Command<Produce>(DeleteProduce);

        public async void DeleteProduce(Produce produce)
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var existingProduce = await (produce.Type == Produce.ProduceType.Fruit
                ? dbContext.GetFruitByNameAndUserId(produce.Name, App.LoggedInUserId)
                : dbContext.GetVegetableByNameAndUserId(produce.Name, App.LoggedInUserId));

            

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
    }
}



