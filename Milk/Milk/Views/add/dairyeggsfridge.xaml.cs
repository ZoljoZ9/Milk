using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Milk.Views.add.MeatsPoloutrySeafood;
using Milk.Views.add.DSF;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class dairyeggsfridge : ContentPage
    {
        public ObservableCollection<DEFPicture> Pictures { get; set; }
        public DEFPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public dairyeggsfridge()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<DEFPicture>
            {
                new DEFPicture { Source = "cheese.jpg", ImageText = "Cheese", TargetPageType = typeof(CheesePage) },
                new DEFPicture { Source = "cream.jpg", ImageText = "Cream, Custard & Desserts", TargetPageType = typeof(CreamCustardDessertsPage) },
                new DEFPicture { Source = "eggs.jpg", ImageText = "Eggs, Butter & Magarine", TargetPageType = typeof(EggsButterMagerinePage) },
                new DEFPicture { Source = "milk1.jpg", ImageText = "Milk", TargetPageType = typeof(MilkPage) },
                new DEFPicture { Source = "yoghurt.jpg", ImageText = "Yoghurt", TargetPageType = typeof(YoghurtPage) },
            };
            RowTappedCommand = new Command<DEFPicture>(async (picture) =>
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
            Title = "Dairy, Eggs, & Fridge";
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is DEFPicture picture)
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
            }
        }
    }

    public class DEFPicture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }

    }
}
