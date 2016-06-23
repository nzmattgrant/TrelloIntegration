using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class Comment
    {
        public string id { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public Card card { get; set; }
        public string text { get; set; }
    }
}