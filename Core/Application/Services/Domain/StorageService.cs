using System.Web;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.Interfaces.Services.Domain;
using Domain.Utils;

namespace Application.Services.Domain
{
    public class StorageService : IStorageService
    {
        private string BUCKET_NAME { get; } = "resoar";

        /// <summary>
        /// Get a object from AWS S3 / Digital Ocean Spaces
        /// </summary>
        /// <param name="key"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task<GetObjectResponse> GetObject(string key, string folder)
        {
            var client = GetClient();
            return await client.GetObjectAsync(
                bucketName: $"{BUCKET_NAME}/{folder}",
                key: key
            );
        }

        /// <summary>
        /// Delete a object from AWS S3 / Digital Ocean Spaces
        /// </summary>
        /// <param name="key"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task Delete(string key, string folder)
        {
            var client = GetClient();
            await client.DeleteObjectAsync(
                bucketName: $"{BUCKET_NAME}/{folder}",
                key: key
            );
        }

        /// <summary>
        /// Upload a new object to AWS S3 / Digital Ocean Spaces
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="key"></param>
        /// <param name="folder"></param>
        /// <param name="contentType"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public async Task Upload(
            MemoryStream stream,
            string key,
            string folder,
            string contentType,
            S3CannedACL permission,
            Dictionary<string, string>? metadata = null
        )
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = key,
                BucketName = $"{BUCKET_NAME}/{folder}",
                ContentType = contentType,
                CannedACL = permission
            };

            if (metadata != null)
            {
                foreach (var item in metadata)
                {
                    uploadRequest.Metadata.Add(item.Key, HttpUtility.UrlEncode(item.Value));
                }
            }


            var client = GetClient();
            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }

        /// <summary>
        /// Creates a new storage client
        /// </summary>
        /// <returns></returns>
        private AmazonS3Client GetClient()
        {
            var region = EnvironmentManager.GetStorageRegion();
            var endpoint = EnvironmentManager.GetStorageEndpoint();
            var config = new AmazonS3Config();

            if (!String.IsNullOrEmpty(region))
                config.RegionEndpoint = RegionEndpoint.GetBySystemName(region);

            if (!String.IsNullOrEmpty(endpoint))
                config.ServiceURL = endpoint;

            return new AmazonS3Client(
                awsAccessKeyId: EnvironmentManager.GetStorageAccessKey(),
                awsSecretAccessKey: EnvironmentManager.GetStorageSecretKey(),
                clientConfig: config
            );
        }
    }
}