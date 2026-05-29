using SQLite;
using WalkBuddy.Data;
using WalkBuddy.Models;
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
            await _database.CreateTableAsync<Session>();
            await SeedSessionsIfNeeded(_database);

            return _database;
        }

        private static async Task SeedSessionsIfNeeded(SQLiteAsyncConnection db)
        {
            var count = await db.Table<Session>().CountAsync();
            if (count > 0)
                return;

            var sessions = new[]
            {
                new Session
                {
                    CreatedAt = DateTime.Today.AddDays(-1).AddHours(9),
                    PromptCount = 3,
                    StartEmotion = Emotion.Stressed,
                    EndEmotion = Emotion.Calm,
                    Note = "Noted birds and softer light."
                },
                new Session
                {
                    CreatedAt = DateTime.Today.AddDays(-4).AddHours(17),
                    PromptCount = 2,
                    StartEmotion = Emotion.Low,
                    EndEmotion = Emotion.Neutral,
                    Note = "Felt steadier after a short loop."
                },
                new Session
                {
                    CreatedAt = DateTime.Today.AddDays(-9).AddHours(8),
                    PromptCount = 4,
                    StartEmotion = Emotion.Neutral,
                    EndEmotion = Emotion.Energised,
                    Note = string.Empty
                }
            };

            await db.InsertAllAsync(sessions);
        }
    }
}
