using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrelloIntegration.Models
{
    public class SessionInfo
    {
        [Key]
        [Column(Order = 1)]
        public string SessionID { get; set; }
        [Key]
        [Column(Order = 2)]
        public string UserToken { get; set; }
    }
}