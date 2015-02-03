using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DerekApplication.Startup))]
namespace DerekApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
