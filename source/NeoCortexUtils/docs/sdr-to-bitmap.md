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




