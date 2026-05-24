using CommunityToolkit.Mvvm.ComponentModel;
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
        public ObservableCollection<string> Feelings { get; set; } = [];

        public string FeelingsString { get; set; } = string.Empty;
    }
}
