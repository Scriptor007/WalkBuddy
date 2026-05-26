using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Collections.ObjectModel;

namespace WalkBuddy.Models
{
    public partial class User : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _additionalInfo = string.Empty;

        [Ignore]
        public ObservableCollection<string> Interests { get; set; } = [];

        public string InterestsString { get; set; } = string.Empty;

        [Ignore]
        public ObservableCollection<string> ChallengingStates { get; set; } = [];

        public string ChallengingString { get; set; } = string.Empty;

        [Ignore]
        public ObservableCollection<string> PositiveStates { get; set; } = [];

        public string PositiveString { get; set; } = string.Empty;

        [ObservableProperty]
        private string _mobility = string.Empty;
    }
}
