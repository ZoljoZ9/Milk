using Milk.Views.add;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Milk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Browse : ContentPage
    {
        public ObservableCollection<Picture> Pictures { get; set; }
        public Picture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public Browse()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<Picture>
            {
                new Picture { Source = "bananas.png", ImageText = "Fruit & Veg", TargetPageType = typeof(fruitveg) },
                new Picture { Source = "Bread.png", ImageText = "Bakery", TargetPageType = typeof(BakeryPage) },
                new Picture { Source = "meat.png", ImageText = "Meat & Seafood", TargetPageType = typeof(pms) },
                new Picture { Source = "deli.jpg", ImageText = "Deli", TargetPageType = typeof(deliPage) },
                new Picture { Source = "milk1.jpg", ImageText = "Dairy, Eggs & Fridge", TargetPageType = typeof(dairyeggsfridge) },
                new Picture { Source = "oil.jpg", ImageText = "Pantry", TargetPageType = typeof(pantry) },
                new Picture { Source = "confect.jpg", ImageText = "Snacks & Confectionary", TargetPageType = typeof(snacksconfectionery) },
                new Picture { Source = "icecream.jpg", ImageText = "Frozen", TargetPageType = typeof(freezer) },
                new Picture { Source = "lemon.jpg", ImageText = "Drinks", TargetPageType = typeof(drinks) },
                new Picture { Source = "personal.jpg", ImageText = "Beauty & Personal care", TargetPageType = typeof(beautypersonalcare) },
                new Picture { Source = "cleaning.jpg", ImageText = "Cleaning & Maintenance", TargetPageType = typeof(test) },
                new Picture { Source = "pet.jpg", ImageText = "Pet", TargetPageType = typeof(pet) },

            };
            RowTappedCommand = new Command<Picture>(async (picture) =>
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
                Console.WriteLine("Navigated to: " + picture.TargetPageType.Name); // Add this line for debugging
            });
            this.BindingContext = this;


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Title = "Select your groceries";

            // Check if there are more than one page on the navigation stack
            if (Navigation.NavigationStack.Count > 1)
            {
                // Pop all pages except the root page (Browse page in this case)
                for (int i = 0; i < Navigation.NavigationStack.Count - 1; i++)
                {
                    await Navigation.PopAsync(animated: false);
                }
            }
        }



    }

    public class Picture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }
    }
}
