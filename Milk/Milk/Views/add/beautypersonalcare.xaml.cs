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
using Milk.Views.add.BeautyPersonalCare;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class beautypersonalcare : ContentPage
    {
        public ObservableCollection<BPCPicture> Pictures { get; set; }
        public BPCPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public beautypersonalcare()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<BPCPicture>
            {
                new BPCPicture { Source = "cosmetic.jpg", ImageText = "Cosmetics", TargetPageType = typeof(CosmeticsPage) },
                new BPCPicture { Source = "dentalcare.jpg", ImageText = "Dental Care", TargetPageType = typeof(DentalCarePage) },
                new BPCPicture { Source = "deodorant.jpg", ImageText = "Female Deodorants & Body Sprays", TargetPageType = typeof(FemaleDeodorantsBodySpraysPage) },
                new BPCPicture { Source = "firstaid.jpg", ImageText = "First Aid & Medicinal", TargetPageType = typeof(FirstAidMedicinalPage) },
                new BPCPicture { Source = "shampoo.jpg", ImageText = "Hair Care", TargetPageType = typeof(HairCarePage) },
                new BPCPicture { Source = "razor.jpg", ImageText = "Men's Care", TargetPageType = typeof(MenCarePage) },
                new BPCPicture { Source = "tampons.jpg", ImageText = "Period Care", TargetPageType = typeof(PeriodCarePage) },
                new BPCPicture { Source = "personalcare.jpg", ImageText = "Personal Care & Hygiene", TargetPageType = typeof(PersonalCareHygienePage) },
                new BPCPicture { Source = "soap.jpg", ImageText = "Shower, Bath & Soap", TargetPageType = typeof(ShowerBathSoapPage) },
                new BPCPicture { Source = "skincare.jpg", ImageText = "Skin Care", TargetPageType = typeof(SkinCarePage) },
                new BPCPicture { Source = "suncream.jpg", ImageText = "Sun Protection", TargetPageType = typeof(SunProtectionPage) },
                new BPCPicture { Source = "razor.jpg", ImageText = "Women's Hair Removal", TargetPageType = typeof(WomenHairRemovalPage) },

            };
            RowTappedCommand = new Command<BPCPicture>(async (picture) =>
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
            Title = "Beauty & Personal Care";
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is BPCPicture picture)
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
            }
        }
    }

    public class BPCPicture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }

    }
}