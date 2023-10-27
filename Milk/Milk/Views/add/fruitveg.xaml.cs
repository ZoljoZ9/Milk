using Milk.Views.add;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class fruitveg : ContentPage

    {
        public ObservableCollection<FruitVegPicture> Pictures { get; set; }
        public FruitVegPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public fruitveg()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<FruitVegPicture>
            {
                new FruitVegPicture { Source = "apple.png", ImageText = "Fruit", TargetPageType = typeof(fruit) },
                new FruitVegPicture { Source = "broccoli.png", ImageText = "Vegetables", TargetPageType = typeof(vegetablePage) },
                new FruitVegPicture { Source = "salad.png", ImageText = "Salads", TargetPageType = typeof(SaladPage) },


            };
            RowTappedCommand = new Command<FruitVegPicture>(async (picture) =>
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
            Title = "Fruit & Veg";
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is FruitVegPicture picture)
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
            }
        }
    }

    public class FruitVegPicture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }

    }
    
}