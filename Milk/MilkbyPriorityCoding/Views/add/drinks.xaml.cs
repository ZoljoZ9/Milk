using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls.Xaml;
using Milk.Views.add.MeatsPoloutrySeafood;
using Milk.Views.add.Drinking;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class drinks : ContentPage
    {
        public ObservableCollection<DRIPicture> Pictures { get; set; }
        public DRIPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public drinks()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<DRIPicture>
            {
                new DRIPicture { Source = "coffee.jpg", ImageText = "Coffee", TargetPageType = typeof(CoffeePage) },
                new DRIPicture { Source = "juice.jpg", ImageText = "Cordials, Juice, & Iced Teas", TargetPageType = typeof(CordialsJuiceIcedTeasPage) },
                new DRIPicture { Source = "choclatemilk.jpg", ImageText = "Flavoured Milk", TargetPageType = typeof(FlavouredMilkPage) },
                new DRIPicture { Source = "milk1.jpg", ImageText = "Long Life Milk", TargetPageType = typeof(LongLifeMilkPage) },
                new DRIPicture { Source = "coke.jpg", ImageText = "Soft Drinks", TargetPageType = typeof(SoftDrinksPage) },
                new DRIPicture { Source = "energydrinks.jpg", ImageText = "Sports & Energy Drinks", TargetPageType = typeof(SportsEnergyDrinksPage) },
                new DRIPicture { Source = "tea.jpg", ImageText = "Tea", TargetPageType = typeof(TeaPage) },
                new DRIPicture { Source = "water.jpg", ImageText = "Water", TargetPageType = typeof(WaterPage) },


            };
            RowTappedCommand = new Command<DRIPicture>(async (picture) =>
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
            Title = "Drinks";
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is DRIPicture picture)
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
            }
        }
    }

    public class DRIPicture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }

    }
}
