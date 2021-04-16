using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander_Utilities
{
  public class BlobUtility
    {
        public CloudBlobClient blobClient;

        public BlobUtility(string accountName, string accountKey)
        {
            

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            "DefaultEndpointsProtocol=https;AccountName=" + accountName + ";AccountKey=" + accountKey);
        

            blobClient = storageAccount.CreateCloudBlobClient();
        }
    }

}
