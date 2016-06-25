using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.Models
{
    public class Card
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDList { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}