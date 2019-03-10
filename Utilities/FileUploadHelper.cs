using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Http;

namespace Utilities
{
    public class FileUploadHelper
    {
        public FileUploadHelper()
        {
        }

        public async Task<string> UploadImageToAzureStorage(HttpPostedFileBase image, string newFileName)
        {
            try
            {
                Console.WriteLine("image==null: " + image == null);
                Console.WriteLine(image == null ? "" : "image.ContentLength: " + image.ContentLength + ", image.ContentType: " + image.ContentType);

                if (image != null && image.ContentLength != 0)
                {
                   
                    string connectionString = Helper.GetConnectionString("AzureStorageConnectionString");
                    //Connect to Azure
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

                    // Create a reference to the file client.
                    CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

                    // Get a reference to the file share we created previously.
                    CloudFileShare share = fileClient.GetShareReference("documents");
                    await share.CreateIfNotExistsAsync();

                    if (share.Exists())
                    {
                        // Generate a SAS for a file in the share
                        CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                        CloudFileDirectory cloudFileDirectory = rootDir.GetDirectoryReference("files");
                        await cloudFileDirectory.CreateIfNotExistsAsync();
                        CloudFile cloudFile = cloudFileDirectory.GetFileReference(newFileName);

                        Stream fileStream = image.InputStream;
                        cloudFile.UploadFromStream(fileStream);
                        fileStream.Dispose();

                        //Generating a shared access signature for a file or file share
                        string policyName = "sampleSharePolicy" + DateTime.UtcNow.Ticks;

                        // Create a new shared access policy and define its constraints.
                        SharedAccessFilePolicy sharedPolicy = new SharedAccessFilePolicy()
                        {
                            SharedAccessExpiryTime = DateTime.UtcNow.AddDays(2),
                            Permissions = SharedAccessFilePermissions.Read | SharedAccessFilePermissions.Write
                        };

                        // Get existing permissions for the share.
                        FileSharePermissions permissions = share.GetPermissions();
                        permissions.SharedAccessPolicies.Clear();
                        // Add the shared access policy to the share's policies. Note that each policy must have a unique name.
                        permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
                        share.SetPermissions(permissions);
                        
                        string sasToken = cloudFile.GetSharedAccessSignature(null, policyName);
                        Uri fileSasUri = new Uri(cloudFile.StorageUri.PrimaryUri.ToString() + sasToken);
                        return fileSasUri.OriginalString.ToString();
                    }
                }

                return "NULL";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return ex.Message + ex.StackTrace;
            }
        }

        public string sasToken(string filename)
        {
            string connectionString = Helper.GetConnectionString("AzureStorageConnectionString");
            //Connect to Azure
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create a reference to the file client.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            // Generate a SAS for a file in the share
            CloudFileShare share = fileClient.GetShareReference("documents");
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
            CloudFileDirectory cloudFileDirectory = rootDir.GetDirectoryReference("files");
            CloudFile cloudFile = cloudFileDirectory.GetFileReference(filename);

            //Generating a shared access signature for a file or file share
            string policyName = "sampleSharePolicy" + DateTime.UtcNow.Ticks;

            // Create a new shared access policy and define its constraints.
            SharedAccessFilePolicy sharedPolicy = new SharedAccessFilePolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddDays(2),
                Permissions = SharedAccessFilePermissions.Read | SharedAccessFilePermissions.Write
            };

            // Get existing permissions for the share.
            FileSharePermissions permissions = share.GetPermissions();

            // Add the shared access policy to the share's policies. Note that each policy must have a unique name.
            permissions.SharedAccessPolicies.Clear();
            permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);
            share.SetPermissions(permissions);

            // Generate a SAS for a file in the share and associate this access policy with it.
            cloudFile.Properties.ContentType = "application/octet-stream";
            string tick = $"&{ DateTimeOffset.UtcNow.Ticks}";
            string sasToken = cloudFile.GetSharedAccessSignature(null, policyName);
            Uri fileSasUri = new Uri(cloudFile.StorageUri.PrimaryUri.ToString() + sasToken + tick);
            return fileSasUri.OriginalString.ToString();
        }

        public bool UploadImageToDisk()
        {
            return true;
        }

        public bool UploadFileToAzureStorage()
        {
            //string connectionString = "";

            ////Connect to Azure
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse();

            //// Create a reference to the file client.
            //CloudFileClient = storageAccount.CreateCloudFileClient();

            //// Create a reference to the Azure path
            //CloudFileDirectory cloudFileDirectory = GetCloudFileShare().GetRootDirectoryReference().GetDirectoryReference(path);

            ////Create a reference to the filename that you will be uploading
            //CloudFile cloudFile = cloudSubDirectory.GetFileReference(fileName);

            ////Open a stream from a local file.
            //Stream fileStream = File.OpenRead(localfile);

            ////Upload the file to Azure.
            //await cloudFile.UploadFromStreamAsync(fileStream);
            //fileStream.Dispose();

            return true;
        }

        public Task<HttpResponseMessage> UploadFileToDisk(HttpRequestMessage request)
        {
            //if (!request.Content.IsMimeMultipartContent())
            //{
            //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //}

            //string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/uploads");
            //var provider = new MultipartFormDataStreamProvider(root);

            //var task = request.Content.ReadAsMultipartAsync(provider).
            //    ContinueWith<HttpResponseMessage>(o =>
            //    {

            //        string file1 = provider.FileData[0].LocalFileName;
            //        // this is the file name on the server where the file was saved 

            //        return new HttpResponseMessage()
            //        {
            //            Content = new StringContent("File uploaded.")
            //        };
            //    }
            //);
            //return task;
            return null;
        }

        public async Task<bool> DeleteImageAzureStorage(string newFileName)
        {
            try
            {
                if (newFileName != null)
                {
                     string connectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
                    //Connect to Azure
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

                    // Create a reference to the file client.
                    CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

                    // Get a reference to the file share we created previously.
                    CloudFileShare share = fileClient.GetShareReference("documents");
                    await share.CreateIfNotExistsAsync();

                    if (share.Exists())
                    {
                        // Generate a SAS for a file in the share
                        CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                        CloudFileDirectory cloudFileDirectory = rootDir.GetDirectoryReference("files");
                        await cloudFileDirectory.CreateIfNotExistsAsync();
                        CloudFile cloudFile = cloudFileDirectory.GetFileReference(newFileName);
                        cloudFile.DeleteIfExists(null);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                //throw ex;
                return false;
            }
        }
    }
}