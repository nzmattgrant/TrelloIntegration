using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class Comment
    {
        public int id { get; set; }
        public string text { get; set; }
        public Card card { get; set; }
    }
}