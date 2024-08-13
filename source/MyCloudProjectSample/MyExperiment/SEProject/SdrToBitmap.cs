using NeoCortexApi.Utility;
using System;
using System.Collections.Generic;
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

    }
}
