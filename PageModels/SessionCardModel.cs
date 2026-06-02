using System;
using System.Collections.Generic;
using System.Text;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    public class SessionCardModel
    {
        public string DateText { get; init; } = string.Empty;
        public string StartEmotionText { get; init; } = string.Empty;
        public string EndEmotionText { get; init; } = string.Empty;
        public string PromptPreview { get; init; } = string.Empty;
        public string FooterText { get; init; } = string.Empty;
        public static SessionCardModel FromSession(Session session)
        {
            var dateText = session.CreatedAt.Date == DateTime.Today
                ? "Today"
                : session.CreatedAt.ToString("dddd, d MMM");
            var promptPreview = string.IsNullOrWhiteSpace(session.Note)
                ? "No reflection yet."
                : session.Note.Trim();
            var reflectionPreview = string.IsNullOrWhiteSpace(session.Note)
                ? "No reflection yet"
                : session.Note.Trim();
            var footerText = session.PromptCount > 0
                ? $"{session.PromptCount} prompts · {reflectionPreview}"
                : reflectionPreview;
            return new SessionCardModel
            {
                DateText = dateText,
                StartEmotionText = session.StartEmotion.ToString(),
                EndEmotionText = session.EndEmotion.ToString(),
                PromptPreview = promptPreview,
                FooterText = footerText
            };
        }
    }
}
