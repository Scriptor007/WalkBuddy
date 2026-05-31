using System;
using System.Collections.Generic;
using System.Text;

namespace WalkBuddy.Models
{
    public class EmotionWithIcon
    {
        public string? Emotion { get; set; } = string.Empty;

        public string? Glyph { get; set; } = string.Empty;

        public EmotionWithIcon(string? emotion = null, string? glyph = null)
        {
            Emotion = emotion;
            Glyph = glyph;
        }
    }
}
