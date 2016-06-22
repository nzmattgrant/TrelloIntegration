using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class Card
    {
        public string Name { get; set; }
        public IEnumerable<string> Comments { get; set; }
    }
}