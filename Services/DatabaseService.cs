using SQLite;
using WalkBuddy.Data;
using WalkBuddy.Models;

namespace WalkBuddy.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public async Task<SQLiteAsyncConnection> Init()
        {
            if (_database != null)
                return _database;

            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Session>();

            return _database;
        }
    }
}
