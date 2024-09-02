using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls.Xaml;
using Milk.Views.add.MeatsPoloutrySeafood;
using Milk.Views.add.CatDog;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pet : ContentPage
    {
        public ObservableCollection<PETPicture> Pictures { get; set; }
        public PETPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public pet()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<PETPicture>
            {
                new PETPicture { Source = "birdfood.jpg", ImageText = "Bird, Fish & Smaill Pets", TargetPageType = typeof(BirdFishSmallPets) },
                new PETPicture { Source = "catfood.jpg", ImageText = "Cat & Kitten", TargetPageType = typeof(CatKittenPage) },
                new PETPicture { Source = "dogfood.jpg", ImageText = "Dog & Puppy", TargetPageType = typeof(DogPuppyPage) },


            };
            RowTappedCommand = new Command<PETPicture>(async (picture) =>
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
            Title = "Pet";
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is PETPicture picture)
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
            }
        }
    }

    public class PETPicture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }

    }
}
