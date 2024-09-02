﻿using Milk.Data;
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

namespace Milk.Views.add.FrozenFood
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrozenMealsPage : ContentPage
    {
        private List<Produce> frozens;
        private List<Produce> selectedFrozens; // To store selected fruits

        public int UserId { get; set; }


        public FrozenMealsPage()
        {
            InitializeComponent();

            // Initialize selectedFruits list
            selectedFrozens = new List<Produce>();
        }


        private async Task InitializeFrozens()
        {
            var dbContext = new AppDbContext(App.DatabasePath);
            var loggedInUserId = App.LoggedInUserId;

            frozens = new List<Produce>
                {
                        new Produce { Name = "Chicken Alfredo", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Chicken Alfredo", loggedInUserId), ImagePath = "chickenalfredo.png" },
                        new Produce { Name = "Beef Stroganoff", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Beef Stroganoff", loggedInUserId), ImagePath = "beefstroganoff.png" },
                        new Produce { Name = "Vegetarian Lasagna", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Vegetarian Lasagna", loggedInUserId), ImagePath = "vegetarianlasagna.png" },
                        new Produce { Name = "Shrimp Scampi", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Shrimp Scampi", loggedInUserId), ImagePath = "shrimpscampi.png" },
                        new Produce { Name = "Chicken Parmesan", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Chicken Parmesan", loggedInUserId), ImagePath = "chickenparmesan.png" },
                        new Produce { Name = "Spaghetti Bolognese", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Spaghetti Bolognese", loggedInUserId), ImagePath = "spaghettibolognese.png" },
                        new Produce { Name = "Butter Chicken", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Butter Chicken", loggedInUserId), ImagePath = "butterchicken.png" },
                        new Produce { Name = "Vegetable Stir-Fry", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Vegetable Stir-Fry", loggedInUserId), ImagePath = "vegetablestirfry.png" },




                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeFrozens();
            originalItems = new ObservableCollection<Produce>(frozens);
            fruitListView.ItemsSource = originalItems;
            Title = "Frozen Meals";

        }


        private async void PlusButton_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var frozen = (Produce)button.BindingContext;

            frozen.Quantity++;
            if (!selectedFrozens.Contains(frozen))
            {
                selectedFrozens.Add(frozen);
            }


            // Update the database
            await UpdateDatabase(frozen, 1);
        }

        private async void MinusButton_Clicked(object sender, EventArgs e)
        {
            // This is similar to what you have in the fruit page
            var button = (Button)sender;
            var frozen = (Produce)button.BindingContext;

            frozen.Quantity--;

            if (frozen.Quantity < 0)
                frozen.Quantity = 0;

            await UpdateDatabase(frozen, -1);
        }

        private async Task UpdateDatabase(Produce frozen, int adjustment)
        {
            try
            {
                using (var dbContext = new AppDbContext(App.DatabasePath))
                {
                    var loggedInUserId = App.LoggedInUserId;

                    var existingFrozen = await dbContext.GetFrozenByNameAndUserId(frozen.Name, loggedInUserId);
                    if (existingFrozen != null)
                    {
                        existingFrozen.Quantity += adjustment;

                        // Check if Quantity is zero or less to delete item
                        if (existingFrozen.Quantity <= 0)
                        {
                            await dbContext.Database.DeleteAsync(existingFrozen);
                        }
                        else
                        {
                            await dbContext.Database.UpdateAsync(existingFrozen);
                        }
                    }
                    else
                    {
                        frozen.UserId = loggedInUserId;
                        await dbContext.Database.InsertAsync(frozen);
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
            var frozen = (Produce)button.BindingContext;

            if (frozen.Quantity > 0)
            {
                frozen.Quantity = 0;
                fruitListView.ItemsSource = null;  // Reset the ItemsSource
                fruitListView.ItemsSource = frozens;  // Set it back

                // Remove the fruit from selectedFruits if present
                if (selectedFrozens.Contains(frozen))
                {
                    selectedFrozens.Remove(frozen);
                }

                // Update the database to set quantity to 0 (or delete it, depending on your needs)
                var dbContext = new AppDbContext(App.DatabasePath);
                var existingFrozen = await dbContext.GetFrozenByNameAndUserId(frozen.Name, App.LoggedInUserId);
                if (existingFrozen != null)
                {
                    existingFrozen.Quantity = 0;
                    await dbContext.Database.UpdateAsync(existingFrozen);
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