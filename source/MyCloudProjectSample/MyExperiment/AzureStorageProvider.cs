using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using System;
using System.Collections.Generic;
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

        public Task<string> DownloadInputAsync(string fileName)
        {
            throw new NotImplementedException();
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


        public Task UploadExperimentResult(IExperimentResult result)
        {
            throw new NotImplementedException();
        }

        public Task UploadResultAsync(string experimentName, IExperimentResult result)
        {
            throw new NotImplementedException();
        }
    }


}
