using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GoPiGoControlCenter.Startup))]
namespace GoPiGoControlCenter
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}