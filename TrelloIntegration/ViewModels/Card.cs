using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class Card
    {
        public string id { get; set; }
        public string name { get; set; }
        public string idList { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}