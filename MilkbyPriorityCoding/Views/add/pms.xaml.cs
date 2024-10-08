﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls.Xaml;
using Milk.Views.add.MeatsPoloutrySeafood;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;


namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pms : ContentPage
    {
        public ObservableCollection<PMSPicture> Pictures { get; set; }
        public PMSPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public pms()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<PMSPicture>
            {
                new PMSPicture { Source = "poultry.jpg", ImageText = "Poultry", TargetPageType = typeof(PoultryPage) },
                new PMSPicture { Source = "beef.jpg", ImageText = "Beef", TargetPageType = typeof(RedMeatPage) },
                new PMSPicture { Source = "lamb.jpg", ImageText = "Lamb", TargetPageType = typeof(LambPage) },
                new PMSPicture { Source = "pork.jpg", ImageText = "Pork", TargetPageType = typeof(PorkPage) },
                new PMSPicture { Source = "seafood.jpg", ImageText = "Seafood", TargetPageType = typeof(SeafoodPage) },


            };
            RowTappedCommand = new Command<PMSPicture>(async (picture) =>
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
                Console.WriteLine("Navigated to: " + picture.TargetPageType.Name); // Add this line for debugging
            });
            this.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = "Meat & Seafood";
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is PMSPicture picture)
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
            }
        }
    }

    public class PMSPicture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }

    }
}
