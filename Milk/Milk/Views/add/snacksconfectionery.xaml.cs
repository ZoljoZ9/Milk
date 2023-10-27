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
using Milk.Views.add.SNC;

namespace Milk.Views.add
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class snacksconfectionery : ContentPage
    {
        public ObservableCollection<SNCPicture> Pictures { get; set; }
        public SNCPicture SelectedPicture { get; set; } // You need this property for the TwoWay binding
        public ICommand RowTappedCommand { get; private set; }

        public snacksconfectionery()
        {
            InitializeComponent();

            Pictures = new ObservableCollection<SNCPicture>
            {
                new SNCPicture { Source = "biscuts.jpg", ImageText = "Biscuts & Crackers", TargetPageType = typeof(BiscutCrackersPage) },
                new SNCPicture { Source = "chips.jpg", ImageText = "Chips", TargetPageType = typeof(ChipsPage) },
                new SNCPicture { Source = "confect.jpg", ImageText = "Confectory", TargetPageType = typeof(ConfectionaryPage) },
                new SNCPicture { Source = "gum.jpg", ImageText = "Gums, Mints, & Lozingers", TargetPageType = typeof(GumsMintsLozPage) },
                new SNCPicture { Source = "snacks.jpg", ImageText = "Snacks", TargetPageType = typeof(SnacksPage) },
            };
            RowTappedCommand = new Command<SNCPicture>(async (picture) =>
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
            Title = "Snacks & Confectionery";
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.BindingContext is SNCPicture picture)
            {
                var targetPage = (Page)Activator.CreateInstance(picture.TargetPageType);
                await Navigation.PushAsync(targetPage);
            }
        }
    }

    public class SNCPicture
    {
        public string Source { get; set; }
        public string ImageText { get; set; }
        public Type TargetPageType { get; set; }

    }
}
