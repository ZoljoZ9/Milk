using Milk.Data;
using Milk.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using static Produce;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views
{
    public partial class ManualAdd : ContentPage
    {
        private int _quantity = 0;

        public ManualAdd()
        {
            InitializeComponent();
            UpdateQuantity(); // Initialize the quantity's display.

            // Setting the BindingContext for this page.
            BindingContext = new ListViewModel(App.LoggedInUserId);

            // Only set the ItemSource once, to the keys of the CategoryMappings dictionary.
            categoryPicker.ItemsSource = CategoryMappings.Keys.ToList();
            var myPicker = new Picker
            {
                TitleColor = Colors.Black
            };
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void MinusButton_Clicked(object sender, EventArgs e)
        {
            if (_quantity > 0)
            {
                _quantity--;
                UpdateQuantity();
            }
        }
        public Dictionary<string, ProduceType> CategoryMappings = new Dictionary<string, ProduceType>
        {
            { "Fresh Produce", ProduceType.Fruit },
            { "Bakery Items", ProduceType.Bakery },
            { "Meat", ProduceType.RedMeat },
            { "Deli", ProduceType.Deli },
            { "Dairy, Eggs and Fridge", ProduceType.Cheese },
            { "Pantry", ProduceType.Baking },
            { "Snacks", ProduceType.Chip },
            { "Frozen Foods", ProduceType.Frozen },
            { "Beverages", ProduceType.Drink },
            { "Beauty & Personal Care", ProduceType.Personal },
            { "Pets", ProduceType.Pet },
            { "Cleaning & Maintenance", ProduceType.Cleaning },


            // ... add all other mappings here
        };

        private void PlusButton_Clicked(object sender, EventArgs e)
        {
            _quantity++;
            UpdateQuantity();
        }

        private void UpdateQuantity()
        {
            _quantityEntry.Text = _quantity.ToString();
        }

        private async void OnUpdateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(itemNameEntry.Text))
            {
                await DisplayAlert("Error", "Item name cannot be empty.", "OK");
                return;
            }

            if (_quantity <= 0)
            {
                await DisplayAlert("Error", "Quantity must be a positive number.", "OK");
                return;
            }

            var selectedCategoryName = (string)categoryPicker.SelectedItem;
            if (string.IsNullOrWhiteSpace(selectedCategoryName))
            {
                await DisplayAlert("Error", "Please select a category.", "OK");
                return;
            }

            // Fetch the corresponding ProduceType from the dictionary
            ProduceType selectedType = CategoryMappings[selectedCategoryName];

            var newItem = new Produce
            {
                Name = itemNameEntry.Text,
                Type = selectedType,
                Quantity = _quantity,
                UserId = App.LoggedInUserId
            };

            // Attempt to insert the newItem into the database
            try
            {
                var dbContext = new AppDbContext(App.DatabasePath);
                await dbContext.Database.InsertAsync(newItem);

                MessagingCenter.Send<object>(this, "UpdateList"); // Inform other parts of your app about the new item

                await DisplayAlert("Success", $"{newItem.Name} has been added to your shopping list.", "OK");

                itemNameEntry.Text = string.Empty;
                _quantity = 0;
                UpdateQuantity();
                categoryPicker.SelectedItem = null;
            }
            catch (Exception ex)
            {
                // Handle any exceptions during the database operation
                await DisplayAlert("Error", $"Failed to add item. Error: {ex.Message}", "OK");
            }
        }


        void OnCategorySelected(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategoryName = (string)picker.SelectedItem;

            if (CategoryMappings.TryGetValue(selectedCategoryName, out ProduceType selectedType))
            {
                // Do something with the selectedType
            }
        }

    }
}
