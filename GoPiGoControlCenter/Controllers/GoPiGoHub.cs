using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace GoPiGoControlCenter.Controllers
{
	public class GoPiGoHub : Hub
	{
		private static readonly IHubContext context = GlobalHost.ConnectionManager.GetHubContext<GoPiGoHub>();
		private static string carConnectionIdId;

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
				if (!string.IsNullOrEmpty(carConnectionIdId))
				{
					throw new InvalidOperationException("There is already a car connected?!");
				}
				carConnectionIdId = this.Context.ConnectionId;
				context.Clients.All.sendCarConnected(true);
			}
			catch (InvalidOperationException)
			{
				throw;
			}
			catch (Exception e)
			{
				//TODO: handle and/or log e
			}
		}

		public bool GetCarConnectionStatus()
		{
			return !string.IsNullOrEmpty(carConnectionIdId);
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

		public override Task OnDisconnected(bool stopCalled)
		{
			if (this.Context.ConnectionId == carConnectionIdId)
			{
				carConnectionIdId = null;
				context.Clients.All.sendCarConnected(false);
			}
			return base.OnDisconnected(stopCalled);
		}
	}
}