using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class Board
    {
        //Lower case to match with the API calls
        public int id { get; set; }
        public string name { get; set; }
        public IEnumerable<List> Lists { get; set; }
    }
}