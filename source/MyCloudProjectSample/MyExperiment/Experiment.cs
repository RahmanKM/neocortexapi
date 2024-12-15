using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using MyExperiment.SEProject;
using System;
using System.Collections.Generic;
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

        public Experiment(IConfigurationSection configSection, IStorageProvider storageProvider, ILogger log)
        {
            this.storageProvider = storageProvider;
            this.logger = log;

            config = new MyConfig();
            configSection.Bind(config);
        }


        /// <summary>
        /// Executes the experiment using the provided input files and parameters.
        /// </summary>
        public async Task<IExperimentResult> RunAsync(string dateTimeFile, string scalarEncoderAQIFile, string value1, string value2, double value3)
        {
            ExperimentResult res = new ExperimentResult(this.config.GroupId, Guid.NewGuid().ToString());

            try
            {
                // Log the start time of the experiment
                res.StartTimeUtc = DateTime.UtcNow;
                logger?.LogInformation($"Experiment started at {res.StartTimeUtc}");

                // Initialize SdrToBitmap for encoding
                SdrToBitmap sdrToBitmap = new SdrToBitmap();

                // Step 1: Process and visualize the scalar encoder (value1)
                logger?.LogInformation("Encoding and visualizing scalar value (Value1)");
                byte[] scalarBitmap = sdrToBitmap.EncodeAndVisualizeSingleValueTest(value1);
                string scalarFileName = $"ScalarValue_{Guid.NewGuid()}.png";
                await storageProvider.UploadResultFile(scalarFileName, scalarBitmap);
                logger?.LogInformation($"Uploaded scalar visualization: {scalarFileName}");

                // Step 2: Generate 1D bitmap for visualization (value2)
                logger?.LogInformation("Encoding and visualizing 1D value (Value2)");
                byte[] bitmap1D = sdrToBitmap.EncodeAndVisualizeSingleValueTest3(value2);
                string bitmap1DFileName = $"1DValue_{Guid.NewGuid()}.png";
                await storageProvider.UploadResultFile(bitmap1DFileName, bitmap1D);
                logger?.LogInformation($"Uploaded 1D visualization: {bitmap1DFileName}");

                // Step 3: Process DateTime encoding from the input file
                logger?.LogInformation($"Processing DateTime encoding tests using file: {dateTimeFile}");
                await RunEncodeTestsAsync(dateTimeFile);

                // Step 4: Process AQI encoding from the input file
                logger?.LogInformation($"Processing AQI encoding tests using file: {scalarEncoderAQIFile}");
                await RunScalarAQITestsAsync(scalarEncoderAQIFile);

                // Step 5: Generate and visualize geospatial encoding
                logger?.LogInformation("Encoding and visualizing geospatial data");
                byte[] geoSpatialBitmap = sdrToBitmap.GeoSpatialEncoderTestDrawBitMap(value3);
                string geoSpatialFileName = $"GeoSpatial_{Guid.NewGuid()}.png";
                await storageProvider.UploadResultFile(geoSpatialFileName, geoSpatialBitmap);
                logger?.LogInformation($"Uploaded geospatial visualization: {geoSpatialFileName}");

                // Capture the end time and duration
                res.EndTimeUtc = DateTime.UtcNow;
                res.Duration = res.EndTimeUtc.Value - res.StartTimeUtc.Value;
                logger?.LogInformation($"Experiment completed. Duration: {res.Duration}");

                // Store the result files
                res.OutputFiles = JsonSerializer.Serialize(new string[] { scalarFileName, bitmap1DFileName, geoSpatialFileName });
                res.Description = "Experiment completed successfully";

                return res;
            }
            catch (Exception ex)
            {
                logger?.LogError($"Experiment failed: {ex.Message}");
                res.Description = "Experiment failed: " + ex.Message;
                return res;
            }
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
        /// <param name="value1">Third additional input data for EncodeAndVisualizeSingleValueTest method in the SDR to Bitmap conversion.</param>
        /// <param name="value2">Fourth additional input data for EncodeAndVisualizeSingleValueTest3 method in the SDR to Bitmap conversion.</param>
        /// <param name="value3">Third additional input data for GeoSpatialEncoderTestDrawBitMap method in the SDR to Bitmap conversion.</param>
        public async Task seProject(string dateTimeFileName, string aqiFileName, string value1, string value2, double value3)
        {
            // Generate bitmap with binary encoder
            SdrToBitmap sdrToBitmap = new SdrToBitmap();
            byte[] bitmapData = sdrToBitmap.EncodeAndVisualizeSingleValueTest(value1);

            // Upload the bitmap to the blob container
            string bitmapFileName = "EncodedValueVisualization_ScalarEncoder_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";
            await storageProvider.UploadResultFile(bitmapFileName, bitmapData);

            // Generate 1D Bitmap with binary encoder
            byte[] bitmapData1D = sdrToBitmap.EncodeAndVisualizeSingleValueTest3(value2);
            string bitmapFileName1D = "Draw1DBitmap_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";
            await storageProvider.UploadResultFile(bitmapFileName1D, bitmapData1D);

            // DateTime encoder test 
            await this.RunEncodeTestsAsync(dateTimeFileName);

            // ScalarEncoder AQI Test Bitmap run
            await this.RunScalarAQITestsAsync(aqiFileName);

            // GeoSpatial Data
            byte[] bitmapGeoSpatialData = sdrToBitmap.GeoSpatialEncoderTestDrawBitMap(value3);

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
                    "DataRow - W: {W}, R: {R}, Input: {Input}",
                    dataRow.W,
                    dataRow.R,
                    dataRow.Input
                );

                SdrToBitmap sdrToBitmap = new SdrToBitmap();
                byte[] result = sdrToBitmap.EncodeFullDateTimeTest(dataRow.W, dataRow.R, dataRow.Input);
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
            var dateTimeDataRows = JsonSerializer.Deserialize<Dictionary<string, List<DateTimeDataRow>>>(jsonFileName);

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
            var scalarEncoderDataRows = JsonSerializer.Deserialize<Dictionary<string, List<ScalarEncoderDataWithAQI>>>(jsonFileName);
            return scalarEncoderDataRows["ScalarEncoderDataWithAQI"];
        }


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
