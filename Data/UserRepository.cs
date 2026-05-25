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

            if (user.ChallengingStates.Count > 0)
                user.ChallengingString = string.Join(",", user.ChallengingStates);

            if (user.PositiveStates.Count > 0)
                user.PositiveString = string.Join(",", user.PositiveStates);

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
