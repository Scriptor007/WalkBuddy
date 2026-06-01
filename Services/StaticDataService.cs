using WalkBuddy.Models;
using WalkBuddy.Utilities;

namespace WalkBuddy.Services
{
    public class StaticDataService
    {
        public List<string> Interests { get; } = ["Park & gardens", "Trees & Woodland", "Flowers & plants", "Birds & wildlife", "Water - rivers & ponds", "Urban nature", "Sky & clouds", "Sounds of nature", "Light & shadows", "Textures & surfaces"];

        public List<string> ChallengingStates { get; } = ["Often stressed", "Hard to switch off", "Often low or flat", "Tired a lot", "Anxious thoughts", "Easily irritated"];

        public List<string> PositiveStates { get; } = ["Generally content", "Curious & open", "Looking for calm", "Appreciate small things", "Feeling motivated"];

        public List<string> Feelings { get; } = ["Stressed", "Anxious", "Sad", "Neutral", "Content"];

        public List<string> Mobility { get; } = ["I can walk freely", "I have some mobility limitations"];

        public List<EmotionWithIcon> Emotions { get; } = [
            new EmotionWithIcon(Emotion.Stressed.ToString(), "\uF6A2"),
            new EmotionWithIcon(Emotion.Low.ToString(), "\uF6A1"),
            new EmotionWithIcon(Emotion.Neutral.ToString(), "\uE812"),
            new EmotionWithIcon(Emotion.Calm.ToString(), "\uF6A7"),
            new EmotionWithIcon(Emotion.Energised.ToString(), "\uE1D3")];
    }
}
