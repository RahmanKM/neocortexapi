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
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result1, (int)Math.Sqrt(result1.Length), (int)Math.Sqrt(result1.Length));
```
Convert to 2-D Array:

```csharp
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result1, (int)Math.Sqrt(result1.Length), (int)Math.Sqrt(result1.Length));
```

Transpose the 2-D Array:
```csharp
var twoDimArray = ArrayUtils.Transpose(twoDimenArray);
```

Draw the Bitmap: Utilize DrawBitmap for visual representation.
```csharp
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, Path.Combine(folderName, filename), Color.Yellow, Color.DarkOrange, text: i.ToString());
```

Bitmap Visualization Outcomes
The DrawBitmap method is instrumental in converting SDRs into insightful bitmap images, with active bits represented in Dark Orange and inactive bits in Yellow. The method parameters set the bitmap's height and width to 1024 pixels, ensuring a detailed and clear visual output. Each bitmap image is saved with a corresponding index value in the top left corner, facilitating easy identification.

The generated bitmap are as follows:

![AQI_scalar_encoder](https://github.com/TanzeemHasan/neocortexapi/assets/74203937/2b474bcf-9e92-48df-b8ae-4b92c6d09bbe)

### DrawBitmap sample for Geospatial Encoder

In the exploration of geospatial data through Sparse Distributed Representations (SDRs), we utilize the DrawBitmap method to translate encoded geographical coordinates into visually interpretable bitmap images. This approach allows for the visualization of spatial information encoded within SDRs, offering insights into the encoded geographical regions.

Encoding Process for Geographical Coordinates
To encode and visualize geographical coordinates, we set the encoder parameters as follows, aiming to cover a specific range of latitude and longitude:
```csharp
encoderSettings.Add("W", 103);
encoderSettings.Add("N", 238);
encoderSettings.Add("MinVal", 48.75);
encoderSettings.Add("MaxVal", 51.86);
```
These settings enable the encoding of geographical data within the specified latitude range, capturing the essence of spatial information in binary form.

Generating Bitmaps from Encoded Geospatial Data:
1. Initialization: The geographical coordinates within the range of 48.75 to 51.86 are encoded using the configured encoder. This process converts each coordinate into a 1-Dimensional SDR.

2. Transformation to 2-D Array: The resulting 1-D SDR is then mapped to a 2-Dimensional array, preparing it for bitmap visualization:
```csharp
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result2, (int)Math.Sqrt(result2.Length), (int)Math.Sqrt(result2.Length));
```
This array, with dimensions inferred from the SDR's length, is then transposed to align with the bitmap generation process.

3. The transposed 2-D array is passed to the DrawBitmap method along with visualization parameters such as dimensions (1024x1024 pixels), file path, and colors for active and inactive cells (Active: Green, Inactive: Red):
```csharp
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, $"{folderName}\\{j}.png", Color.Red, Color.Green, text: j.ToString());
```

The bitmap generated are as follows:

<img src="https://user-images.githubusercontent.com/74201238/114312848-68730c00-9af4-11eb-814b-f93abc095885.png" width="450"><br />

The bitmap images generated for geographical coordinates offer a unique view of the spatial patterns encoded within the SDRs. For example, the image for latitude 51.85 illustrates the encoded location's characteristics, providing a visual representation of the geographical information.

The bitmaps generated in this case are:

<img src="https://user-images.githubusercontent.com/74201238/114312899-a2441280-9af4-11eb-8a29-c3379d280fe0.png" width="450"><br />

### Bitmap representation of Image using Spatial Pooler

The Spatial Pooler's role within Hierarchical Temporal Memory (HTM) systems is to convert input data into SDRs, which are efficient and robust representations of the original data. The `DrawBitmaps` function, part of the `NeoCortexUtils` utility class, facilitates the visualization of these SDRs as bitmap images.

#### Creating SDRs from Spatial Pooler

SDRs are generated by encoding geospatial data using a Spatial Pooler. The Spatial Pooler learns and remembers spatial patterns of the input data, producing a binary output array where '1's represent active columns and '0's represent inactive columns. For detailed information on training the Spatial Pooler and creating SDRs, refer to the [Spatial Pooler Example](#).

### Bitmap Visualization Process

1. **Conversion to 2D Array**: The active array produced by the Spatial Pooler is reshaped into a 2-dimensional (2D) array using the `ArrayUtils` class. This is essential for creating a visual representation. In this example, we assume the dimensions of the 2D array are 64x64.

2. **Drawing the Bitmap**: The 2D array is then visualized as a bitmap image. The bitmap dimensions and scale are calculated to ensure that the SDR is accurately represented within the bitmap. Colors are assigned to distinguish between active and inactive bits.

    ```csharp
    System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(bmpWidth, bmpHeight);
    ```

    The `Bitmap` constructor from the `System.Drawing` library requires the bitmap's width and height as parameters.

    ```csharp
    NeoCortexUtils.DrawBitmaps(arrays, outputImage, Color.Yellow, Color.Gray, OutImgSize, OutImgSize);
    ```

    The `DrawBitmaps` function takes several parameters:

    - `arrays`: The 2D array containing the SDRs.
    - `outputImage`: The file path for the output bitmap image.
    - `Color.Yellow`: Represents inactive bits.
    - `Color.Gray`: Represents active bits.
    - `OutImgSize`: Specifies both the width and height of the output bitmap.

### Example: Visual Representation of the Letter 'L'

Consider an SDR generated from the Spatial Pooler that encodes the shape of the letter 'L'. To visualize this SDR:

1. **Setting Dimensions**: The SDR is represented in a 2D array with dimensions 64x64, which is then scaled up to create a 1024x1024 bitmap image.

2. **Calculating the Scale**: 
    
    ```csharp
    var scale = ((bmpWidth) / twoDimArrays.Count) / (w+1);
    ```

    This scale determines how many pixels in the bitmap represent a single bit in the 2D array. For a detailed and enlarged visualization of the 'L', a calculated scale ensures each bit occupies multiple pixels.

3. **Visual Outcome**: The bitmap images below illustrate the 'L' shape encoded in SDRs. The active bits (representing the 'L') are colored Gray, while the inactive bits are Yellow.

    ![SDR Bitmap Representation of 'L'](https://user-images.githubusercontent.com/74201563/113511808-0baaab00-9562-11eb-81ea-3ccc35eaa34d.png)

    Changing the scale can adjust the bitmap's granularity and size of the represented 'L'.

   
