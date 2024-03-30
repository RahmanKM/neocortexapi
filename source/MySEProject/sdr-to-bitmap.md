# Bitmap Representation of Sparse Distributed Representations (SDRs)

This document outlines the method for visualizing Sparse Distributed Representations (SDRs) as bitmap images using the `DrawBitmap` method. Visualizing SDRs can significantly aid in understanding the patterns and information encoded within them, especially when dealing with complex encoders or spatial pooling processes. This method provides a tangible way to assess and interpret the activity and structure of SDRs, facilitating a deeper understanding of their functionality in various applications.

## Introduction

### What is SDR:

Sparse Distributed Representation (SDR) is a concept used in computing that mirrors the brain's method of processing information. Essentially, it's a way of storing data where most of the bits are off (0), and only a few are on (1). This setup allows for handling a wide variety of information efficiently and robustly, much like how the brain operates with its neurons.

### What is Bitmap:

Bitmap is a type of file format which is used to store images. A bitmap is a spatially mapped array of bits i.e. a map of bits. For the purpose of representing SDRs as bitmaps, we first feed the output of encoders as inputs to the SP.

### The DrawBitmap Method

The `DrawBitmap` method offers a versatile approach to represent SDRs visually, enabling the examination of their properties and behaviors. By converting SDRs into bitmap images, we can observe the activation patterns and interactions within the data, providing insights into the encoding and processing mechanisms at play. This document serves as a guide to utilizing the `DrawBitmap` method across different scenarios, highlighting its application with simple SDR examples as well as encoder-generated SDRs.

### Syntax

```csharp
void DrawBitmap(int[,] twoDimArray, int width, int height, string filePath, Color inactiveCellColor, Color activeCellColor, string text = null)
```

### Parameters

- `int[,] twoDimArray`: The SDR represented as a two-dimensional array of active columns.
- `int width`: The desired output width of the bitmap.
- `int height`: The desired output height of the bitmap.
- `string filePath`: The path where the bitmap PNG file will be saved.
- `Color inactiveCellColor`: The color used to represent inactive cells.
- `Color activeCellColor`: The color used to represent active cells.
- `string text`: Optional text to be included with the bitmap image.

### Description

The `DrawBitmap` method transforms a two-dimensional array representing an SDR into a visual bitmap image. By specifying the dimensions, colors, and additional text, users can customize the visualization to suit their analysis needs. The method scales the SDR array to fit the specified bitmap dimensions, allowing for a clear and adjustable representation of the SDR's structure.

Turning an SDR into a visual bitmap involves a few straightforward steps:

1. **Setting the Scene**:
   - Determine the `width` and `height` for the bitmap, scaling the image to fit your needs.
   - Choose colors for the active and inactive cells to make the SDR's structure clear.

2. **Calculating the Scale**:
   - A scale factor is calculated based on the ratio of the bitmap's width to the SDR array's width. This helps adjust the cell sizes in the bitmap to fit the entire SDR.

3. **Drawing the Bitmap**:
   - Go through each cell in the SDR:
     - Color it with the active cell color if it's active (`1`).
     - Use the inactive cell color if it's inactive (`0`).
   - The scale factor ensures each cell in the bitmap represents the SDR accurately.

4. **Saving the drawn bitmaps**:
   - Once every cell is colored, the bitmap is saved to the location specified in `filePath`.

This method simplifies analyzing and understanding SDR patterns by providing a visual representation.

## Examples

### Basic SDR Examples with binary encoders
#### 1. Example of visualizing a Number

