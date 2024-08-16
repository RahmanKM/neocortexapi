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
        // Generate bitmap image for a randdom integer value
        public byte[] EncodeAndVisualizeSingleValueTest()
        {
            var encoderSettings = new Dictionary<string, object>
            {
                { "N", 156 }
            };

            var encoder = new BinaryEncoder(encoderSettings); // Assuming you have a BinaryEncoder class defined

            string inputValue = "50149";
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

        public byte[] EncodeAndVisualizeSingleValueTest3()
        {
            var encoderSettings = new Dictionary<string, object>
            {
                { "N", 156}
            };

            var encoder = new BinaryEncoder(encoderSettings);

            // Input value to encode. This is the value that will be converted into a binary representation.
            object inputValue = "56.7";

            // Encode the input value.
            var result = encoder.Encode(inputValue);

            // Drawing the bitmap with the calculated dimensions.
            // This call needs to match the signature and expectations of your implementation of 'DrawBitmap'.
            using (MemoryStream memoryStream = new MemoryStream()) 
            {
                return NeoCortexUtils1.Draw1DBitmap(result, 200);
            }

        }

        
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

        public List<byte[]> ScalarEncodingExperimentWithAQI(int[] inputs, double minValue, double maxValue)
        {
            DateTime now = DateTime.Now;

            ScalarEncoder encoder = new ScalarEncoder(new Dictionary<string, object>()
            {
                { "W", 21},
                { "N", 100},
                { "Radius", -1.0},
                { "MinVal", minValue},
                { "MaxVal", maxValue },
                { "Periodic", false},
                { "Name", "scalar"},
                { "ClipInput", false},
            });

            List<byte[]> resultImages = new List<byte[]>();

            foreach (int i in inputs)
            {
                var result = encoder.Encode(i);

                int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
                var twoDimArray = ArrayUtils.Transpose(twoDimenArray);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Draw the bitmap into the MemoryStream
                    NeoCortexUtils1.DrawBitmap(twoDimArray, 1024, 1024, memoryStream, Color.White, Color.Blue);

                    // Convert the MemoryStream to a byte array
                    byte[] imageBytes = memoryStream.ToArray();
                    resultImages.Add(imageBytes);
                }
            }

            // Return the list of byte arrays, each representing an image
            return resultImages;
        }

        public byte[] GeoSpatialEncoderTestDrawBitMap(double input = 48.75)
        {
            Dictionary<string, object> encoderSettings = new Dictionary<string, object>();
            encoderSettings.Add("W", 21);
            encoderSettings.Add("N", 40);
            encoderSettings.Add("MinVal", (double)48.75); // latitude value of Italy 
            encoderSettings.Add("MaxVal", (double)51.86);// latitude value of Germany
            encoderSettings.Add("Radius", (double)1.5);
            encoderSettings.Add("Periodic", (bool)false);
            encoderSettings.Add("ClipInput", (bool)true);
            encoderSettings.Add("IsRealCortexModel", false);

            GeoSpatialEncoderExperimental encoder = new GeoSpatialEncoderExperimental(encoderSettings);

            var result = encoder.Encode(input);// it use for encoding the input according to the given parameters.
            int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
            var twoDimArray = ArrayUtils.Transpose(twoDimenArray);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Draw the bitmap directly into the MemoryStream
                NeoCortexUtils1.DrawBitmap(twoDimArray, 1024, 1024, memoryStream, Color.Black, Color.Yellow);

                return memoryStream.ToArray();
            }

        }



        private static Dictionary<string, Dictionary<string, object>> getFullEncoderSettings()
        {
            Dictionary<string, Dictionary<string, object>> encoderSettings = new Dictionary<string, Dictionary<string, object>>();

            encoderSettings.Add("SeasonEncoder",
              new Dictionary<string, object>()
              {
                    { "W", 3},
                    { "N", 12},
                    //{ "Radius", 365/4},
                    { "MinVal", 1.0},
                    { "MaxVal", 367.0},
                    { "Periodic", true},
                    { "Name", "SeasonEncoder"},
                    { "ClipInput", true},
                    { "Offset", 100},
              }
              );

            encoderSettings.Add("DayOfWeekEncoder",
                new Dictionary<string, object>()
                {
                    { "W", 21},
                    { "N", 66},
                    { "MinVal", 0.0},
                    { "MaxVal", 7.0},
                    { "Periodic", false},
                    { "Name", "DayOfWeekEncoder"},
                    { "ClipInput", true},
                    { "Offset", 90},
                });

            encoderSettings.Add("WeekendEncoder", new Dictionary<string, object>()
                {
                    { "W", 21},
                    { "N", 42},
                    { "MinVal", 0.0},
                    { "MaxVal", 1.0},
                    { "Periodic", false},
                    { "Name", "WeekendEncoder"},
                    { "ClipInput", true},
                    { "Offset", 100},
                });


            encoderSettings.Add("DateTimeEncoder", new Dictionary<string, object>()
                {
                    { "W", 21},
                    { "N", 1024},
                    { "MinVal", DateTimeOffset.Now.AddYears(-4)},
                    { "MaxVal", DateTimeOffset.Now.AddYears(+1)},
                    { "Periodic", false},
                    { "Name", "DateTimeEncoder"},
                    { "ClipInput", true},
                    { "Offset", 100},
                });

            return encoderSettings;
        }

    }
}
