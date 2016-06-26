using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrelloIntegration.ViewModels
{
    public class DashboardViewModel
    {
        public string UserFullName { get; set; }

        public DashboardViewModel() { }

        public DashboardViewModel(string userFullName)
        {
            UserFullName = userFullName;
        }
    }
}