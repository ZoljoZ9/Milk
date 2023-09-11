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
                new PMSPicture { Source = "apple.png", ImageText = "Poultry", TargetPageType = typeof(PoultryPage) },
                new PMSPicture { Source = "pumpkins.png", ImageText = "Meat", TargetPageType = typeof(RedMeatPage) },
                new PMSPicture { Source = "pumpkins.png", ImageText = "Meat", TargetPageType = typeof(LambPage) },
                new PMSPicture { Source = "pumpkins.png", ImageText = "Meat", TargetPageType = typeof(PorkPage) },
                new PMSPicture { Source = "pumpkins.png", ImageText = "Seafood", TargetPageType = typeof(SeafoodPage) },


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
            Title = "Select your groceries";
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
