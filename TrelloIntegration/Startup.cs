using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrelloIntegration.Startup))]
namespace TrelloIntegration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
