using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls.Xaml;
using Milk.Views.add.MeatsPoloutrySeafood;
using Milk.Views.add.FrozenFood;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class freezer : ContentPage
    {
        public ObservableCollection<FRZPicture> Pictures { get; set; }
        public FRZPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public freezer()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<FRZPicture>
            {
                new FRZPicture { Source = "chipsfroz.jpg", ImageText = "Chips & Wedges", TargetPageType = typeof(ChipsWedgesPage) },
                new FRZPicture { Source = "frozen.jpg", ImageText = "Frozen Meals", TargetPageType = typeof(FrozenMealsPage) },
                new FRZPicture { Source = "frozenmeat.jpg", ImageText = "Frozen Meat", TargetPageType = typeof(FrozenMeatPage) },
                new FRZPicture { Source = "frozen.jpg", ImageText = "Frozen Party Food", TargetPageType = typeof(FrozenPartyFoodPage) },
                new FRZPicture { Source = "frozen.jpg", ImageText = "Frozen Pies & Sausage Rolls", TargetPageType = typeof(FrozenPiesSausageRollsPage) },
                new FRZPicture { Source = "frozendesert.jpg", ImageText = "Frozen Desert", TargetPageType = typeof(FrozenDesertPage) },
                new FRZPicture { Source = "frozenfruit.jpg", ImageText = "Frozen Fruit", TargetPageType = typeof(FrozenFruitPage) },
                new FRZPicture { Source = "pizza.jpg", ImageText = "Frozen Pizza", TargetPageType = typeof(FrozenPizzaPage) },
                new FRZPicture { Source = "seafood.jpg", ImageText = "Frozen Seafood", TargetPageType = typeof(FrozenSeafoodPage) },
                new FRZPicture { Source = "frozenveg.jpg", ImageText = "Frozen Vegetables", TargetPageType = typeof(FrozenVegetablesPage) },
                new FRZPicture { Source = "icecream.jpg", ImageText = "Ice Cream", TargetPageType = typeof(IcereemPage) },


            };
            RowTappedCommand = new Command<FRZPicture>(async (picture) =>
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
            Title = "Freezer";
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is FRZPicture picture)
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
            }
        }
    }

    public class FRZPicture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }

    }
}
