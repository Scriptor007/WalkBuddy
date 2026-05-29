using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using WalkBuddy.Models;

namespace WalkBuddy.Data
{
    public class SessionRepository
    {
        private readonly DatabaseService _databaseService;

        public SessionRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> SaveSessionAsync(Session session)
        {
            SQLiteAsyncConnection db = await _databaseService.Init();

            if (session.ID != 0)
                return await db.UpdateAsync(session);
            else
                return await db.InsertAsync(session);
        }

        public async Task<Session?> GetSessionAsync(int id)
        {
            SQLiteAsyncConnection db = await _databaseService.Init();
            return await db.Table<Session>()
                .Where(s => s.ID == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Session?> GetLatestSessionAsync()
        {
            SQLiteAsyncConnection db = await _databaseService.Init();
            return await db.Table<Session>()
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
