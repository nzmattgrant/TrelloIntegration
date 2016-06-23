using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class List
    {
        public string id { get; set; }
        public string name { get; set; }
        public string idBoard { get; set; }
        public IEnumerable<Card> Cards { get; set; }
    }
}