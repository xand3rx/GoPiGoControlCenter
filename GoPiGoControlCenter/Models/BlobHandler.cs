using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GoPiGoControlCenter.Models
{
	public class BlobHandler
	{
		// Retrieve storage account from connection string.
		private readonly CloudStorageAccount storageAccount;
		private readonly string imageDirectoryUrl;

		/// <summary>
		/// Receives the users Id for where the pictures are and creates 
		/// a blob storage with that name if it does not exist.
		/// </summary>
		/// <param name="imageDirectoryUrl"></param>
		public BlobHandler(string imageDirectoryUrl)
		{
			var connectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
			this.storageAccount = CloudStorageAccount.Parse(connectionString);
			this.imageDirectoryUrl = imageDirectoryUrl;

			// Create the blob client.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Retrieve a reference to a container. 
			CloudBlobContainer container = blobClient.GetContainerReference(imageDirectoryUrl);

			// Create the container if it doesn't already exist.
			container.CreateIfNotExists();

			//Make available to everyone
			container.SetPermissions(
				new BlobContainerPermissions
				{
					PublicAccess = BlobContainerPublicAccessType.Blob
				});
		}

		public string Upload(HttpPostedFileBase file)
		{
			if (file == null)
			{
				throw new ArgumentNullException(nameof(file));
			}

			// Create the blob client.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Retrieve a reference to a container. 
			CloudBlobContainer container = blobClient.GetContainerReference(imageDirectoryUrl);

			CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
			blockBlob.UploadFromStream(file.InputStream);

			return blockBlob.Uri.ToString();
		}

		public List<string> GetBlobs()
		{
			// Create the blob client. 
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Retrieve reference to a previously created container.
			CloudBlobContainer container = blobClient.GetContainerReference(imageDirectoryUrl);

			List<string> blobs = new List<string>();

			// Loop over blobs within the container and output the URI to each of them
			foreach (var blobItem in container.ListBlobs())
				blobs.Add(blobItem.Uri.ToString());

			return blobs;
		}
	}
}