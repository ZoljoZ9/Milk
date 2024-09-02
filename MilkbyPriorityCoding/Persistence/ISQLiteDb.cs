using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Milk.Persistence
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();

    }
}
