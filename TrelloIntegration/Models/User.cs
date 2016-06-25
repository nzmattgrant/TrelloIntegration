using System;

namespace TrelloIntegration.Models
{
    public class User
    {
        public string ID { get; set; }
        public string FullName { get; set; }
        public string TrelloToken { get; set; }
        public DateTime LastLoggedIn { get; set; }
    }
}