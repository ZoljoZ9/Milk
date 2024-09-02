using Milk.Models;
using Milk.Views.add;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

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

        public async Task DeleteAllDelis()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Deli).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllDelis()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Deli).ToListAsync();
        }
        public async Task<List<Produce>> GetAllDelisForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Deli && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetDelisByUserId(int userId)
        {
            return await GetAllDelisForUser(userId);
        }


        public async Task InsertOrReplaceDelis(List<Produce> delis)
        {
            foreach (var deli in delis)
            {
                deli.Type = Produce.ProduceType.Deli;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(deli);
            }
        }

        public async Task<Produce> GetDelisByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }
        public async Task<int> GetDelisQuantityByNameAndUserId(string name, int userId)
        {
            var deli = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return deli?.Quantity ?? 0;
        }

        public async Task DeleteAllCheeses()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Cheese).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllCheeses()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Cheese).ToListAsync();
        }
        public async Task<List<Produce>> GetAllCheesesForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Cheese && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetCheesesByUserId(int userId)
        {
            return await GetAllCheesesForUser(userId);
        }

        public async Task<Produce> GetCheesesByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }
        public async Task InsertOrReplaceCheeses(List<Produce> cheeses)
        {
            foreach (var cheese in cheeses)
            {
                cheese.Type = Produce.ProduceType.Cheese;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(cheese);
            }
        }


        public async Task<int> GetCheesesQuantityByNameAndUserId(string name, int userId)
        {
            var cheese = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return cheese?.Quantity ?? 0;
        }

        public async Task DeleteAllBakingSupply()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Baking).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllBakingSupply()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Baking).ToListAsync();
        }
        public async Task<List<Produce>> GetAllBakingSupplyForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Baking && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetBakingSupplyByUserId(int userId)
        {
            return await GetAllBakingSupplyForUser(userId);
        }


        public async Task InsertOrReplaceBakingSupply(List<Produce> bakings)
        {
            foreach (var baking in bakings)
            {
                baking.Type = Produce.ProduceType.Cheese;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(baking);
            }
        }
        public async Task<Produce> GetBakingSupplyByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetBakingSupplyQuantityByNameAndUserId(string name, int userId)
        {
            var baking = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return baking?.Quantity ?? 0;
        }


        public async Task DeleteAllChip()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Baking).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllChip()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Chip).ToListAsync();
        }
        public async Task<List<Produce>> GetAllChipForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Chip && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetChipByUserId(int userId)
        {
            return await GetAllChipForUser(userId);
        }


        public async Task InsertOrReplaceChip(List<Produce> chips)
        {
            foreach (var chip in chips)
            {
                chip.Type = Produce.ProduceType.Chip;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(chip);
            }
        }
        public async Task<Produce> GetChipByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetChipQuantityByNameAndUserId(string name, int userId)
        {
            var chip = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return chip?.Quantity ?? 0;
        }

        public async Task DeleteAllFrozen()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Frozen).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllFrozen()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Frozen).ToListAsync();
        }
        public async Task<List<Produce>> GetAllFrozenForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Frozen && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetFrozenByUserId(int userId)
        {
            return await GetAllFrozenForUser(userId);
        }


        public async Task InsertOrReplaceFrozen(List<Produce> frozens)
        {
            foreach (var frozen in frozens)
            {
                frozen.Type = Produce.ProduceType.Frozen;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(frozen);
            }
        }
        public async Task<Produce> GetFrozenByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetFrozenQuantityByNameAndUserId(string name, int userId)
        {
            var frozen = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return frozen?.Quantity ?? 0;
        }

        public async Task DeleteAllDrink()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Drink).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllDrink()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Drink).ToListAsync();
        }
        public async Task<List<Produce>> GetAllDrinkForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Drink && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetDrinkByUserId(int userId)
        {
            return await GetAllDrinkForUser(userId);
        }


        public async Task InsertOrReplaceDrink(List<Produce> drinks)
        {
            foreach (var drink in drinks)
            {
                drink.Type = Produce.ProduceType.Drink;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(drink);
            }
        }
        public async Task<Produce> GetDrinkByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetDrinkQuantityByNameAndUserId(string name, int userId)
        {
            var drink = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return drink?.Quantity ?? 0;
        }

        public async Task DeleteAllPersonal()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Personal).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllPersonal()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Personal).ToListAsync();
        }
        public async Task<List<Produce>> GetAllPersonalForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Personal && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetPersonalByUserId(int userId)
        {
            return await GetAllPersonalForUser(userId);
        }


        public async Task InsertOrReplacePersonal(List<Produce> personals)
        {
            foreach (var personal in personals)
            {
                personal.Type = Produce.ProduceType.Personal;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(personal);
            }
        }
        public async Task<Produce> GetPersonalByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetPersonalQuantityByNameAndUserId(string name, int userId)
        {
            var personal = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return personal?.Quantity ?? 0;
        }

        public async Task DeleteAllCleaning()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Cleaning).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllCleaning()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Cleaning).ToListAsync();
        }
        public async Task<List<Produce>> GetAllCleaningForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Cleaning && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetCleaningByUserId(int userId)
        {
            return await GetAllCleaningForUser(userId);
        }


        public async Task InsertOrReplaceCleaning(List<Produce> cleanings)
        {
            foreach (var cleaning in cleanings)
            {
                cleaning.Type = Produce.ProduceType.Cleaning;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(cleaning);
            }
        }
        public async Task<Produce> GetCleaningByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetCleaningQuantityByNameAndUserId(string name, int userId)
        {
            var cleaning = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return cleaning?.Quantity ?? 0;
        }

        public async Task DeleteAllPet()
        {
            await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Pet).DeleteAsync();
        }

        public async Task<List<Produce>> GetAllPet()
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Pet).ToListAsync();
        }
        public async Task<List<Produce>> GetAllPetForUser(int userId)
        {
            return await _dbConnection.Table<Produce>().Where(p => p.Type == Produce.ProduceType.Pet && p.UserId == userId).ToListAsync();
        }
        public async Task<List<Produce>> GetPetByUserId(int userId)
        {
            return await GetAllPetForUser(userId);
        }

        public async Task DeleteUserById(int userId)
        {
            var user = await _dbConnection.Table<GroceryUsers>().Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                await _dbConnection.DeleteAsync(user);
            }
        }

        public async Task InsertOrReplacePet(List<Produce> pets)
        {
            foreach (var pet in pets)
            {
                pet.Type = Produce.ProduceType.Pet;                // Set the type as Vegetable
                await _dbConnection.InsertOrReplaceAsync(pet);
            }
        }
        public async Task<Produce> GetPetByNameAndUserId(string name, int userId)
        {
            return await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> GetPetQuantityByNameAndUserId(string name, int userId)
        {
            var pet = await _dbConnection.Table<Produce>().Where(f => f.Name == name && f.UserId == userId).FirstOrDefaultAsync();
            return pet?.Quantity ?? 0;
        }





    }
}
