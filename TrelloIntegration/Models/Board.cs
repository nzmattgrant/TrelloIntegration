using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.Models
{
    public class Board
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<List> Lists { get; set; }
    }
}