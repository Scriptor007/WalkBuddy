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

        public async Task<Session> SaveSessionAsync(Session session)
        {
            SQLiteAsyncConnection db = await _databaseService.Init();

            if (session.ID != 0)
                await db.UpdateAsync(session);
            else
                await db.InsertAsync(session);

            return session;
        }
    }
}
