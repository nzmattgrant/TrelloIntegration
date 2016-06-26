using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TrelloIntegration.Models;

namespace TrelloIntegration.DAL
{
    public interface ITrelloIntegrationContext
    {
        DbSet<User> Users { get; set; }
        int SaveChanges();
    }

    public class TrelloIntegrationContext : DbContext, ITrelloIntegrationContext
    {
        public TrelloIntegrationContext() : base("TrelloIntegrationContext")
        {
            Database.SetInitializer<TrelloIntegrationContext>(new CreateDatabaseIfNotExists<TrelloIntegrationContext>());
        }

        public DbSet<User> Users { get; set; }

    }
}