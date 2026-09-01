using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using WalkBuddy.Models;

namespace WalkBuddy.Data
{
    public class PromptRepository
    {
        private readonly DatabaseService _databaseService;

        public PromptRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<PromptLog> SaveSessionAsync(PromptLog promptLog)
        {
            SQLiteAsyncConnection db = await _databaseService.Init();

            if (promptLog.ID != 0)
                await db.UpdateAsync(promptLog);
            else
                await db.InsertAsync(promptLog);

            return promptLog;
        }
    }
}
