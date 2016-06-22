﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class List
    {
        public int id { get; set; }
        public string name { get; set; }
        public int idBoard { get; set; }
        public IEnumerable<Card> Cards { get; set; }
    }
}