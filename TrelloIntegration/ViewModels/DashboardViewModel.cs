using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TrelloIntegration.Services;

namespace TrelloIntegration.ViewModels
{
    public class DashboardViewModel
    {
        public string UserToken { get; set; }
        public IEnumerable<BoardViewModel> Boards { get; set; }
    }
}