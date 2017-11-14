using System.Web;
using System.Web.Mvc;
using GoPiGoControlCenter.Models;

namespace GoPiGoControlCenter.Controllers
{
	public class PhotoController : Controller
	{
		public ActionResult Test()
		{
			return View();
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult Upload(HttpPostedFileBase uploadPicture)
		{
			JsonResult returnResult = null;
			if (uploadPicture != null)
			{
				BlobHandler bh = new BlobHandler("pictures");
				var uri = bh.Upload(uploadPicture);

				GoPiGoHub.SendPictureUri(uri);

				returnResult = new JsonResult
				{
					Data = uri,
					ContentType = "text/plain",
					ContentEncoding = null
				};
			}

			return returnResult;
		}
	}
}