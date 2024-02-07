## DrawBitmap Method

## Purpose
The DrawBitmap method in the BitmapDrawer class serves the purpose of creating a bitmap image from an array of active columns. This method provides flexibility in generating visual representations of data in the form of a bitmap. 

### Description
The `DrawBitmap` method in the `NeoCortexUtils` class is responsible for generating a bitmap representation from a 1D, 2D, 3D array of active columns. This method supports customization of colors, text overlay, and bitmap scaling.

### Method Signature
```csharp
public static void DrawBitmap(int[,] twoDimArray, int width, int height, String filePath, Color inactiveCellColor, Color activeCellColor, string text = null)
```

### Parameters

1. twoDimArray (int[,]): Array of active columns with two dimensions.
2. width (int): Output width of the bitmap.
3. height (int): Output height of the bitmap.
4. filePath (String): The filename of the generated bitmap (PNG format).
5. inactiveCellColor (Color): Color of inactive cells in the bitmap.
6. activeCellColor (Color): Color of active cells in the bitmap.
7. text (string, optional): Text to be written on the bitmap.

### Overloaded Methods

Overload 1 Default colors, no text overlay
```csharp
public static void DrawBitmap(int[,] twoDimArray, int width, int height, String filePath, string text = null)
```

Overload 2 Custom colors and text overlay
```csharp
public static void DrawBitmap(int[,] twoDimArray, int width, int height, String filePath, Color inactiveCellColor, Color activeCellColor, string text = null)
```

Overload 3: Scale bitmap with custom colors and text overlay
```csharp
public static void DrawBitmap(int[,] twoDimArray, int scale, String filePath, Color inactiveCellColor, Color activeCellColor, string text = null)
```

Example Usage for DrawBitmaps
```csharp
DrawBitmaps(listOfArrays, "output_combined_bitmap.png", Color.DarkGray, Color.Yellow, 1024, 1024);
```





