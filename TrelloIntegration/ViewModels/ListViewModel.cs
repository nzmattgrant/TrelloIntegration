using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class ListViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDBoard { get; set; }
        public IEnumerable<CardViewModel> Cards { get; set; }
    }
}