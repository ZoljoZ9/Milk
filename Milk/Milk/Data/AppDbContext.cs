using Milk.Models;
using Milk.Views.add;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Milk.Data
{
    public class AppDbContext : IDisposable
    {
        private SQLiteAsyncConnection _dbConnection;

        public AppDbContext(string dbPath)                            
        {
            _dbConnection = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeDatabaseAsync()
        {
            await _dbConnection.CreateTableAsync<GroceryUsers>();
            await _dbConnection.CreateTableAsync<Produce>();
            //await _dbConnection.CreateTableAsync<Vegetable>();
            //await _dbConnection.CreateTableAsync<BakeryItem>();// Add this line

        }
        public async Task<List<GroceryUsers>> GetUsersByEmail(string email)
        {
            return await _dbConnection.Table<GroceryUsers>().Where(u => u.email == email).ToListAsync();
        }

        public async Task InsertUser(GroceryUsers user)
        {
            await _dbConnection.InsertAsync(user);
        }
        public async Task DeleteAllFruit()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Fruit).DeleteAsync();


        }



        public async Task<List<Produce>> GetAllFruit()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Fruit).ToListAsync();
        }

        public async Task<List<Produce>> GetAllFruitForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Fruit && p.UserId == userId).ToListAsync();
        }

        public async Task<List<Produce>> GetFruitByUserId(int userId)
        {
            return await GetAllFruitForUser(userId);
        }


        public SQLiteAsyncConnection Connection
        {
            get { return _dbConnection; }
        }

        public void Dispose()
        {
            _dbConnection.CloseAsync().Wait();
            _dbConnection = null;
        }



        public async Task InsertOrReplaceFruit(List<Produce> fruits)
        {
            foreach (var fruit in fruits)
            {
                fruit.Type = Produce.ProduceType.Fruit; // Set the type as Fruit
                await _dbConnection.InsertOrReplaceAsync(fruit);
            }
        }

        public async Task<Produce> GetFruitByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetFruitQuantityByNameAndUserId(string name, int userId)
        {
            var fruit = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return fruit?.Quantity ?? 0;
        }

        // Methods for Vegetables...
        public async Task DeleteAllVegetable()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Vegetable).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllVegetable()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Vegetable).ToListAsync();
        }
        public async Task<List<Produce>> GetAllVegetablesForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Vegetable && p.UserId == userId).ToListAsync();
        }

        public async Task<List<Produce>> GetVegetableByUserId(int userId)
        {
            return await GetAllVegetablesForUser(userId);
        }


        public async Task InsertOrReplaceVegetable(List<Produce> vegetables)
        {
            foreach (var vegetable in vegetables)
            {
                vegetable.Type = Produce.ProduceType.Vegetable;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(vegetable);
            }
        }

        public async Task<Produce> GetVegetableByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }
        public async Task<int> GetVegetableQuantityByNameAndUserId(string name, int userId)
        {
            var vegetable = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return vegetable?.Quantity ?? 0;
        }

        // Methods for Bakery...
        public async Task DeleteAllBakeries()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Bakery).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllBakeries()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Bakery).ToListAsync();
        }
        public async Task<List<Produce>> GetAllBakeriesForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Bakery && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetBakeriesByUserId(int userId)
        {
            return await GetAllBakeriesForUser(userId);
        }
        public async Task DeleteProduce(Produce item)
        {
            await _dbConnection.DeleteAsync(item);
        }

        public async Task InsertOrReplaceBakeries(List<Produce> bakeries)
        {
            foreach (var bakery in bakeries)
            {
                bakery.Type = Produce.ProduceType.Bakery;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(bakery);
            }
        }

        public async Task<Produce> GetBakeriesByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }
        public async Task<int> GetBakeriesQuantityByNameAndUserId(string name, int userId)
        {
            var bakery = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return bakery?.Quantity ?? 0;
        }
        public SQLiteAsyncConnection Database => _dbConnection;

        public async Task DeleteAllSalad()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Salad).DeleteAsync();


        }



        public async Task<List<Produce>> GetAllSalad()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Salad).ToListAsync();
        }

        public async Task<List<Produce>> GetAllSaladForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Salad && p.UserId == userId).ToListAsync();
        }

        public async Task<List<Produce>> GetSaladByUserId(int userId)
        {
            return await GetAllSaladForUser(userId);
        }

        public async Task InsertOrReplaceSalad(List<Produce> salads)
        {
            foreach (var salad in salads)
            {
                salad.Type = Produce.ProduceType.Salad; // Set the type as Fruit
                await _dbConnection.InsertOrReplaceAsync(salad);
            }
        }

        public async Task<Produce> GetSaladByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetSaladQuantityByNameAndUserId(string name, int userId)
        {
            var salad = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return salad?.Quantity ?? 0;
        }

        public async Task DeleteAllRedMeat()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.RedMeat).DeleteAsync();


        }



        public async Task<List<Produce>> GetAllRedMeat()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.RedMeat).ToListAsync();
        }

        public async Task<List<Produce>> GetAllRedMeatForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.RedMeat && p.UserId == userId).ToListAsync();
        }

        public async Task<List<Produce>> GetRedMeatByUserId(int userId)
        {
            return await GetAllRedMeatForUser(userId);
        }

        public async Task InsertOrReplaceBeef(List<Produce> redmeats)
        {
            foreach (var redmeat in redmeats)
            {
                redmeat.Type = Produce.ProduceType.RedMeat; // Set the type as Fruit
                await _dbConnection.InsertOrReplaceAsync(redmeat);
            }
        }

        public async Task<Produce> GetRedMeatByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetRedMeatQuantityByNameAndUserId(string name, int userId)
        {
            var redmeat = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return redmeat?.Quantity ?? 0;
        }
    }
}
