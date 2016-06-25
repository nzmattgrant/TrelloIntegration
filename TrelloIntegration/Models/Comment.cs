using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.Models
{
    public class Comment
    {
        public string ID { get; set; }
        public CommentDataViewModel Data { get; set; }
    }

    public class CommentDataViewModel
    {
        public Card Card { get; set; }
        public string Text { get; set; }
    }
}