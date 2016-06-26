namespace TrelloIntegration.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TrelloIntegration.DAL.TrelloIntegrationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TrelloIntegration.DAL.TrelloIntegrationContext";
        }

        protected override void Seed(TrelloIntegration.DAL.TrelloIntegrationContext context){}
    }
}
