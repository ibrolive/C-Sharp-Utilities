using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using Amazon;
using Amazon.S3.Model;
using System.Web;
using System.Net;

namespace Utilities
{
    public class AmazonFileUploadHelper
    {
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        private static IAmazonS3 s3Client;

        public async Task<string> UploadFileAsync(HttpPostedFileBase file, string uniqueFileName)
        {
            try
            {
                IAmazonS3 client = new AmazonS3Client(Helper.GetConfiguration("AWS_ACCESS_KEY_ID"), Helper.GetConfiguration("AWS_SECRET_KEY"), bucketRegion);
                var fileTransferUtility =
                        new TransferUtility(Helper.GetConfiguration("AWS_SECRET_KEY"), Helper.GetConfiguration("AWS_SECRET_KEY"), bucketRegion);
                Stream fileStream = file.InputStream;
                await fileTransferUtility.UploadAsync(fileStream,
                                                  Helper.GetConfiguration("BUCKET_NAME"), file.FileName);
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = Helper.GetConfiguration("BUCKET_NAME"),
                    InputStream = file.InputStream,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 9291456, // 9 MB.
                    Key = file.FileName,
                    CannedACL = S3CannedACL.PublicRead
                };
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                request.BucketName = Helper.GetConfiguration("BUCKET_NAME");
                request.Key = file.FileName;
                request.Expires = DateTime.Now.AddYears(10);
                request.Protocol = Protocol.HTTP;
                string url = client.GetPreSignedURL(request);
                return url;
            }
            catch (AmazonS3Exception e)
            {
                return e.Message + e.StackTrace;
            }
            catch (Exception e)
            {
                return e.Message + e.StackTrace;
            }
        }

        public async Task<string> Upload(HttpPostedFileBase file)
        {
            IAmazonS3 client = new AmazonS3Client(Helper.GetConfiguration("AWS_ACCESS_KEY_ID"), Helper.GetConfiguration("AWS_SECRET_KEY"), bucketRegion);
            var fileTransferUtility =
                    new TransferUtility(Helper.GetConfiguration("AWS_ACCESS_KEY_ID"), Helper.GetConfiguration("AWS_SECRET_KEY"), bucketRegion);
            Stream fileStream = file.InputStream;
            await fileTransferUtility.UploadAsync(fileStream,
                                              Helper.GetConfiguration("BUCKET_NAME"), file.FileName);
            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = Helper.GetConfiguration("BUCKET_NAME"),
                InputStream = file.InputStream,
                StorageClass = S3StorageClass.StandardInfrequentAccess,
                PartSize = 6291456, // 6 MB.
                Key = file.FileName,
                CannedACL = S3CannedACL.PublicRead
            };
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
            request.BucketName = Helper.GetConfiguration("BUCKET_NAME");
            request.Key = file.FileName;
            request.Expires = DateTime.Now.AddDays(30);
            request.Protocol = Protocol.HTTP;
            string url = client.GetPreSignedURL(request);
            return url;
        }

        public async Task<bool> RemoveFile(string keyName)
        {
            try
            {
                IAmazonS3 client = new AmazonS3Client(Helper.GetConfiguration("AWS_ACCESS_KEY_ID"), Helper.GetConfiguration("AWS_SECRET_KEY"), bucketRegion);
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = Helper.GetConfiguration("BUCKET_NAME"),
                    Key = keyName
                };
                await client.DeleteObjectAsync(deleteObjectRequest);
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                return false;
            }
        }
    }
}
