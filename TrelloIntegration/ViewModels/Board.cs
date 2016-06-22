using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class Board
    {
        public string Name { get; set; }
        public IEnumerable<List> Lists { get; set; }
    }
}