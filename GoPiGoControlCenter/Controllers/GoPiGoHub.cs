using System;
using Microsoft.AspNet.SignalR;

namespace GoPiGoControlCenter.Controllers
{
	public class GoPiGoHub : Hub
	{
		private static readonly IHubContext context = GlobalHost.ConnectionManager.GetHubContext<GoPiGoHub>();

		public void SendCommand(GoPiGoCommand command)
		{
			try
			{
				context.Clients.All.executeCommand(command.ToString());
			}
			catch (Exception e)
			{
				//TODO: handle and/or log e
			}
		}

		public void RegisterCar()
		{
			try
			{
				context.Clients.All.sendCarConnected();
			}
			catch (Exception e)
			{
				//TODO: handle and/or log e
			}
		}

		public static void SendPictureUri(string uri)
		{
			try
			{
				context.Clients.All.showPicture(uri);
			}
			catch (Exception e)
			{
				//TODO: handle and/or log e
			}
		}
	}
}