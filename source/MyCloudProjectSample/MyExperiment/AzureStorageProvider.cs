using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyExperiment
{
    public class AzureStorageProvider : IStorageProvider
    {
        private MyConfig _config;
        private ILogger logger;

        public AzureStorageProvider(IConfigurationSection configSection)
        {
            _config = new MyConfig();
            configSection.Bind(_config);
        }

        /// <summary>
        /// Commits the experiment request by deleting the message from the queue.
        /// </summary>
        /// <param name="request">The experiment request to commit.</param>
        public async Task CommitRequestAsync(IExperimentRequest request) 
        {
            var queueClient = new QueueClient(_config.StorageConnectionString, _config.Queue);

            if (string.IsNullOrWhiteSpace(request.MessageId) || string.IsNullOrWhiteSpace(request.PopReceipt))
            {
                throw new ArgumentException("Invalid MessageId or Invalid pop receipt.");
            }

            try
            {
                await queueClient.DeleteMessageAsync(request.MessageId, request.PopReceipt);
                Console.WriteLine("Message Successfully Deleted.");
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Failed to delete message: {ex.Message}");
            }
        }

        /// <summary>
        /// Downloads the content of a specified file from an Azure Blob Storage container.
        /// </summary>
        /// <param name="fileName">The name of the file to download from the blob storage.</param>
        /// <returns>
        /// A string containing the contents of the file if it exists in the blob storage.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// Thrown when the specified file does not exist in the blob container.
        /// </exception>
        public async Task<string> DownloadInputAsync(string fileName)
        {
            // Create a connection to the specified blob container using configuration settings.
            BlobContainerClient container = new BlobContainerClient(this._config.StorageConnectionString, this._config.TrainingContainer);

            // Ensure the blob container exists; create it if it does not.
            await container.CreateIfNotExistsAsync();

            // Retrieve a reference to the blob using the provided file name.
            BlobClient blob = container.GetBlobClient(fileName);

            // Check if the specified blob exists in the container.
            if (await blob.ExistsAsync())
            {
                // Download the blob's content as a stream.
                BlobDownloadInfo download = await blob.DownloadAsync();

                // Read the contents of the file using a StreamReader.
                using (StreamReader reader = new StreamReader(download.Content))
                {
                    // Read the entire file content asynchronously and return it as a string.
                    string fileContent = await reader.ReadToEndAsync();
                    return fileContent;
                }
            }
            else
            {
                // Throw an exception if the specified file does not exist in the blob container.
                throw new FileNotFoundException($"'{fileName}' could not be found.");
            }
        }


        /// <summary>
        /// Receives an experiment request from the Azure queue asynchronously.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation, returning the experiment request received from the queue.</returns>
        public async Task<IExperimentRequest> ReceiveExperimentRequestAsync(CancellationToken token)
        {
            QueueClient queueClient = new QueueClient(_config.StorageConnectionString, _config.Queue);

            // Receive a message from the queue
            QueueMessage message = await queueClient.ReceiveMessageAsync();

            if (message != null)
            {
                try
                {
                    // Process the received message
                    string msgTxt = Encoding.UTF8.GetString(message.Body.ToArray());
                    ExerimentRequestMessage request = JsonSerializer.Deserialize<ExerimentRequestMessage>(msgTxt);
                    request.MessageId = message.MessageId;
                    request.PopReceipt = message.PopReceipt;
                    return request;
                }
                catch (JsonException jsonEx)
                {
                    logger?.LogError(jsonEx, "An error occured while deserializing the queue message");
                    Console.Error.WriteLine("Queue message is corrupted.");
                }
            }
            else
            {
                logger?.LogInformation("Queue message null");
            }

            return null;
        }

        /// <summary>
        /// Uploads a single result file to Azure Blob Storage.
        /// </summary>
        /// <param name="fileName">The name of the file to upload.</param>
        /// <param name="data">The data to upload in byte array format.</param>
        public async Task UploadResultFile(string fileName, byte[] data)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(this._config.StorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(this._config.ResultContainer);

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Prepare the data for uploading
            using (MemoryStream memoryStream = new MemoryStream(data)) 
            {
                await blobClient.UploadAsync(memoryStream);
            }
        }

        public async Task UploadResultAsync(IExperimentResult result)
        {
            string rowKey = Guid.NewGuid().ToString("N");
            string partitionKey = "rahman-cc";

            var testResult = new ExperimentResult(partitionKey, rowKey)
            {
                ExperimentId = result.ExperimentId,
                Description = result.Description,
                StartTimeUtc = result.StartTimeUtc,
                EndTimeUtc = result.EndTimeUtc,
                OutputFiles = result.OutputFiles,
                Duration = result.Duration,
            };

            Console.WriteLine($"Upload ExperimentResult to table: {this._config.ResultTable}");
            var client = new TableClient(this._config.StorageConnectionString, this._config.ResultTable);

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
    }


}
