using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalkBuddy.Models
{
    public class Session
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int PromptCount { get; set; } = 0;

        public Emotion StartEmotion { get; set; }

        public Emotion EndEmotion { get; set; };

        public string Note {  get; set; } = string.Empty;
    }
}
