using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TrelloIntegration.Models
{
    public class TrelloIntegrationContext : DbContext
    {
        public DbSet<SessionInfo> SessionInfos { get; set; }

        public TrelloIntegrationContext()
            : base("DefaultConnection")
        { 
            
        }
    }
}