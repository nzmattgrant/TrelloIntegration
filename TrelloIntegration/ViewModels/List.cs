using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class List
    {
        public string Name { get; set; }
        public IEnumerable<Card> Cards { get; set; }
    }
}