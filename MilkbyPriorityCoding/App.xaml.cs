using Milk.Data;
using Milk.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Milk
{
    public partial class App : Application
    {
        public static int LoggedInUserId { get; set; }
        public static string DatabasePath { get; private set; }
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Login());

        }
        private async Task InitializeAsync()
        {
            await App.DbContext.InitializeDatabaseAsync();
        }



        public static GroceryUsers LoggedInUser { get; set; }

        private static AppDbContext _dbContext;

        public static AppDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    try
                    {
                        // Use FileSystem.AppDataDirectory for MAUI
                        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "MySQLite.db3");

                        DatabasePath = dbPath;

                        // Initialize the DbContext with the path
                        _dbContext = new AppDbContext(dbPath);
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
                return _dbContext;
            }
        }





        protected override async void OnStart()
        {
            await InitializeAsync();
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
