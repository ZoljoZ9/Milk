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
using Milk.Views.add.CleaningMaintenance;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class test : ContentPage
    {
        public ObservableCollection<CAMPicture> Pictures { get; set; }
    public CAMPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
    public ICommand RowTappedCommand { get; private set; }

    public test()
    {
        InitializeComponent();

        Pictures = new ObservableCollection<CAMPicture>
            {
                new CAMPicture { Source = "cleaning.jpg", ImageText = "Cleaning Goods", TargetPageType = typeof(CleaningGoodsPage) },
                new CAMPicture { Source = "bbq.jpg", ImageText = "Garden & Outdoors", TargetPageType = typeof(GardenOutdoorsPage) },
                new CAMPicture { Source = "hammer.jpg", ImageText = "Hardware", TargetPageType = typeof(HardwarePage) },
                new CAMPicture { Source = "kitchensupplies.jpg", ImageText = "Kitchen", TargetPageType = typeof(KitchenPage) },
                new CAMPicture { Source = "laundry.jpg", ImageText = "Laundry", TargetPageType = typeof(LaundryPage) },
                new CAMPicture { Source = "pestcontrol.jpg", ImageText = "Pest Control", TargetPageType = typeof(PestControlPage) },
                new CAMPicture { Source = "toiletpaper.jpg", ImageText = "Toilet Paper, Tissues, & Paper Towels", TargetPageType = typeof(ToiletPaperTissuesPaperTowelsPage) },


            };
        RowTappedCommand = new Command<CAMPicture>(async (picture) =>
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
        Title = "Cleaning & Maintenance";
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        if (sender is Image image && image.BindingContext is CAMPicture picture)
        {
            var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
            await Navigation.PushAsync(targetPage);
        }
    }
}

public class CAMPicture
{
    public string Source { get; set; }
    public string ImageText { get; set; }
    public Type TargetPageType { get; set; }

}
}
