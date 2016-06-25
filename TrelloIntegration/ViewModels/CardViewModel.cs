﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class CardViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string IDList { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}