using SQLite;
using WalkBuddy.Models;

namespace WalkBuddy.Data
{
    public class UserRepository
    {
        private readonly DatabaseService _databaseService;

        public UserRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> SaveUserAsync(User user)
        {
            SQLiteAsyncConnection db = await _databaseService.Init();

            if (user.Interests.Count > 0)
                user.InterestsString = string.Join(",", user.Interests);

            if (user.Feelings.Count > 0)
                user.FeelingsString = string.Join(",", user.Feelings);

            if (user.ID != 0)
                return await db.UpdateAsync(user);
            else
                return await db.InsertAsync(user);
        }

        public async Task<User> GetUserAsync(int id)
        {
            SQLiteAsyncConnection db = await _databaseService.Init();
            return await db.Table<User>().Where(u => u.ID == id).FirstOrDefaultAsync();
        }
    }
}