Let's visualize a basic SDR with a pattern of activation. This simple example will help understand the visualization process. So for this we can take a simple value say ```40148```. Now we need SDR encode this data in order to visualize.
```csharp
            // This snippet creates a dictionary, encoderSettings, to hold the configuration parameters for the encoder. The dictionary contains key-value pairs where each key is a setting name,and the associated value               // is the setting's value. In this case, the only parameter specified is "N", set to 156. The parameter "N" represents the size of the output encoded vector, 
            var encoderSettings = new Dictionary<string, object>
            {
                { "N", 156},
            };

            // Here, a BinaryEncoder instance is created with the previously defined encoderSettings. The BinaryEncoder utilizes these settings to determine how to encode input values into binary format.
            // The size of the encoded output is determined by the "N" parameter in the settings,    
            var encoder = new BinaryEncoder(encoderSettings);

            // Input value to encode. This is the value that will be converted into a binary representation.
            string inputValue = "40148";

            // Encode the input value.
            var result = encoder.Encode(inputValue);
```  

Now we can make this result 2D as this is now 1 dimension and we need two dimension for drawing the bitmap.
```csharp
// converts the one-dimensional array result into a two-dimensional array twoDimenArray. The ArrayUtils.Make2DArray method is used for this conversion,
// where result is the source array. The dimensions for the new 2D array are determined by the square root of the length of result, suggesting that the original data is reshaped into a square matrix
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
// ArrayUtils.Transpose method reorganizes twoDimenArray by flipping it over its diagonal, effectively swapping its rows and columns.
var twoDimArray = ArrayUtils.Transpose(twoDimenArray);
```
Now we can draw this data in the DrawBitMap method. 
```csharp
// DrawBitMap method is called with the 2D array we made, then we pass the width and the height consecutively which is 1024 for bitmap drawing,
// and we set the inactive bits to black by passing the Color.Black, and active bits to yellow.
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, filePath, Color.Black, Color.Yellow);
```
We can get the below image from the DrawBitMap method

