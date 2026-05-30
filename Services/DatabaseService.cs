using SQLite;

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
            await _database.CreateTableAsync<Models.User>();
            await _database.CreateTableAsync<Models.Session>();

            return _database;
        }
    }
}
