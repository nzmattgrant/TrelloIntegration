using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TrelloIntegration.Models;

namespace TrelloIntegration.DAL
{
    public class TrelloIntegrationContext : DbContext
    {
        public TrelloIntegrationContext() : base("TrelloIntegrationContext") { }

        public DbSet<User> Users { get; set; }

    }
}