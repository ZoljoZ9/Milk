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
    public partial class FrozenPartyFoodPage : ContentPage
    {
        private List<Produce> frozens;
        private List<Produce> selectedFrozens; // To store selected fruits

        public int UserId { get; set; }


        public FrozenPartyFoodPage()
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
                        new Produce { Name = "Mozzarella Sticks", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Mozzarella Sticks", loggedInUserId), ImagePath = "mozzarellasticks.png" },
                        new Produce { Name = "Spring Rolls", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Spring Rolls", loggedInUserId), ImagePath = "springrolls.png" },
                        new Produce { Name = "Mini Quiches", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Mini Quiches", loggedInUserId), ImagePath = "miniquiches.png" },
                        new Produce { Name = "Potato Skins", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Potato Skins", loggedInUserId), ImagePath = "potatoskins.png" },
                        new Produce { Name = "Chicken Wings", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Chicken Wings", loggedInUserId), ImagePath = "chickenwings.png" },
                        new Produce { Name = "Pigs in a Blanket", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Pigs in a Blanket", loggedInUserId), ImagePath = "pigsinablanket.png" },
                        new Produce { Name = "Spinach and Artichoke Dip", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Spinach and Artichoke Dip", loggedInUserId), ImagePath = "spinachartichokedip.png" },
                        new Produce { Name = "Chicken Satay Skewers", Type = ProduceType.Frozen, Quantity = await dbContext.GetFrozenQuantityByNameAndUserId("Chicken Satay Skewers", loggedInUserId), ImagePath = "chickensatay.png" },






                    // ... Continue for other fruits
                };

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeFrozens();
            originalItems = new ObservableCollection<Produce>(frozens);
            fruitListView.ItemsSource = originalItems;
            Title = "Frozen Party Food";

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