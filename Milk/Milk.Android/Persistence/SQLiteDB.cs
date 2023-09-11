using Milk.Persistence;
using SQLite;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(App3.Droid.Persistence.SQLiteDb))]
namespace App3.Droid.Persistence
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, "SQLiteDB.db");
            return new SQLiteAsyncConnection(path);
        }
    }
}
