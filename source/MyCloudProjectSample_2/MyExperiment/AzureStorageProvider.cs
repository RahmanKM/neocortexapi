using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment
{
    public class AzureStorageProvider : IStorageProvider
    {
        private MyConfig config;

        /// <summary>
        /// Constructor that configures the Azure storage provider using settings defined in the configuration section.
        /// </summary>
        /// <param name="configSection">Configuration section from which to bind settings.</param>
        public AzureStorageProvider(IConfigurationSection configSection)
        {
            config = new MyConfig();
            configSection.Bind(config);  // Bind configuration settings to the MyConfig object
        }

        /// <summary>
        /// Downloads a file from an Azure Blob Container and returns its contents as a string.
        /// Throws FileNotFoundException if the file does not exist.
        /// </summary>
        /// <param name="fileName">The name of the file to download.</param>
        /// <returns>The contents of the file as a string.</returns>
        public async Task<string> DownloadInputFile(string fileName)
        {
            BlobContainerClient container = new BlobContainerClient(this.config.StorageConnectionString, this.config.TrainingContainer);
            await container.CreateIfNotExistsAsync(); // Ensure the container exists

            BlobClient blob = container.GetBlobClient(fileName); // Get a reference to the blob

            if (await blob.ExistsAsync()) // Check if the blob exists
            {
                BlobDownloadInfo download = await blob.DownloadAsync(); // Download the blob content

                using (StreamReader reader = new StreamReader(download.Content)) // Read the content from the stream
                {
                    return await reader.ReadToEndAsync();
                }
            }
            else
            {
                throw new FileNotFoundException($"The file '{fileName}' does not exist in the blob container.");
            }
        }

        /// <summary>
        /// Uploads experimental results to Azure Table Storage, handling errors gracefully and logging output.
        /// </summary>
        /// <param name="result">The experiment result to upload.</param>
        public async Task UploadExperimentResult(IExperimentResult result)
        {
            Random rnd = new Random();
            int rowKeyNumber = rnd.Next(0, 1000);
            string rowKey = "rahmancc-" + rowKeyNumber.ToString();
            string partitionKey = "rahman-cc-proj-" + rowKey;

            var testResult = new ExperimentResult(partitionKey, rowKey)
            {
                ExperimentId = result.ExperimentId,
                Name = result.Name,
                Description = result.Description,
                StartTimeUtc = result.StartTimeUtc,
                EndTimeUtc = result.EndTimeUtc,
                TestData = result.TestData,
            };

            Console.WriteLine($"Upload ExperimentResult to table: {this.config.ResultTable}");
            var client = new TableClient(this.config.StorageConnectionString, this.config.ResultTable);

            await client.CreateIfNotExistsAsync();

            try
            {
                await client.AddEntityAsync<ExperimentResult>(testResult);
                Console.WriteLine("Uploaded to Table Storage completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload to Table Storage: {ex.ToString()}");
            }
        }

        /// <summary>
        /// Uploads a single result file to Azure Blob Storage.
        /// </summary>
        /// <param name="fileName">The name of the file to upload.</param>
        /// <param name="data">The data to upload in byte array format.</param>
        public async Task UploadResultFile(string fileName, byte[] data)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(this.config.StorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(this.config.ResultContainer);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            using (MemoryStream memoryStream = new MemoryStream(data)) // Prepare the data for uploading
            {
                await blobClient.UploadAsync(memoryStream);
            }
        }

        /// <summary>
        /// Uploads multiple result files to Azure Blob Storage, each under a unique file name.
        /// </summary>
        /// <param name="baseFileName">Base name for files to generate unique names for each upload.</param>
        /// <param name="resultImages">List of image data in byte array format to upload.</param>
        public async Task UploadResultFiles(string baseFileName, List<byte[]> resultImages)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(this.config.StorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(this.config.ResultContainer);

            for (int i = 0; i < resultImages.Count; i++)
            {
                string fileName = $"{baseFileName}_{i + 1}.png"; // Generate a unique file name for each image

                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                using (MemoryStream memoryStream = new MemoryStream(resultImages[i])) // Upload each image
                {
                    await blobClient.UploadAsync(memoryStream);
                }
            }
        }
    }
}

