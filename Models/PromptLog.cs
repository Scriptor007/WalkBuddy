using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalkBuddy.Models
{
    public class PromptLog
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string PromptText { get; set; } = string.Empty;

        public bool WasRejected { get; set; } = false;

        public bool UsedMidWalkCheckIn { get; set; } = false;

        public bool UsedAudioOutput { get; set; } = false;

        public double ScreenOnTimeSeconds { get; set; } = 0;

        public double TimeToFirstInteractionSeconds { get; set; } = 0;
    }
}
