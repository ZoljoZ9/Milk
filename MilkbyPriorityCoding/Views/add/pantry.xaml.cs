using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls.Xaml;
using Milk.Views.add.MeatsPoloutrySeafood;
using Milk.Views.add.Pantryfood;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pantry : ContentPage
    {
        public ObservableCollection<PantryPicture> Pictures { get; set; }
        public PantryPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public pantry()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<PantryPicture>
            {
                new PantryPicture { Source = "baking.jpg", ImageText = "Baking", TargetPageType = typeof(BakingPage) },
                new PantryPicture { Source = "cereal.jpg", ImageText = "Breakfast & Spread", TargetPageType = typeof(BreakfastSpreadPage) },
                new PantryPicture { Source = "canned.jpg", ImageText = "Canned food & Instant Meals", TargetPageType = typeof(CannedFoodInstandMealsPage) },
                new PantryPicture { Source = "sauce.jpg", ImageText = "Condiments", TargetPageType = typeof(CondimentsPage) },
                new PantryPicture { Source = "pastasauce.jpg", ImageText = "Cooking Sauces", TargetPageType = typeof(CookingSaucesPage) },
                new PantryPicture { Source = "nuts.jpg", ImageText = "Nuts & Dried Fruit", TargetPageType = typeof(NutsDriedFruit) },
                new PantryPicture { Source = "herbs.jpg", ImageText = "Herbs & Spices", TargetPageType = typeof(HerbsSpicesPage) },
                new PantryPicture { Source = "international.jpg", ImageText = "International Foods", TargetPageType = typeof(InternationalFoodsPage) },
                new PantryPicture { Source = "milk1.jpg", ImageText = "Longlife Milk", TargetPageType = typeof(LongLifeMilkPage) },
                new PantryPicture { Source = "oil.jpg", ImageText = "Oil & Vinegar", TargetPageType = typeof(OilVinegarPage) },
                new PantryPicture { Source = "pasta.jpg", ImageText = "Pasta, Rice & Grains", TargetPageType = typeof(PastaRiceGrainsPage) },
                new PantryPicture { Source = "coffee.jpg", ImageText = "Tea & Coffee", TargetPageType = typeof(TeaCoffeePage) },



            };
            RowTappedCommand = new Command<PantryPicture>(async (picture) =>
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
            Title = "Pantry";
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is PantryPicture picture)
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
            }
        }
    }

    public class PantryPicture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }

    }
}