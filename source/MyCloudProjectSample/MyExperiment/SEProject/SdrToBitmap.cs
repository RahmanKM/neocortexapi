using NeoCortexApi.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment.SEProject
{
    public class SdrToBitmap
    {
        public void EncodeAndVisualizeSingleValueTest1()
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

            // Specify the file path for the output image.
            string filePath = "EncodedValueVisualization-18_112.png";

            // Drawing the bitmap with the calculated dimensions.
            // This call needs to match the signature and expectations of your implementation of 'DrawBitmap'.
            NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, filePath, Color.Black, Color.Yellow);

            // Ensure your 'DrawBitmap' method is prepared to handle the dimensions and scaling correctly.
        }
    }
}
