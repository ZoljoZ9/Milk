using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Milk.Models
{
    [Table("GroceryUsers")]
    public class GroceryUsers
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        // ... other properties
    }

}
