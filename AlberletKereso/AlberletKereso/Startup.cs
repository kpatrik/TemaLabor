using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AlberletKereso.Startup))]
namespace AlberletKereso
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
