using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using LearningFoundation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using MyExperiment.SEProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyExperiment
{
    /// <summary>
    /// This class implements the ML experiment that will run in the cloud. This is refactored code from my SE project.
    /// </summary>
    public class Experiment : IExperiment
    {
        private IStorageProvider storageProvider;

        private ILogger logger;

        private MyConfig config;

        private string expectedProjectName;
        /// <summary>
        /// construct the class
        /// </summary>
        /// <param name="configSection"></param>
        /// <param name="storageProvider"></param>
        /// <param name="expectedPrjName"></param>
        /// <param name="log"></param>
        public Experiment(IConfigurationSection configSection, IStorageProvider storageProvider, string expectedPrjName, ILogger log)
        {
            this.storageProvider = storageProvider;
            this.logger = log;
            this.expectedProjectName = expectedPrjName;
            config = new MyConfig();
            configSection.Bind(config);
        }

        /// <summary>
        /// Executes the experiment process based on the provided input files and logs the results.
        /// This method configures the test parameters, performs the experiment, serializes the results to JSON,
        /// and captures execution details for auditing and debugging purposes.
        /// </summary>
        /// <param name="inputFile">The main input file name for the experiment.</param>
        /// <param name="sdrTestFile1">First additional test file used in the SDR to Bitmap conversion.</param>
        /// <param name="sdrTestFile2">Second additional test file used in the SDR to Bitmap conversion.</param>
        /// <returns>A task that returns an experiment result object encapsulating details such as start and end times, accuracy, and test data.</returns>
        public Task<IExperimentResult> Run(string inputFile, string sdrTestFile1, string sdrTestFile2)
        {
            // Generates a unique row key identifier for this experiment instance.
            Random rnd = new Random();
            int rowKeyNumber = rnd.Next(0, 1000);
            string rowKey = "rahman-cc-" + rowKeyNumber.ToString();

            // Initialize experiment result with configuration and unique identifiers.
            ExperimentResult res = new ExperimentResult(this.config.GroupId, rowKey);

            // Capture the start time of the experiment.
            res.StartTimeUtc = DateTime.UtcNow;
            res.ExperimentId = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            res.RowKey = rowKey;
            res.PartitionKey = "rahman-cc-proj-" + rowKey;
            res.TestName = "SDR to Bitmap";

            try
            {
                // Compiles test results from multiple input files into a structured format.
                var testResults = new
                {
                    inputFile,
                    sdrTestFile1,
                    sdrTestFile2
                };

                // Convert test results into JSON format for standardized reporting.
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(testResults, Newtonsoft.Json.Formatting.Indented);
                res.Description = json;
                this.logger?.LogInformation($"The file result we got: {json}");
                res.TestData = json;
                res.Accuracy = 100; // Assuming a fixed accuracy for demonstration
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, "Error processing the experiment.");
            }

            res.EndTimeUtc = DateTime.UtcNow;
            this.logger?.LogInformation("The process successfully completed.");
            return Task.FromResult<IExperimentResult>(res);
        }




        /// <inheritdoc/>
        public async Task RunQueueListener(CancellationToken cancelToken)
        {
            QueueClient queueClient = new QueueClient(this.config.StorageConnectionString, this.config.Queue);

            //
            // Implements the step 3 in the architecture picture.
            while (cancelToken.IsCancellationRequested == false)
            {
                QueueMessage message = await queueClient.ReceiveMessageAsync();

                if (message != null)
                {
                    try
                    {

                        string msgTxt = Encoding.UTF8.GetString(message.Body.ToArray());

                        this.logger?.LogInformation($"Received the message {msgTxt}");

                        // The message in the step 3 on architecture picture.
                        ExerimentRequestMessage request = JsonSerializer.Deserialize<ExerimentRequestMessage>(msgTxt);
                        this.logger?.LogInformation($"The real message {request.InputFile}");

                        // Step 4.
                        var inputFile = request.InputFile;
                        var dateTimeFile = request.DateTimeDataRow;
                        var scalarEncoderAQIFile = request.ScalarEncoderAQI;

                        // Here is your SE Project code started.(Between steps 4 and 5).
                        IExperimentResult result = await this.Run(inputFile, dateTimeFile, scalarEncoderAQIFile);

                        await this.seProject(dateTimeFile, scalarEncoderAQIFile);

                        // Step 4 (oposite direction)
                        //TODO. do serialization of the result.
                        //await storageProvider.UploadResultFile("outputfile.txt", null);
                        
                        await storageProvider.UploadExperimentResult(result);

                        await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                    }
                    catch (Exception ex)
                    {
                        this.logger?.LogError(ex, "TODO...");
                    }
                }
                else
                {
                    await Task.Delay(500);
                    logger?.LogTrace("Queue empty...");
                }
            }

            this.logger?.LogInformation("Cancel pressed. Exiting the listener loop.");
        }


        #region Methods
        /// <summary>
        /// Executes a series of encoding and visualization tests, generates bitmap images, 
        /// and uploads them to the cloud storage. This method runs multiple encoding tests including:
        /// 1. Encoding and visualizing a single scalar value.
        /// 2. Generating a 1D bitmap using a scalar encoder.
        /// 3. Running DateTime encoding tests.
        /// 4. Running scalar encoding tests with AQI data.
        /// 5. Generating and uploading geospatial data visualizations.
        /// </summary>
        /// <param name="dateTimeFileName">The JSON file name containing the DateTime data for encoding tests.</param>
        /// <param name="aqiFileName">The JSON file name containing the AQI data for scalar encoding tests.</param>
        public async Task seProject(string dateTimeFileName, string aqiFileName)
        {
            // Generate bitmap with binary encoder
            SdrToBitmap sdrToBitmap = new SdrToBitmap();
            byte[] bitmapData = sdrToBitmap.EncodeAndVisualizeSingleValueTest();

            // Upload the bitmap to the blob container
            string bitmapFileName = "EncodedValueVisualization_ScalarEncoder_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";
            await storageProvider.UploadResultFile(bitmapFileName, bitmapData);

            // Generate 1D Bitmap with binary encoder
            byte[] bitmapData1D = sdrToBitmap.EncodeAndVisualizeSingleValueTest3();
            string bitmapFileName1D = "Draw1DBitmap_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";
            await storageProvider.UploadResultFile(bitmapFileName1D, bitmapData1D);

            // DateTime encoder test 
            await this.RunEncodeTestsAsync(dateTimeFileName);

            // ScalarEncoder AQI Test Bitmap run
            await this.RunScalarAQITestsAsync(aqiFileName);

            // GeoSpatial Data
            byte[] bitmapGeoSpatialData = sdrToBitmap.GeoSpatialEncoderTestDrawBitMap();

            // Upload the bitmap to the blob container
            string bitmapGeoSpatialFileName = "GeoSpatialBitmap_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";
            await storageProvider.UploadResultFile(bitmapGeoSpatialFileName, bitmapGeoSpatialData);
        }

        /// <summary>
        /// Runs the DateTime encoding tests by processing a list of data rows extracted from a JSON file.
        /// For each data row, the method encodes the DateTime, generates a bitmap image, and uploads the image to cloud storage.
        /// The details of each data row are logged for debugging and auditing purposes.
        /// </summary>
        /// <param name="jsonFileName">The JSON file name containing DateTime data rows.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task RunEncodeTestsAsync(string jsonFileName)
        {
            var dataRows = await GetDateTimeDataRowsAsync(jsonFileName);

            foreach (var dataRow in dataRows)
            {
                this.logger?.LogInformation(
                    "DataRow - W: {W}, R: {R}, Input: {Input}, ExpectedOutput: {ExpectedOutput}",
                    dataRow.W,
                    dataRow.R,
                    dataRow.Input,
                    string.Join(", ", dataRow.ExpectedOutput)
                );

                SdrToBitmap sdrToBitmap = new SdrToBitmap();
                byte[] result = sdrToBitmap.EncodeFullDateTimeTest(dataRow.W, dataRow.R, dataRow.Input, dataRow.ExpectedOutput);
                string fileName = "DateTimeBitMap_" + dataRow.Input + "_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";

                await storageProvider.UploadResultFile(fileName, result);
            }
        }

        /// <summary>
        /// Runs the scalar encoding tests using AQI data by processing a list of data rows extracted from a JSON file.
        /// For each data row, the method encodes the AQI values, generates multiple bitmap images, and uploads each image to cloud storage.
        /// The details of each data row are logged for debugging and auditing purposes.
        /// </summary>
        /// <param name="jsonFileName">The JSON file name containing AQI data rows.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task RunScalarAQITestsAsync(string jsonFileName)
        {
            var dataRows = await GetScalarEncoderDataWithAQI(jsonFileName);

            foreach (var dataRow in dataRows)
            {
                this.logger?.LogInformation(
                    "DataRow - Inputs: {Inputs}, MinValue: {MinValue}, MaxValue: {MaxValue}",
                    string.Join(", ", dataRow.Inputs),
                    dataRow.MinValue,
                    dataRow.MaxValue
                );

                SdrToBitmap sdrToBitmap = new SdrToBitmap();
                List<byte[]> results = sdrToBitmap.ScalarEncodingExperimentWithAQI(dataRow.Inputs, dataRow.MinValue, dataRow.MaxValue);

                foreach (var result in results)
                {
                    string fileName = "ScalarAQIBitmap_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";
                    await storageProvider.UploadResultFile(fileName, result);
                }
            }
        }

        /// <summary>
        /// Retrieves and deserializes DateTime data rows from a JSON file stored in cloud storage.
        /// Logs the deserialized and formatted JSON data for debugging purposes.
        /// </summary>
        /// <param name="jsonFileName">The JSON file name to be downloaded and deserialized.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of a list of DateTimeDataRow objects.</returns>
        public async Task<List<DateTimeDataRow>> GetDateTimeDataRowsAsync(string jsonFileName)
        {
            string jsonString = await storageProvider.DownloadInputFile(jsonFileName);
            var dateTimeDataRows = JsonSerializer.Deserialize<Dictionary<string, List<DateTimeDataRow>>>(jsonString);

            var formattedJson = JsonSerializer.Serialize(dateTimeDataRows, new JsonSerializerOptions { WriteIndented = true });
            this.logger?.LogInformation("Deserialized and formatted JSON data: {FormattedJson}", formattedJson);

            return dateTimeDataRows["DateTimeDataRow"];
        }

        /// <summary>
        /// Retrieves and deserializes scalar encoder data with AQI from a JSON file stored in cloud storage.
        /// </summary>
        /// <param name="jsonFileName">The JSON file name to be downloaded and deserialized.</param>
        /// <returns>A Task representing the asynchronous operation, with a result of a list of ScalarEncoderDataWithAQI objects.</returns>
        public async Task<List<ScalarEncoderDataWithAQI>> GetScalarEncoderDataWithAQI(string jsonFileName)
        {
            string jsonString = await storageProvider.DownloadInputFile(jsonFileName);
            var scalarEncoderDataRows = JsonSerializer.Deserialize<Dictionary<string, List<ScalarEncoderDataWithAQI>>>(jsonString);
            return scalarEncoderDataRows["ScalarEncoderDataWithAQI"];
        }

        /// <summary>
        /// Represents a data row used in DateTime encoding tests.
        /// Contains the width (W), radius (R), input DateTime string, and expected output array.
        /// </summary>
        public class DateTimeDataRow
        {
            public int W { get; set; }
            public double R { get; set; }
            public string Input { get; set; }
            public int[] ExpectedOutput { get; set; }
        }

        /// <summary>
        /// Represents a data row used in scalar encoding tests with AQI data.
        /// Contains the input values, minimum value, and maximum value.
        /// </summary>
        public class ScalarEncoderDataWithAQI
        {
            public int[] Inputs { get; set; }
            public double MinValue { get; set; }
            public double MaxValue { get; set; }
        }


        #endregion
    }
}
