using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class Card
    {
        public int id { get; set; }
        public string name { get; set; }
        public int idList { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}