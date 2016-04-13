using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarPoolDemo.Startup))]
namespace CarPoolDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
