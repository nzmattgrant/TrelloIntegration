using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class CommentViewModel
    {
        public string ID { get; set; }
        public CommentDataViewModel Data { get; set; }
    }

    public class CommentDataViewModel
    {
        public CardViewModel Card { get; set; }
        public string Text { get; set; }
    }
}