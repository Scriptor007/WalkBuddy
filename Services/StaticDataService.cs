namespace WalkBuddy.Services
{
    public class StaticDataService
    {
        public List<string> Interests { get; } = ["Park & gardens", "Trees & Woodland", "Flowers & plants", "Birds & wildlife", "Water - rivers & ponds", "Urban nature", "Sky & clouds", "Sounds of nature", "Light & shadows", "Textures & surfaces"];

        public List<string> ChallengingStates { get; } = ["Often stressed", "Hard to switch off", "Often low or flat", "Tired a lot", "Anxious thoughts", "Easily irritated"];

        public List<string> PositiveStates { get; } = ["Generally content", "Curious & open", "Looking for calm", "Appreciate small things", "Feeling motivated"];

        public List<string> Mobility { get; } = ["I can walk freely", "I have some mobility limitations"];
    }
}
