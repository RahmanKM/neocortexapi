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

### Basic SDR Example

Let's visualize a basic SDR with a pattern of activation. This simple example will help understand the visualization process:

```csharp
// Simple SDR array representing a basic pattern
int[,] sdr = new int[,] { {0, 1, 1, 0}, {1, 0, 0, 1}, {1, 1, 0, 0}, {0, 0, 1, 1} };
// Visualizing the SDR using DrawBitmap
DrawBitmap(sdr, 4, 4, "basic_sdr_bitmap.png", Color.White, Color.Black);
```




