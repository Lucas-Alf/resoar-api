using Amazon.S3;
using Amazon.S3.Model;

namespace Application.Interfaces.Services.Domain
{
    public interface IStorageService
    {
        /// <summary>
        /// Get a object from AWS S3 / Digital Ocean Spaces
        /// </summary>
        /// <param name="documentKey"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        Task<GetObjectResponse> GetObject(string documentKey, string folder);

        /// <summary>
        /// Delete a object from AWS S3 / Digital Ocean Spaces
        /// </summary>
        /// <param name="key"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        Task Delete(string key, string folder);

        /// <summary>
        /// Upload a new object to AWS S3 / Digital Ocean Spaces
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileKey"></param>
        /// <param name="folder"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        Task Upload(MemoryStream file, string fileKey, string folder, string contentType, S3CannedACL permission, Dictionary<string, string>? metadata = null);
    }
}
