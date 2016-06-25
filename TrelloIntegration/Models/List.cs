using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.Models
{
    public class List
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDBoard { get; set; }
        public IEnumerable<Card> Cards { get; set; }
    }
}