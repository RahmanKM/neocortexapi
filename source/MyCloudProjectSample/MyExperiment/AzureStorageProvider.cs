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

        public AzureStorageProvider(IConfigurationSection configSection)
        {
            config = new MyConfig();
            configSection.Bind(config);
        }

        public async Task<string> DownloadInputFile(string fileName)
        {
            BlobContainerClient container = new BlobContainerClient(this.config.StorageConnectionString, this.config.TrainingContainer);
            await container.CreateIfNotExistsAsync();

            // Get a reference to a blob named "sample-file"
            BlobClient blob = container.GetBlobClient(fileName);

            // Check if the blob exists
            if (await blob.ExistsAsync())
            {
                // Download the blob content as a stream
                BlobDownloadInfo download = await blob.DownloadAsync();

                // Read the content from the stream and return it as a string
                using (StreamReader reader = new StreamReader(download.Content))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            else
            {
                throw new FileNotFoundException($"The file '{fileName}' does not exist in the blob container.");
            }
        }

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
                //await client.UpsertEntityAsync<ExperimentResult>(minimalResult);
                Console.WriteLine("Uploaded to Table Storage completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload to Table Storage: {ex.ToString()}");
            }

        }

        public async Task UploadResultFile(string fileName, byte[] data)
        {
            var experimentLabel = fileName;

            BlobServiceClient blobServiceClient = new BlobServiceClient(this.config.StorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(this.config.ResultContainer);

            // Write encoded data to text file
            byte[] testData = data;

            string blobName = experimentLabel;

            // Upload the text data to the blob container
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            using (MemoryStream memoryStream = new MemoryStream(testData))
            {
                await blobClient.UploadAsync(memoryStream);
            }

        }

        public async Task UploadResultFiles(string baseFileName, List<byte[]> resultImages)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(this.config.StorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(this.config.ResultContainer);

            for (int i = 0; i < resultImages.Count; i++)
            {
                string fileName = $"{baseFileName}_{i + 1}.png";  // Generate a unique file name for each image

                byte[] imageData = resultImages[i];

                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                using (MemoryStream memoryStream = new MemoryStream(imageData))
                {
                    await blobClient.UploadAsync(memoryStream);
                }
            }
        }


    }


}
