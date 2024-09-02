using Milk.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Milk.Data
{
    public class DatabaseHelper
    {
        private readonly string _dbPath;
        private SQLiteConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _dbPath = dbPath;
            _database = new SQLiteConnection(_dbPath);
            CreateTable();
        }

        private void CreateTable()
        {
            _database.CreateTable<GroceryUsers>();
        }

        // ... other database operations ...
    }

}
