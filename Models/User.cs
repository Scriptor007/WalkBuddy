using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WalkBuddy.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Username { get; set; } = string.Empty;
    }
}
