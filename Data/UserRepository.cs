using SQLite;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            if (user.Interests.Count > 0)
                user.InterestsString = string.Join(",", user.Interests);

            if (user.ChallengingStates.Count > 0)
                user.ChallengingString = string.Join(",", user.ChallengingStates);

            if (user.PositiveStates.Count > 0)
                user.PositiveString = string.Join(",", user.PositiveStates);

            SQLiteAsyncConnection db = await _databaseService.Init();

            if (user.ID != 0)
                return await db.UpdateAsync(user);
            else
                return await db.InsertAsync(user);
        }
        
        public async Task<User> GetUserAsync()
        {
            SQLiteAsyncConnection db = await _databaseService.Init();

            User user = await db.Table<User>().FirstOrDefaultAsync();
            MapToObservableCollections(user);

            return user;
        }

        private void MapToObservableCollections(User user)
        {
            user.Interests.Clear();

            if (!string.IsNullOrWhiteSpace(user.InterestsString))
            {
                string[] interests = user.InterestsString.Split(',');
                foreach (var interest in interests)
                {
                    user.Interests.Add(interest);
                }
            }

            user.PositiveStates.Clear();

            if (user.PositiveString != null)
            {
                string[] posStates = user.PositiveString.Split(',');
                foreach (var state in posStates)
                {
                    user.PositiveStates.Add(state);
                }
            }

            user.ChallengingStates.Clear();

            if (user.ChallengingString != null)
            {
                string[] challStates = user.ChallengingString.Split(',');
                foreach (var state in challStates)
                {
                    user.ChallengingStates.Add(state);
                }
            }
        }
    }
}
