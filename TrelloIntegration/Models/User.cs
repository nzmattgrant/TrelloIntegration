using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.Models
{
    public class User
    {
        //[key]
        public string id { get; set; }
        public string fullName { get; set; }
        public string TrelloToken { get; set; }

    }
}