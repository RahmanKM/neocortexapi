using NeoCortexApi.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                { "N", 156}
            };

            var encoder = new BinaryEncoder(encoderSettings);

            // Input value to encode. This is the value that will be converted into a binary representation.
            string inputValue = "50149";

            // Encode the input value.
            var result = encoder.Encode(inputValue);

            // Preparing for visualization by converting the 1D result array into a 2D array for the 'DrawBitmap' method.
            int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
            var twoDimArray = ArrayUtils.Transpose(twoDimenArray);

            // Create the bitmap in-memory and return the byte array
            using (var memoryStream = new MemoryStream())
            {
                // Drawing the bitmap in memory
                NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, memoryStream, Color.Black, Color.Yellow);

                // Return the bitmap data as a byte array
                return memoryStream.ToArray();
            }
        }
    }
}
