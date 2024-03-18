# Bitmap Representation of Sparse Distributed Representations (SDRs)

This document outlines the method for visualizing Sparse Distributed Representations (SDRs) as bitmap images using the `DrawBitmap` method. Visualizing SDRs can significantly aid in understanding the patterns and information encoded within them, especially when dealing with complex encoders or spatial pooling processes. This method provides a tangible way to assess and interpret the activity and structure of SDRs, facilitating a deeper understanding of their functionality in various applications.

## Introduction

The `DrawBitmap` method offers a versatile approach to represent SDRs visually, enabling the examination of their properties and behaviors. By converting SDRs into bitmap images, we can observe the activation patterns and interactions within the data, providing insights into the encoding and processing mechanisms at play. This document serves as a guide to utilizing the `DrawBitmap` method across different scenarios, highlighting its application with simple SDR examples as well as encoder-generated SDRs.

## The DrawBitmap Method

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

### Basic SDR Examples
#### Example of visualizing a Number
Let's visualize a basic SDR with a pattern of activation. This simple example will help understand the visualization process. So for this we can take a simple value say ```18```. Now we need SDR encode this data in order to visualize.
```csharp
var encoderSettings = new Dictionary<string, object>
            {
                { "W", 3},
                { "N", 16},
                { "MinVal", 0},
                { "MaxVal", 100},
                { "ClipInput", true},
                { "Padding", 10}, 
            };

            var encoder = new BinaryEncoder(encoderSettings);

            // Input value to encode. This is the value that will be converted into a binary representation.
            string inputValue = "18";

            // Encode the input value.
            var result = encoder.Encode(inputValue);
```  

Now we can make this result 2D as this is now 1 dimension and we need two dimension for drawing the bitmap.
```csharp
int[,] twoDimenArray = new int[1, result.Length];
for (int i = 0; i < result.Length; i++)
{
   twoDimenArray[0, i] = result[i];
}
//Transpose of the 2-D array
var twoDimArray = ArrayUtils.Transpose(twoDimenArray);
```
Now we can draw this data in the DrawBitMap method. 
```csharp
NeoCortexUtils.DrawBitmap(twoDimArray, 16, 16, filePath,Color.Black, Color.Yellow);
```
We can get the below image from the DrawBitMap method

![EncodedValueVisualization18](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/88946733-7ae2-4d0c-bd04-0332b18f3e28)

Figure 1: Outpit image for 18.

#### Example of visualizing a random double value
For this example we will take the value ```45.67```. We will encode the value by setting up the sdr encoder settings and encode the value with that settings. and also set up the two dimensional array
```csharp
var encoderSettings = new Dictionary<string, object>
            {
                { "W", 3},
                { "N", 160},
                { "MinVal", 0},
                { "MaxVal", 100},
                { "ClipInput", true},
                { "Padding", 10}, 
            };

            var encoder = new BinaryEncoder(encoderSettings);
            // Input value to encode. This is the value that will be converted into a binary representation.
            string inputValue = "str";

            // Encode the input value.
            var result = encoder.Encode(inputValue);

            // Preparing for visualization by converting the 1D result array into a 2D array for the 'DrawBitmap' method.
            int[,] twoDimenArray = new int[1, result.Length];
            for (int i = 0; i < result.Length; i++)
            {
                twoDimenArray[0, i] = result[i];
            }

            var twoDimArray = ArrayUtils.Transpose(twoDimenArray);
```
Now lets draw this using DrawBitMap method
```csharp
NeoCortexUtils.DrawBitmap(twoDimArray, 160, 160, "EncodedValueVisualization-str.png", Color.Black, Color.Yellow);
```
It treturns the below picture,

![EncodedValueVisualization-str](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/81fe228b-468b-43f3-9023-419543a946ff)

Figure 2: Outpit image for "45.67"








