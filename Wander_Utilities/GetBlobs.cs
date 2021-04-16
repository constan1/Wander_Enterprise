using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander_Utilities
{
    public class GetBlobs
    {

        private readonly IOptions<StorageAccountOptions> _optionAccessor;

        public GetBlobs(IOptions<StorageAccountOptions> optionAccessor)
        {
            _optionAccessor = optionAccessor;

        }

        public List<string> GetAllBlobs()
        {


            List<string> blob_list = new List<string>();

            BlobUtility blobUtility = new BlobUtility(_optionAccessor.Value.StorageAccountNameOption, _optionAccessor.Value.StorageAccountKeyOption);

            CloudBlobContainer container = blobUtility.blobClient.GetContainerReference("propertyimages");

            CloudBlobDirectory blob_d = container.GetDirectoryReference("New");

            var rootDirFolders = blob_d.ListBlobsSegmentedAsync(true, BlobListingDetails.Metadata, null, null, null, null).Result;

            foreach (var blob in rootDirFolders.Results)
            {

                blob_list.Add(blob.Uri.ToString());
            }

            return blob_list;
        }
}
}
