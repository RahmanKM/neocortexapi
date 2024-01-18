## DrawBitmap Method

### Description
The `DrawBitmap` method in the `NeoCortexUtils` class is responsible for generating a bitmap representation from a 1D, 2D, 3D array of active columns. This method supports customization of colors, text overlay, and bitmap scaling.

### Method Signature
```csharp
public static void DrawBitmap(int[,] twoDimArray, int width, int height, String filePath, Color inactiveCellColor, Color activeCellColor, string text = null)