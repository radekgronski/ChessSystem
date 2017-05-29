using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChessSystem.Startup))]
namespace ChessSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