![EncodedValueVisualization-18_112](https://github.com/TanzeemHasan/neocortexapi/assets/74203937/9bfc6c35-925a-41cc-95db-50f494a8cedd)


#### 2. Example of visualizing a number
For this example we will take a random integer value ```50149```. We will encode the value by setting up the sdr encoder settings and encode the value with that settings. and also set up the two dimensional array
```csharp
            // This snippet creates a dictionary, encoderSettings, to hold the configuration parameters for the encoder. The dictionary contains key-value pairs where each key is a setting name,and the associated value               // is the setting's value. In this case, the only parameter specified is "N", set to 156. The parameter "N" represents the size of the output encoded vector, 
            var encoderSettings = new Dictionary<string, object>
            {
                { "N", 156},
            };

            // Here, a BinaryEncoder instance is created with the previously defined encoderSettings. The BinaryEncoder utilizes these settings to determine how to encode input values into binary format.
            // The size of the encoded output is determined by the "N" parameter in the settings,    
            var encoder = new BinaryEncoder(encoderSettings);

            // Input value to encode. This is the value that will be converted into a binary representation.
            string inputValue = "50149";

            // Encode the input value.
            var result = encoder.Encode(inputValue);

            // converts the one-dimensional array result into a two-dimensional array twoDimenArray. The ArrayUtils.Make2DArray method is used for this conversion,
            // where result is the source array. The dimensions for the new 2D array are determined by the square root of the length of result, suggesting that the original data is reshaped into a square matrix
            int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));

            // ArrayUtils.Transpose method reorganizes twoDimenArray by flipping it over its diagonal, effectively swapping its rows and columns.
            var twoDimArray = ArrayUtils.Transpose(twoDimenArray);
```
Now lets draw this using DrawBitMap method
```csharp
// DrawBitMap method is called with the 2D array we made, then we pass the width and the height consecutively which is 1024 for bitmap drawing,
// and we set the inactive bits to black by passing the Color.Black, and active bits to yellow.
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, "EncodedValueVisualization-45_67.png", Color.Black, Color.Yellow);
```
It returns the below picture,

![EncodedValueVisualization-50148](https://github.com/TanzeemHasan/neocortexapi/assets/74203937/16ccc118-d5f6-494c-a9c4-71a5ab86c50d)

The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/44772a45ac31c48e74a648ca9b1386fb82520590/source/UnitTestsProject/EncoderTests/DateTimeEncoderTests.cs#L116)

### DrawBitMap example with Binary Encoder 1D image

We can also use DrawBitMap method for generating 1D images by getting the binary encoded value from our input. 

```csharp
            // This snippet creates a dictionary, encoderSettings, to hold the configuration parameters for the encoder. The dictionary contains key-value pairs where each key is a setting name,and the associated value               // is the setting's value. In this case, the only parameter specified is "N", set to 156. The parameter "N" represents the size of the output encoded vector, 
            var encoderSettings = new Dictionary<string, object>
            {
                { "N", 156},
            };

            // Here, a BinaryEncoder instance is created with the previously defined encoderSettings. The BinaryEncoder utilizes these settings to determine how to encode input values into binary format.
            // The size of the encoded output is determined by the "N" parameter in the settings,    
            var encoder = new BinaryEncoder(encoderSettings);

            // Input value to encode. This is the value that will be converted into a binary representation.
            string inputValue = "50149";

            // Encode the input value.
            var result = encoder.Encode(inputValue);
```
Now we can create a method which can generate Images from the 1D sdrs from the binary encoder and make the data suitable for the DrawBitMap method to generate a 1D Image
```csharp

// We can call our created method and pass the sdrs named result, filepath and a sample scale vale of 200
NeoCortexUtils.Draw1DBitmap(result, filePath, 200);
```

Here is the Modified Draw1DBitmap
```csharp
        /// <summary>
        /// Draws a 1D bitmap from an array of values.
        /// </summary>
        /// <param name="array">1D array of values where each value should be 0 or 1.</param>
        /// <param name="filePath">The bitmap PNG filename.</param>
        /// <param name="scale">Scale factor for each bit in the array. Determines the width of each bit in the image.</param>
        public static void Draw1DBitmap(int[] array, string filePath, int scale = 10)
        {
            // The height is fixed to a small value since we're creating a 1D image (a line)
            int height = 300;
            int width = array.Length * scale;
            using (var bitmap = new System.Drawing.Bitmap(width, height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White); // Background color

                    // Drawing each bit
                    for (int i = 0; i < array.Length; i++)
                    {
                        Color color = array[i] == 1 ? Color.Black : Color.White; // Active bits are black
                        int x = i * scale;
                        g.FillRectangle(new SolidBrush(color), x, 0, scale, height);
                    }
                }

                bitmap.Save(filePath, ImageFormat.Png);
            }
        }
```

Here is the output image:

![1D_example](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/169d238f-6e4f-444c-8413-d3823596edfe)

The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/7d329e02b165acd03deff0926121d06d9e04b71e/source/UnitTestsProject/EncoderTests/DateTimeEncoderTests.cs#L179).
The Draw1DBitmap method can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/7d329e02b165acd03deff0926121d06d9e04b71e/source/NeoCortexUtils/NeoCortexUtils.cs#L213)


### DrawBitmap example for DateTime Encoder

A DateTime encoder is a type of encoder that transforms datetime information such as dates and times into Sparse Distributed Representations (SDRs)
We can make and use different types of encoder to visualize that particular type of data in order to visualize them. We can take a datetime data and encode them with DateTime encoder and visualize them with DrawBitMaps method. For this example we can take "08/03/2024 21:58:07" and send it through datetime encoder to get SDR.

```csharp
//taking the input
object input = "08/03/2024 21:58:07";
//This line creates a new instance of a DateTimeEncoder with specified settings (encoderSettings) and precision. The DateTimeEncoder.Precision.Days parameter
//indicates that the encoder should focus on the day-level granularity of datetime values. This means that the encoder will represent datetime values in a way that emphasizes their day component, which could be //particularly useful for tasks where daily patterns are important, such as analyzing daily sales data or daily weather patterns.
var encoder = new DateTimeEncoder(encoderSettings, DateTimeEncoder.Precision.Days);
//DateTimeOffset.Parse(input.ToString()) converts the input (which is expected to be a datetime value in a string or similar format) into a
//DateTimeOffset object, which represents a point in time, typically expressed as a date and time of day, along with an offset indicating the time zone. The Encode method of the DateTimeEncoder then processes this //datetime object according to the encoder's configuration, producing an encoded representation
var result = encoder.Encode(DateTimeOffset.Parse(input.ToString(), CultureInfo.InvariantCulture));
```

Now we can get the result from here and make it a 2D array for DrawBitMap method
```csharp
// converts the one-dimensional array result into a two-dimensional array twoDimenArray. The ArrayUtils.Make2DArray method is used for this conversion,
// where result is the source array. The dimensions for the new 2D array are determined by the square root of the length of result, suggesting that the original data is reshaped into a square matrix
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
// ArrayUtils.Transpose method reorganizes twoDimenArray by flipping it over its diagonal, effectively swapping its rows and columns.
var twoDimArray = ArrayUtils.Transpose(twoDimenArray);
```

If we see the SDR we got after converting the DateTime , it looks like this

```
[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, ]
```
Now if wee send the transposed 2D data in DrawBitMap method:
```csharp
//we set the active bits to green, inactive bits to black and we named the file datetime.png, we set the height and width of the bitmap to 1024
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, "datetime.png", Color.Black, Color.Green);
```

The generated image is this

![DateTime_out_08-03-2024 21-58-07_32x32-N-1024-W-21](https://github.com/TanzeemHasan/neocortexapi/assets/74203937/7f4625e2-eb36-43ff-bf9b-b78a67c77f9b)

The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/44772a45ac31c48e74a648ca9b1386fb82520590/source/UnitTestsProject/EncoderTests/DateTimeEncoderTests.cs#L78). 

### Drawing AQI Values with Scalar Encoder
The Scalar Encoder converts AQI levels into SDRs, capturing the essence of air quality in a binary format. For instance, the AQI levels are segmented into:

0-49: Good
50-149: Moderate
150-249: Unhealthy for Sensitive Groups
250-349: Unhealthy
350-449: Very Unhealthy
450-500: Hazardous
Generating Bitmaps
To visualize the encoded AQI values:

Encode AQI Levels: Use the Scalar Encoder to transform AQI values into SDRs, which are stored in a 1-D array, result1.
```csharp
// Initializes a ScalarEncoder to convert numerical values into Sparse Distributed Representations (SDRs).
// Configuration:
// - "W": 21, sets the width of the encoding to represent the active bits in the output SDR.
// - "N": 100, defines the total size of the output SDR.
// - "Radius": -1.0, disables the radius-based input value grouping, preferring exact value encoding.
// - "MinVal" and "MaxVal": specify the range of input values the encoder can handle.
// - "Periodic": false, indicates that the encoder should not treat input values as cyclic.
// - "Name": "scalar", assigns a name to this encoder instance for identification.
// - "ClipInput": false, allows input values outside the MinVal-MaxVal range without clipping them.
// This setup is ideal for encoding scalar values into SDRs, which are then used in Hierarchical Temporal Memory (HTM) systems for further processing.
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

// Encodes the scalar inputValue into a Sparse Distributed Representation (SDR) using the ScalarEncoder.
// The input value is transformed based on the encoder's configuration (e.g., value range, output size).
// The resulting SDR is a binary array where '1's represent the encoded value's features in a high-dimensional space,
// facilitating the processing of numerical data in Hierarchical Temporal Memory (HTM) systems. The encoded SDR
// is stored in the 'result' variable for further use.
var result = encoder.Encode(inputValue);

```
Convert to 2-D Array:

```csharp

// converts the one-dimensional array result into a two-dimensional array twoDimenArray. The ArrayUtils.Make2DArray method is used for this conversion,
// where result is the source array. The dimensions for the new 2D array are determined by the square root of the length of result,
// suggesting that the original data is reshaped into a square matrix
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result1, (int)Math.Sqrt(result1.Length), (int)Math.Sqrt(result1.Length));

```

Transpose the 2-D Array:
```csharp
// // ArrayUtils.Transpose method reorganizes twoDimenArray by flipping it over its diagonal, effectively swapping its rows and columns, making the array usable for converting into bitmaps
var twoDimArray = ArrayUtils.Transpose(twoDimenArray);
```

Draw the Bitmap: Utilize DrawBitmap for visual representation.
```csharp
// DrawBitMap method is called with the 2D array we made, then we pass the width and the height consecutively which is 1024 for bitmap drawing,
// and we set the inactive bits to white by passing the Color.White, and active bits to Blue by passing the color Blue.
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, Path.Combine(folderName, filename), Color.White, Color.Blue, text: i.ToString());
```

Bitmap Visualization Outcomes
The DrawBitmap method is instrumental in converting SDRs into insightful bitmap images, with active bits represented in Dark Orange and inactive bits in Yellow. The method parameters set the bitmap's height and width to 1024 pixels, ensuring a detailed and clear visual output. Each bitmap image is saved with a corresponding index value in the top left corner, facilitating easy identification.

The generated bitmap are as follows:

![AQI_scalar_encoder](https://github.com/TanzeemHasan/neocortexapi/assets/74203937/2b474bcf-9e92-48df-b8ae-4b92c6d09bbe)

The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/f9953ba73c30c4dd24d979e1ca5eb52c5eab4137/source/UnitTestsProject/SdrRepresentation/ScalarEncoderTestOverBitmap.cs#L396).

### DrawBitmap sample for Geospatial Encoder

Geospatial encoder is used to encode latitude or longitude values into Sparse Distributed Representations (SDRs) for Hierarchical Temporal Memory (HTM) systems.. In the exploration of geospatial data through Sparse Distributed Representations (SDRs), we utilize the DrawBitmap method to translate encoded geographical coordinates into visually interpretable bitmap images. This approach allows for the visualization of spatial information encoded within SDRs, offering insights into the encoded geographical regions.

Encoding Process for Geographical Coordinates
To encode and visualize geographical coordinates, we set the encoder parameters as follows, aiming to cover a specific range of latitude and longitude:
```csharp
// Initializes encoderSettings with specific parameters for the GeoSpatialEncoderExperimental.
encoderSettings.Add("W", 21); // The width of the encoder's output SDR. Specifies the number of bits set to 1.
encoderSettings.Add("N", 40); // The total number of bits in the output SDR. Determines the SDR's dimensionality.
encoderSettings.Add("MinVal", (double)48.75); // The minimum value of the input range, representing the latitude of Italy.
encoderSettings.Add("MaxVal", (double)51.86); // The maximum value of the input range, representing the latitude of Germany.
encoderSettings.Add("Radius", (double)1.5); // The radius of coverage around a point. Influences how input values are encoded.
encoderSettings.Add("Periodic", (bool)false); // Determines if the encoder should treat inputs as cyclical values.
encoderSettings.Add("ClipInput", (bool)true); // If true, inputs outside the range [MinVal, MaxVal] are clipped to the range's endpoints.
encoderSettings.Add("IsRealCortexModel", false); // Custom setting, potentially indicating if the encoder mimics real cortical encoding.

// Instantiates a new GeoSpatialEncoderExperimental with the configured settings.
// This encoder is designed to handle geospatial data, likely converting latitude (or longitude) values into an SDR.
GeoSpatialEncoderExperimental encoder = new GeoSpatialEncoderExperimental(encoderSettings);
```
These settings enable the encoding of geographical data within the specified latitude range, capturing the essence of spatial information in binary form.

Generating Bitmaps from Encoded Geospatial Data:
1. Initialization: The geographical coordinates within the range of 48.75 to 51.86 are encoded using the configured encoder. This process converts each coordinate into a 1-Dimensional SDR.

2. Transformation to 2-D Array: The resulting 1-D SDR is then mapped to a 2-Dimensional array, preparing it for bitmap visualization:
```csharp
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result2, (int)Math.Sqrt(result2.Length), (int)Math.Sqrt(result2.Length));
```
This array, with dimensions inferred from the SDR's length, is then transposed to align with the bitmap generation process.

3. The transposed 2-D array is passed to the DrawBitmap method along with visualization parameters such as dimensions (1024x1024 pixels), file path, and colors for active and inactive cells (Active: Black, Inactive: LightSeaGreen):
```csharp
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, $"{folderName}\\{j}.png", Color.LightSeaGreen, Color.Black, text: j.ToString());
```

The bitmap generated are as follows:

![geospatial_output](https://github.com/TanzeemHasan/neocortexapi/assets/74203937/bece1fe1-c62d-4d48-aa9a-4994deff26af)

The bitmap images generated for geographical coordinates offer a unique view of the spatial patterns encoded within the SDRs. Now if we change the encoder settings and provide the below settings:
```
encoderSettings.Add("W", 21);
encoderSettings.Add("N", 40);
```
The output will be different for the same value. The bitmaps generated in this case are:

![geospatio_changed_input_parameter](https://github.com/TanzeemHasan/neocortexapi/assets/74203937/dc7b04a4-4183-4628-a8b6-784e999d6299)

The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/2220f6f125c412a236e7ed88402878c9e4cbaa61/source/UnitTestsProject/EncoderTests/GeoSpatialEncoderExperimentalTests.cs#L410). 


### Bitmap representation of Image using Spatial Pooler

In the context of Hierarchical Temporal Memory (HTM) theory, the Spatial Pooler (SP) plays a crucial role in transforming input data into Sparse Distributed Representations (SDRs). These representations capture the essential features of the input data in a way that emphasizes structural and semantic similarities. To visually understand the transformation process and the output of the Spatial Pooler, we can represent SDRs as bitmaps. This example elaborates on how active arrays generated by the SpatialPooler are converted into bitmap images, further enhancing our understanding of HTM's processing capabilities.

#### Generating Inputs for Image Generation and Training in Spatial Pooler: 
In the Spatial Pooler (SP), the generation of inputs for image processing and training involves several steps that lead to the creation of Sparse Distributed Representations (SDRs). The code snippets outlines how inputs are prepared, processed, and used for training the Spatial Pooler as well as generating SDRs.

```csharp
// Define a folder for training files
string trainingFolder = @"..\..\..\TestFiles\Sdr";

// In the trainingImages variable, we are getting the input images for making binarized input 
var trainingImages = Directory.GetFiles(trainingFolder, "*.jpeg");

```
Each image file within the directory is intended to be processed individually to generate binary representations that can be fed into the Spatial Pooler.
The Image we have used for training is these:

![Test7_2](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/aaac62b5-1a5f-4cea-94d7-c9dda22cc9fd)

Before being used as input for the Spatial Pooler, each image undergoes a binarization process. This process converts the grayscale images into binary images, where each pixel is either on or off, representing active or inactive bits. The binarization helps in simplifying the complexity of the input data for the Spatial Pooler.

```csharp
// BinarizeImage function processes each image, resizing it to a specified dimension (imgSize x imgSize) and converting it to a binary format.
// The result is a simpler representation of the original image, which is more suitable for processing by the Spatial Pooler
string inputBinaryImageFile = NeoCortexUtils.BinarizeImage($"{trainingImage}", imgSize, testName);

```

With the binary representation of images ready, the Spatial Pooler computes the Sparse Distributed Representation (SDR) for each binarized image. The computation involves selecting a subset of columns (neurons) to become active in response to the input pattern. This results in an SDR that captures the essential features of the input data.
```csharp
// The compute method of the Spatial Pooler takes the binary vector (inputVector) as input and fills the activeArray with indices of active columns,
// effectively generating the SDR for the given input.
sp.compute(inputVector, activeArray, true);

// getting the active column values from active array where bit values are 1
var activeCols = ArrayUtils.IndexWhere(activeArray, (el) => el == 1);
```
The active columns represented by the SDRs are then used to create visual representations. This is done by converting the SDRs back into a two-dimensional array format suitable for generating bitmap images. These images visually represent the activity of the Spatial Pooler in response to the input data, facilitating an understanding of how the SP processes and recognizes patterns.

```csharp
// converts the one-dimensional array result into a two-dimensional array twoDimenArray. The ArrayUtils.Make2DArray method is used for this conversion,
// where result is the source array. The dimensions for the new 2D array are determined by the square root of the length of result, suggesting that the original data is reshaped into a square matrix
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(activeArray, colDims[0], colDims[1]);

// ArrayUtils.Transpose method reorganizes twoDimenArray by flipping it over its diagonal, effectively swapping its rows and columns.
twoDimenArray = ArrayUtils.Transpose(twoDimenArray);

// Initializes the NeoCortexUtils.DrawBitmaps method to create a bitmap image from the provided 2D arrays.
// The 'arrays' parameter contains the SDRs (Sparse Distributed Representations) to be visualized.
// 'outputImage' specifies the file path where the generated bitmap image will be saved.
// Active bits in the SDR are colored red ('Color.Red'), while inactive bits are colored black ('Color.Black').
// 'OutImgSize' determines both the width and height of the generated bitmap, creating a square image.
// This visualization helps in understanding the patterns of activity within the spatial pooler's output,
// making it easier to analyze the behavior of the neural network model in response to different inputs.
NeoCortexUtils.DrawBitmaps(arrays, outputImage, Color.Red, Color.Black, OutImgSize, OutImgSize);

```
The DrawBitmaps utility function generates bitmap images that visually represent the active columns as a result of the SP's processing. The generated Images looks like this.

![Screenshot 2024-031331 000538](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/8368a405-0900-4516-973c-91a3e6882852)

On the right side we can see the images generated from the binarized file of the input training image, on the left we have the sdr representation by SpatialPooler after feeding the training image.

#### Example representing Overlap(Intersection),Difference and Union for Alphabet T and Neumeric 3 in Bitmap after computing in spatial pooler:

Here we can see the Alphabet T and Neumeric 3

![Screenshot 2024-03-22 000510](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/4922914d-5bc9-4a6e-9d1b-6e79f2706928)

Below are the represenatation for Overlap, Difference and Union:

![generated_diffusion](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/0710ef8e-12c0-4ed3-aebf-f6ee13f93147)

Two SDRs are compared with the help of UnionSDRFun(), DiffSDRFun() and OverlapSDRFun() functions. The overlap.png shows very few intersections as the SDRs are very different from each other. Basically, the combination of Overlap and Difference gives us Union.

#### Example representing Overlap(Intersection),Difference and Union for Alphabet T and 1 in Bitmap after computing in spatial pooler

Generated Bitmap representation of T and l is shown below:

![Screenshot 2024-03-22 001009](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/61375b26-37eb-46d6-9448-fb2fec6d190d)

Below are the representation for Overlap, Difference and Union:

![generate_diffusion_2](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/57a4b93b-934c-423e-ab60-3e3e29469fde)

On comparison between two SDRs, the overlap.png shows more overlaps/intersections in comparison to the above example. SDRs are similar to each other.

The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/8de0bf4d823b94393381b63984c8c1c5a47e330d/source/UnitTestsProject/SdrRepresentation/SpatialPoolerColumnActivityTest.cs#L19).



   
