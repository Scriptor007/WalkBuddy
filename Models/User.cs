using SQLite;
using System.Collections.ObjectModel;

namespace WalkBuddy.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Username { get; set; } = string.Empty;

        public string AdditionalInfo { get; set; } = string.Empty;

        [Ignore]
        public ObservableCollection<string> Interests { get; set; } = [];

        public string InterestsString { get; set; } = string.Empty;

        [Ignore]
        public ObservableCollection<object> ChallengingStates { get; set; } = [];

        public string ChallengingString { get; set; } = string.Empty;

        [Ignore]
        public ObservableCollection<object> PositiveStates { get; set; } = [];

        public string PositiveString { get; set; } = string.Empty;

        [Ignore]
        public ObservableCollection<object> Feelings { get; set; } = [];

        public string FeelingsString { get; set; } = string.Empty;

        public string Mobility { get; set; } = string.Empty;
    }
}
