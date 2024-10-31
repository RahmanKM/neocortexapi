using NeoCortexApi.Encoders;
using NeoCortexApi.Network;
using NeoCortexApi.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment.SEProject
{

    public class SdrToBitmap
    {
        /// <summary>
        /// Encodes a given single numeric value into a Sparse Distributed Representation (SDR) and visualizes this representation as a bitmap.
        /// This method is designed to work with numerical data, transforming it into a binary format that can be easily visualized for analysis.
        /// The resulting binary pattern is rendered as a black and yellow bitmap, where active bits are represented by yellow pixels on a black background.
        /// </summary>
        /// <returns>A byte array containing the bitmap data of the encoded numeric value, formatted as a PNG file.</returns>
        public byte[] EncodeAndVisualizeSingleValueTest(string value)
        {
            var encoderSettings = new Dictionary<string, object>
            {
                { "N", 156 }
            };

            var encoder = new BinaryEncoder(encoderSettings); // Assuming you have a BinaryEncoder class defined

            string inputValue = value;
            var result = encoder.Encode(inputValue);

            int[,] twoDimArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
            twoDimArray = ArrayUtils.Transpose(twoDimArray);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Draw the bitmap directly into the MemoryStream
                NeoCortexUtils1.DrawBitmap(twoDimArray, 1024, 1024, memoryStream, Color.Black, Color.Yellow);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Encodes a single input value into a sparse distributed representation (SDR) using a binary encoder
        /// and visualizes the result as a 1D bitmap. The encoded value is then returned as a byte array in PNG format.
        /// </summary>
        /// <returns>
        /// A byte array containing the generated 1D bitmap image in PNG format, representing the encoded input value.
        /// </returns>
        public byte[] EncodeAndVisualizeSingleValueTest3(string value)
        {
            // Configure the binary encoder settings. 
            // 'N' specifies the number of bits in the SDR representation.
            var encoderSettings = new Dictionary<string, object>
            {
                { "N", 156 }  // 'N' defines the number of bits in the encoded SDR.
            };

            // Initialize the BinaryEncoder with the specified settings.
            var encoder = new BinaryEncoder(encoderSettings);

            // The input value to encode. This is the value that will be converted into a binary representation.
            object inputValue = value;

            // Encode the input value into an SDR (sparse distributed representation).
            var result = encoder.Encode(inputValue);

            // Draw the 1D bitmap using the encoded SDR.
            // The result is saved into a MemoryStream and returned as a byte array in PNG format.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // The 'Draw1DBitmap' method visualizes the 1D SDR.
                return NeoCortexUtils1.Draw1DBitmap(result, 200);
            }
        }

        /// <summary>
        /// Encodes a given date-time value into a sparse distributed representation (SDR) and visualizes it as a 2D bitmap image.
        /// The encoded date-time is transformed into a 2D array of integers, where each value represents the state of a cell in the bitmap.
        /// The resulting bitmap is drawn directly into a memory stream and returned as a byte array in PNG format.
        /// </summary>
        /// <param name="w">
        /// A parameter representing the width of the encoder's internal structure. 
        /// This parameter may affect the overall structure and output of the encoded SDR.
        /// </param>
        /// <param name="r">
        /// A parameter representing the resolution or range for the encoder. 
        /// This can influence the precision of the encoding, affecting how different date-time values are represented.
        /// </param>
        /// <param name="input">
        /// The input date-time value to be encoded. This should be provided as an object, 
        /// which will be parsed into a DateTimeOffset for encoding purposes.
        /// </param>
        /// <param name="expectedOutput">
        /// An array of integers representing the expected output SDR, which is used to compare and verify the encoding process.
        /// </param>
        /// <returns>
        /// A byte array containing the generated 2D bitmap image in PNG format, which visually represents the encoded date-time value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the length of the encoded SDR is an odd number, which is a limitation for 2D drawing.
        /// The encoder's offset properties must be set to ensure the SDR length is an even number.
        /// </exception>
        public byte[] EncodeFullDateTimeTest(int w, double r, Object input, int[] expectedOutput)
        {
            CortexNetworkContext ctx = new CortexNetworkContext();

            var now = DateTimeOffset.Now;

            Dictionary<string, Dictionary<string, object>> encoderSettings = getFullEncoderSettings();

            var encoder = new DateTimeEncoder(encoderSettings, DateTimeEncoder.Precision.Days);

            var result = encoder.Encode(DateTimeOffset.Parse(input.ToString()));

            // This is a limitation for 2D drawing only.
            if ((result.Length % 2) != 0)
                throw new ArgumentException("Only odd number of bits is allowed. Please set Offset pproperty of all encoders to odd value.");

            Debug.WriteLine(NeoCortexApi.Helpers.StringifyVector(result));

            int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
            var twoDimArray = ArrayUtils.Transpose(twoDimenArray);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Draw the bitmap directly into the MemoryStream
                NeoCortexUtils1.DrawBitmap(twoDimArray, 1024, 1024, memoryStream, Color.Black, Color.Yellow);

                return memoryStream.ToArray();
            }

        }

        /// <summary>
        /// Performs an experiment using the ScalarEncoder to encode a series of input values related to Air Quality Index (AQI),
        /// visualizes each encoded result as a bitmap, and returns a list of images represented as byte arrays in PNG format.
        /// </summary>
        /// <param name="inputs">An array of integer input values to be encoded, representing different AQI levels or other scalar data.</param>
        /// <param name="minValue">The minimum possible value for the input range that the encoder will handle.</param>
        /// <param name="maxValue">The maximum possible value for the input range that the encoder will handle.</param>
        /// <returns>
        /// A list of byte arrays where each byte array represents a PNG image of the encoded input value, visualized as a 2D bitmap.
        /// </returns>
        public List<byte[]> ScalarEncodingExperimentWithAQI(int[] inputs, double minValue, double maxValue)
        {
            // Capture the current time (not used further in this method but can be logged or used for debugging).
            DateTime now = DateTime.Now;

            // Initialize the ScalarEncoder with the specified configuration.
            ScalarEncoder encoder = new ScalarEncoder(new Dictionary<string, object>()
            {
                { "W", 21},                // Width of the SDR (number of active bits).
                { "N", 100},               // Total number of bits in the SDR.
                { "Radius", -1.0},         // The radius (sensitivity) for encoding values.
                { "MinVal", minValue},     // The minimum value that the encoder should handle.
                { "MaxVal", maxValue },    // The maximum value that the encoder should handle.
                { "Periodic", false},      // Indicates whether the encoded space is periodic.
                { "Name", "scalar"},       // Name of the encoder.
                { "ClipInput", false},     // Determines whether to clip the input value within the min/max range.
            });

            // Create a list to store the resulting images as byte arrays.
            List<byte[]> resultImages = new List<byte[]>();

            // Loop through each input value, encode it, and generate the corresponding bitmap image.
            foreach (int i in inputs)
            {
                // Encode the input value into an SDR (Sparse Distributed Representation).
                var result = encoder.Encode(i);

                // Convert the 1D SDR array into a 2D array for visualization.
                int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
                var twoDimArray = ArrayUtils.Transpose(twoDimenArray);

                // Create a MemoryStream to store the generated bitmap image.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Draw the 2D SDR as a bitmap image into the MemoryStream.
                    NeoCortexUtils1.DrawBitmap(twoDimArray, 1024, 1024, memoryStream, Color.White, Color.Blue);

                    // Convert the MemoryStream into a byte array representing the PNG image.
                    byte[] imageBytes = memoryStream.ToArray();

                    // Add the byte array to the list of result images.
                    resultImages.Add(imageBytes);
                }
            }

            // Return the list of byte arrays, each representing a visualized encoded input.
            return resultImages;
        }


        /// <summary>
        /// Tests the GeoSpatialEncoder by encoding a given latitude input value and visualizing the encoded result as a bitmap image.
        /// The bitmap is returned as a byte array in PNG format.
        /// </summary>
        /// <param name="input">
        /// The latitude value to be encoded. 
        /// The default value is 48.75, which corresponds to a specific geographic location (Italy).
        /// </param>
        /// <returns>
        /// A byte array representing the encoded latitude value visualized as a 1024x1024 PNG bitmap.
        /// </returns>
        public byte[] GeoSpatialEncoderTestDrawBitMap(double inputValue)
        {
            // Create a dictionary to hold the encoder settings.
            Dictionary<string, object> encoderSettings = new Dictionary<string, object>
            {
                { "W", 21 },                 // Width of the SDR (number of active bits).
                { "N", 40 },                 // Total number of bits in the SDR.
                { "MinVal", 48.75 },         // Minimum latitude value to encode, set to Italy's latitude.
                { "MaxVal", 51.86 },         // Maximum latitude value to encode, set to Germany's latitude.
                { "Radius", 1.5 },           // Radius or sensitivity for encoding latitude values.
                { "Periodic", false },       // Indicates if the latitude range should be considered periodic.
                { "ClipInput", true },       // Determines whether to clip the input value within the min/max range.
                { "IsRealCortexModel", false } // Indicates whether to use a real cortex model (not used in this context).
            };

            // Initialize the GeoSpatialEncoder with the specified settings.
            GeoSpatialEncoderExperimental encoder = new GeoSpatialEncoderExperimental(encoderSettings);

            // Encode the input latitude value into an SDR (Sparse Distributed Representation).
            var result = encoder.Encode(inputValue);

            // Convert the 1D SDR array into a 2D array for visualization.
            int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
            var twoDimArray = ArrayUtils.Transpose(twoDimenArray);

            // Create a MemoryStream to store the generated bitmap image.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Draw the 2D SDR as a bitmap image into the MemoryStream.
                NeoCortexUtils1.DrawBitmap(twoDimArray, 1024, 1024, memoryStream, Color.Black, Color.Yellow);

                // Return the MemoryStream as a byte array representing the PNG image.
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Generates a dictionary of encoder settings used to configure various encoders for different aspects of date and time.
        /// The encoders include configurations for seasons, days of the week, weekends, and specific date-time values.
        /// These settings are used to create Sparse Distributed Representations (SDRs) for the corresponding inputs.
        /// </summary>
        /// <returns>
        /// A dictionary where each key represents an encoder name, and the corresponding value is another dictionary containing the settings for that encoder.
        /// </returns>
        private static Dictionary<string, Dictionary<string, object>> getFullEncoderSettings()
        {
            // Initialize the main dictionary that will hold the settings for all encoders.
            Dictionary<string, Dictionary<string, object>> encoderSettings = new Dictionary<string, Dictionary<string, object>>();

            // Add settings for the "SeasonEncoder" which encodes the season of the year.
            encoderSettings.Add("SeasonEncoder",
              new Dictionary<string, object>()
              {
                { "W", 3},                // Number of active bits in the encoded representation.
                { "N", 12},               // Total number of bits in the representation.
                { "MinVal", 1.0},         // Minimum value (representing the start of the season).
                { "MaxVal", 367.0},       // Maximum value (slightly more than the number of days in a year, allowing for seasonality).
                { "Periodic", true},      // Indicates that the value wraps around (cyclical nature of seasons).
                { "Name", "SeasonEncoder"}, // Name of the encoder.
                { "ClipInput", true},     // Ensures the input is clipped within the MinVal and MaxVal range.
                { "Offset", 100},         // Offset used for aligning this encoder with others.
              }
            );

            // Add settings for the "DayOfWeekEncoder" which encodes the day of the week.
            encoderSettings.Add("DayOfWeekEncoder",
                new Dictionary<string, object>()
                {
                    { "W", 21},               // Number of active bits in the encoded representation.
                    { "N", 66},               // Total number of bits in the representation.
                    { "MinVal", 0.0},         // Minimum value (representing Sunday).
                    { "MaxVal", 7.0},         // Maximum value (representing Saturday).
                    { "Periodic", false},     // Day of the week is not cyclical in this context.
                    { "Name", "DayOfWeekEncoder"}, // Name of the encoder.
                    { "ClipInput", true},     // Ensures the input is clipped within the MinVal and MaxVal range.
                    { "Offset", 90},          // Offset used for aligning this encoder with others.
                });

            // Add settings for the "WeekendEncoder" which encodes whether a day is a weekend or not.
            encoderSettings.Add("WeekendEncoder", new Dictionary<string, object>()
            {
                { "W", 21},               // Number of active bits in the encoded representation.
                { "N", 42},               // Total number of bits in the representation.
                { "MinVal", 0.0},         // Minimum value (representing a weekday).
                { "MaxVal", 1.0},         // Maximum value (representing a weekend).
                { "Periodic", false},     // Not periodic; binary classification.
                { "Name", "WeekendEncoder"}, // Name of the encoder.
                { "ClipInput", true},     // Ensures the input is clipped within the MinVal and MaxVal range.
                { "Offset", 100},         // Offset used for aligning this encoder with others.
            });

            // Add settings for the "DateTimeEncoder" which encodes specific date-time values.
            encoderSettings.Add("DateTimeEncoder", new Dictionary<string, object>()
            {
                { "W", 21},               // Number of active bits in the encoded representation.
                { "N", 1024},             // Total number of bits in the representation.
                { "MinVal", DateTimeOffset.Now.AddYears(-4)}, // Minimum value (4 years ago from now).
                { "MaxVal", DateTimeOffset.Now.AddYears(+1)}, // Maximum value (1 year in the future).
                { "Periodic", false},     // Not periodic; linear time progression.
                { "Name", "DateTimeEncoder"}, // Name of the encoder.
                { "ClipInput", true},     // Ensures the input is clipped within the MinVal and MaxVal range.
                { "Offset", 100},         // Offset used for aligning this encoder with others.
            });

            // Return the dictionary containing all the encoder settings.
            return encoderSettings;
        }


    }
}
