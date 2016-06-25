using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class BoardViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<ListViewModel> Lists { get; set; }
    }
}