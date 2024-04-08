# Bitmap Representation of Sparse Distributed Representations (SDRs)

This document outlines the method for visualizing Sparse Distributed Representations (SDRs) as bitmap images using the `DrawBitmap` method. Visualizing SDRs can significantly aid in understanding the patterns and information encoded within them, especially when dealing with complex encoders or spatial pooling processes. This method provides a tangible way to assess and interpret the activity and structure of SDRs, facilitating a deeper understanding of their functionality in various applications.

## Introduction

### What is SDR:

Sparse Distributed Representation (SDR) is a concept used in computing that mirrors the brain's method of processing information. Essentially, it's a way of storing data where most of the bits are off (0), and only a few are on (1). This setup allows for handling a wide variety of information efficiently and robustly, much like how the brain operates with its neurons. Sparse Distributed Representations (SDRs) are pivotal in computational models that mimic the human brain's processing, such as Hierarchical Temporal Memory (HTM). Brain works on continuous stream of input patterns which represent input sequences based on the input stream's recursive pattern

### What is Bitmap:

Bitmap is a type of file format which is used to store images. A bitmap is a spatially mapped array of bits i.e. a map of bits. For the purpose of representing SDRs as bitmaps, first this needs to feed the output of encoders as inputs to the SP. The purpose of generating bitmaps to represent Sparse Distributed Representations (SDRs) in Hierarchical Temporal Memory (HTM) is to provide a visual representation of the encoded information and the processing performed by HTM algorithms such as encoders, spatial poolers, and temporal memory. By visualizing the SDRs as bitmaps, one can gain insights into how the input data is transformed and processed throughout the different stages of the HTM system. Bitmaps allow for easy interpretation of the sparsity and patterns within the representations, facilitating analysis and debugging of the HTM algorithms. Additionally, visualizing SDRs as bitmaps enables researchers and developers to observe the effects of parameter changes or optimizations on the encoded representations, aiding in the refinement and improvement of HTM algorithms. Bitmaps of SDRs serve as a valuable tool for understanding the inner workings of HTM algorithms and optimizing their performance for various applications.

In the context of an encoder, bitmaps visually depict how raw input data is transformed into sparse binary patterns. Each bit in the bitmap represents the presence or absence of a feature or characteristic in the input data. By generating bitmaps of the encoded representations, one can observe how different input signals are represented sparsely in the binary space. This visualization aids in understanding how the encoder is capturing relevant information from the input data and converting it into a format suitable for further processing by the HTM network.

For the spatial pooler, bitmaps illustrate the activation patterns of columns in response to input data. Each column's activation state is represented by a bit in the bitmap, where active columns are indicated by set bits and inactive columns by unset bits. By generating bitmaps of the spatial pooler's output, one can visualize how the input patterns are distributed and transformed across the columns of the spatial pooler. This visualization helps in analyzing the sparsity and distribution of active columns, as well as understanding the spatial pooling process and its impact on the input representations.

In temporal memory, bitmaps depict the active cells and connections within the network over time. Each bit in the bitmap corresponds to the activation state of a cell or connection, representing the temporal sequence of patterns learned by the HTM network. By generating bitmaps of the temporal memory's activity, one can observe how the network learns and predicts sequences of input patterns, as well as analyze the stability and adaptability of the learned representations. This visualization aids in understanding the temporal processing capabilities of the HTM network and assessing its performance in sequence learning tasks.


### The DrawBitmap Method

The `DrawBitmap` method offers a versatile approach to represent SDRs visually, enabling the examination of their properties and behaviors. By converting SDRs into bitmap images, which can observe the activation patterns and interactions within the data, providing insights into the encoding and processing mechanisms at play. This document serves as a guide to utilizing the `DrawBitmap` method across different scenarios, highlighting its application with simple SDR examples as well as encoder-generated SDRs.

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

### Methodology Description

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

There are three functions of DrawBitMaps in this project. Two DrawBitMaps functions take a two dimensional array and one DrawBitMaps function takes a list of two dimensional arrays. 
The main function of DrawBitMaps is: 
```csharp
public static void DrawBitmap(int[,] twoDimArray, int scale, String filePath, Color inactiveCellColor, Color activeCellColor, string text = null)
        {
            int w = twoDimArray.GetLength(0);
            int h = twoDimArray.GetLength(1);

            System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(w * scale, h * scale);
            int k = 0;
            for (int Xcount = 0; Xcount < w; Xcount++)
            {
                for (int Ycount = 0; Ycount < h; Ycount++)
                {
                    for (int padX = 0; padX < scale; padX++)
                    {
                        for (int padY = 0; padY < scale; padY++)
                        {
                            if (twoDimArray[Xcount, Ycount] == 1)
                            {
                                //myBitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Yellow); // HERE IS YOUR LOGIC
                                myBitmap.SetPixel(Xcount * scale + padX, Ycount * scale + padY, activeCellColor); // HERE IS YOUR LOGIC
                                k++;
                            }
                            else
                            {
                                //myBitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black); // HERE IS YOUR LOGIC
                                myBitmap.SetPixel(Xcount * scale + padX, Ycount * scale + padY, inactiveCellColor); // HERE IS YOUR LOGIC
                                k++;
                            }
                        }
                    }
                }
            }

            Graphics g = Graphics.FromImage(myBitmap);
            var fontFamily = new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif);
            g.DrawString(text, new Font(fontFamily, 32), SystemBrushes.Control, new PointF(0, 0));

            myBitmap.Save(filePath, ImageFormat.Png);
        }
```

This function takes a two-dimensional array twoDimArray representing a binary image, along with parameters for scale, file path, inactive cell color, active cell color, and an optional text label. First, it determines the dimensions of the array w and h. Then, it creates a new bitmap image with dimensions w * scale by h * scale. It iterates over each element in the array, and for each active cell (value of 1), it sets the corresponding pixel in the bitmap to the active cell color, while for inactive cells (value of 0), it sets the pixel to the inactive cell color. This process is repeated at scale times to scale up the image. Additionally, if a text label is provided, it draws the text onto the bitmap. Finally, the bitmap is saved to the specified file path as a PNG image. The function effectively converts a binary array into a bitmap image with customizable colors and text. This function is mainly for generating the SDRs for active columns. This same function can be used for encoder also with transposing the two dimensional array that this function takes as an input.

Also, Another function is written as shown below and that is also for active columns with small changes.

```csharp
public static void DrawBitmap(int[,] twoDimArray, int width, int height, String filePath, Color inactiveCellColor, Color activeCellColor, string text = null)
        {
            int w = twoDimArray.GetLength(0);
            int h = twoDimArray.GetLength(1);

            if (w > width || h > height)
                throw new ArgumentException("Requested width/height must be greather than width/height inside of array.");

            var scale = width / w;

            if (scale * w < width)
                scale++;

            DrawBitmap(twoDimArray, scale, filePath, inactiveCellColor, activeCellColor, text);

        }
```
This function also takes input as a two dimensional array and additionally it takes height and width which is validated by two dimensional array’s row length and column length. Here, ‘w’ is the row's length and ‘h’ is the column’s length. If the condition met then it throws an argument exception else the previous method of draw bitmap function is used.

Another function was written which takes a list of two dimensional arrays as shown follow.
 ```csharp
 public static void DrawBitmaps(List<int[,]> twoDimArrays, String filePath, Color inactiveCellColor, Color activeCellColor, int bmpWidth = 1024, int bmpHeight = 1024)
        {
            int widthOfAll = 0, heightOfAll = 0;

            foreach (var arr in twoDimArrays)
            {
                widthOfAll += arr.GetLength(0);
                heightOfAll += arr.GetLength(1);
            }

            if (widthOfAll > bmpWidth || heightOfAll > bmpHeight)
                throw new ArgumentException("Size of all included arrays must be less than specified 'bmpWidth' and 'bmpHeight'");

            System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(bmpWidth, bmpHeight);
            int k = 0;

            for (int n = 0; n < twoDimArrays.Count; n++)
            {
                var arr = twoDimArrays[n];

                int w = arr.GetLength(0);
                int h = arr.GetLength(1);

                var scale = ((bmpWidth) / twoDimArrays.Count) / (w + 1);// +1 is for offset between pictures in X dim.

                //if (scale * (w + 1) < (bmpWidth))
                //    scale++;

                for (int Xcount = 0; Xcount < w; Xcount++)
                {
                    for (int Ycount = 0; Ycount < h; Ycount++)
                    {
                        for (int padX = 0; padX < scale; padX++)
                        {
                            for (int padY = 0; padY < scale; padY++)
                            {
                                if (arr[Xcount, Ycount] == 1)
                                {
                                    myBitmap.SetPixel(n * (bmpWidth / twoDimArrays.Count) + Xcount * scale + padX, Ycount * scale + padY, activeCellColor); // HERE IS YOUR LOGIC
                                    k++;
                                }
                                else
                                {
                                    myBitmap.SetPixel(n * (bmpWidth / twoDimArrays.Count) + Xcount * scale + padX, Ycount * scale + padY, inactiveCellColor); // HERE IS YOUR LOGIC
                                    k++;
                                }
                            }
                        }
                    }
                }
            }

            myBitmap.Save(filePath, ImageFormat.Png);
        }
 ```

At first, this function takes a list of two dimensional arrays then iterates through each two dimensional array. After this, the two dimensional array also validated like the previous method of draw bitmap function. Here the different thing is, it is calculating the scale value based on the specified bitmap width and the number of arrays (two dimensional arrays) count. At the end, it generates the bitmaps and saves the image.

As of now, there are no bitmap functions of an one dimensional array. That’s why, created a function for representing the SDRs for a one dimensional array

```csharp
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

This Draw1DBitmap method creates a 1D bitmap image representing a binary array. Each element in the array corresponds to a bit in the bitmap image. Active bits (with a value of 1) are represented as black rectangles, while inactive bits (with a value of 0) are represented as white rectangles. The dimensions of the bitmap are determined by the length of the input array and a specified scale factor. The resulting bitmap image is saved to the specified file path in PNG.

## Result
This project showcasing how different data types, including numbers, geographical coordinates, and temporal information, can be visually represented. In this experiment we can input any significant individual data to convert it into the bitmap. Some examples with experiment details provided here in this section.

### Basic SDR Examples with binary encoders

Binary encoder encodes the data and the via the encoder it converts the data into SDR which will then shown in the bitmap using drawbitmap method. At first, visualizing a basic SDR with a pattern of activation. This simple example will help understand the visualization process. So for this by taking a simple value say ```40148```. Now it needs SDR to encode this data in order to visualize.
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

Now by making this result 2D as this is now 1 dimension and which needs two dimension for drawing the bitmap.
```csharp
// converts the one-dimensional array result into a two-dimensional array twoDimenArray. The ArrayUtils.Make2DArray method is used for this conversion,
// where result is the source array. The dimensions for the new 2D array are determined by the square root of the length of result, suggesting that the original data is reshaped into a square matrix
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
// ArrayUtils.Transpose method reorganizes twoDimenArray by flipping it over its diagonal, effectively swapping its rows and columns.
var twoDimArray = ArrayUtils.Transpose(twoDimenArray);
```
After that put this data in the DrawBitMap method. 
```csharp
// DrawBitMap method is called with the 2D array which is made, then pass the width and the height consecutively which is 1024 for bitmap drawing,
// and set the inactive bits to black by passing the Color.Black, and active bits to yellow.
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, filePath, Color.Black, Color.Yellow);
```
After that the below image is generated from the DrawBitMap method

<img src="https://github.com/TanzeemHasan/neocortexapi/assets/74203937/9bfc6c35-925a-41cc-95db-50f494a8cedd" width="400" height="400" />


Similarly, for another example, taken a random integer value ```50149```. This value will encode by setting up the sdr encoder settings and encode the value with that settings and also set up the two dimensional array
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
Now drawing this using DrawBitMap method
```csharp
// DrawBitMap method is called with the 2D array that is made, then the width and the height is passed consecutively which is 1024 for bitmap drawing,
// and set the inactive bits to black by passing the Color.Black, and active bits to yellow.
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, "EncodedValueVisualization-45_67.png", Color.Black, Color.Yellow);
```
It returns the below picture,

<img src="https://github.com/TanzeemHasan/neocortexapi/assets/74203937/16ccc118-d5f6-494c-a9c4-71a5ab86c50d" width="400" height="400" />

The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/44772a45ac31c48e74a648ca9b1386fb82520590/source/UnitTestsProject/EncoderTests/DateTimeEncoderTests.cs#L116)

### DrawBitMap example with Binary Encoder 1D image

This one can also use DrawBitMap method for generating 1D images by getting the binary encoded value from the input. 

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
This method which can generate Images from the 1D sdrs from the binary encoder and make the data suitable for the DrawBitMap method to generate a 1D Image which is created.

```csharp
// The created method can be called and pass the sdrs named result, filepath and a sample scale vale of 200
NeoCortexUtils.Draw1DBitmap(result, filePath, 200);
```

Here is the implemented Draw1DBitmap
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
Different types of encoders can be made and used to visualize that particular type of data in order to visualize them. A datetime data can be taken and encoded them with DateTime encoder and visualize them with DrawBitMaps method. For this example "08/03/2024 21:58:07" can be taken or any other datetime values and send it through datetime encoder to get SDR.

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

Now the result can be found from here and make it a 2D array for DrawBitMap method
```csharp
// converts the one-dimensional array result into a two-dimensional array twoDimenArray. The ArrayUtils.Make2DArray method is used for this conversion,
// where result is the source array. The dimensions for the new 2D array are determined by the square root of the length of result, suggesting that the original data is reshaped into a square matrix
int[,] twoDimenArray = ArrayUtils.Make2DArray<int>(result, (int)Math.Sqrt(result.Length), (int)Math.Sqrt(result.Length));
// ArrayUtils.Transpose method reorganizes twoDimenArray by flipping it over its diagonal, effectively swapping its rows and columns.
var twoDimArray = ArrayUtils.Transpose(twoDimenArray);
```

The SDR found after converting the DateTime looks like this

```
[0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, ]
```
Now the transposed 2D data is sent in DrawBitMap method:
```csharp
//the active bits set to green, inactive bits to black and named the file datetime.png, the height and width of the bitmap is set to 1024
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, "datetime.png", Color.Black, Color.Green);
```

The generated image looks like this

<img src="https://github.com/TanzeemHasan/neocortexapi/assets/74203937/7f4625e2-eb36-43ff-bf9b-b78a67c77f9b" width="400" height="400" />

Here are some other example generated bitmap of different datetimes

|    Datetime   | 05/01/2024 21:58:07                                                                         | 06/02/2024 21:58:07                                                                         | 07/01/2024 21:58:07                                                                         | 01/02/2024 21:58:07                                                                         |
|-------|---------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------|
|    results   | <img src="images/1.png" height="200" width="200"/>                                   | <img src="images/2.png" alt="" height="200" width="200"/>                                   | <img src="images/3.png" alt="" height="200" width="200"/>                                   | <img src="images/4.png" alt="" height="200" width="200"/>                                   |


The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/44772a45ac31c48e74a648ca9b1386fb82520590/source/UnitTestsProject/EncoderTests/DateTimeEncoderTests.cs#L78). 

### Drawing AQI Values with Scalar Encoder
Scalar encoders are crucial in Hierarchical Temporal Memory (HTM) for converting scalar values into sparse distributed representations (SDRs), facilitating efficient storage, computation, and pattern recognition tasks. They enable HTM networks to encode diverse data types while capturing important spatial and temporal patterns, supporting learning, inference, and prediction from complex and high-dimensional data. </br>

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
// DrawBitMap method is called with the 2D array which is made, then passed the width and the height consecutively which is 1024 for bitmap drawing,
// and set the inactive bits to white by passing the Color.White, and active bits to Blue by passing the color Blue.
NeoCortexUtils.DrawBitmap(twoDimArray, 1024, 1024, Path.Combine(folderName, filename), Color.White, Color.Blue, text: i.ToString());
```

Bitmap Visualization Outcomes
The DrawBitmap method is instrumental in converting SDRs into insightful bitmap images, with active bits and inactive bits. The method parameters set the bitmap's height and width to 1024 pixels, ensuring a detailed and clear visual output. Each bitmap image is saved with a corresponding index value in the top left corner, facilitating easy identification.

The generated bitmap are as follows:

![AQI_scalar_encoder](https://github.com/TanzeemHasan/neocortexapi/assets/74203937/2b474bcf-9e92-48df-b8ae-4b92c6d09bbe)

The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/f9953ba73c30c4dd24d979e1ca5eb52c5eab4137/source/UnitTestsProject/SdrRepresentation/ScalarEncoderTestOverBitmap.cs#L396).

### DrawBitmap sample for Geospatial Encoder

Geospatial encoder is used to encode latitude or longitude values into Sparse Distributed Representations (SDRs) for Hierarchical Temporal Memory (HTM) systems. In the exploration of geospatial data through Sparse Distributed Representations (SDRs), the DrawBitmap method is utilized to translate encoded geographical coordinates into visually interpretable bitmap images. This approach allows for the visualization of spatial information encoded within SDRs, offering insights into the encoded geographical regions.

Encoding Process for Geographical Coordinates
To encode and visualize geographical coordinates, the encoder parameters are set as follows, aiming to cover a specific range of latitude and longitude:
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

The bitmap images generated for geographical coordinates offer a unique view of the spatial patterns encoded within the SDRs. Now if the encoder settings is changed and the below settings is provided:
```
encoderSettings.Add("W", 21);
encoderSettings.Add("N", 40);
```
The output will be different for the same value. The bitmaps generated in this case are:

![geospatio_changed_input_parameter](https://github.com/TanzeemHasan/neocortexapi/assets/74203937/dc7b04a4-4183-4628-a8b6-784e999d6299)

The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/2220f6f125c412a236e7ed88402878c9e4cbaa61/source/UnitTestsProject/EncoderTests/GeoSpatialEncoderExperimentalTests.cs#L410). 



### Bitmap representation using Spatial Pooler

In the context of Hierarchical Temporal Memory (HTM) theory, the Spatial Pooler (SP) plays a crucial role in transforming input data into Sparse Distributed Representations (SDRs). These representations capture the essential features of the input data in a way that emphasizes structural and semantic similarities. To visually understand the transformation process and the output of the Spatial Pooler, SDRs can be represented as bitmaps. This example elaborates on how active arrays generated by the SpatialPooler are converted into bitmap images, further enhancing the understanding of HTM's processing capabilities.

### Bitmap representation of Numbers using Spatial Pooler

When a number is encoded then it goes into spatial pooler algorithm to find out the active mini columns which will be activated for that number. So, bitmap representation will present to see which mini columns will be activated. 
Here, used `SpatialPatternLearningExperiment.cs` for getting a range of inputs active columns easily. In this experiment, there is a term called `isInStableState` which defines whether all the inputs active mini columns are stable or instable. So, after `isInStableState` is true just taking all inputs active mini columns and passing the value to `DrawBitMap` function. Generating the bitmaps of all inputs active mini columns.
```csharp
if(isInStableState == true)
{
    if(flag == 2)
    {
        string basePath = Path.Combine(Environment.CurrentDirectory, "OutputOfSpatialPooler");
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }
        int[] fullArray = Enumerable.Repeat(0, mem.HtmConfig.NumColumns).ToArray();
        fullArray = spl.ConvertZerosToOnesAtIndexes(fullArray, activeColumns);
        int[,] twoDimArray = ArrayUtils.Make2DArray<int>(fullArray, (int)Math.Sqrt(mem.HtmConfig.NumColumns), (int)Math.Sqrt(mem.HtmConfig.NumColumns));
        NeoCortexUtils.DrawBitmap(twoDimArray, 10, $"{basePath}\\input {input}.png", Color.Blue, Color.Yellow, text: input.ToString());
    }
}
```
### Generating Bitmaps for Numbers from Spatial Pooler:
Bitmaps of mini columns as a SDR generated for numbers using spatial pooler algorithm. For this, spatial pattern learning experiment is used. There is a variable called isInStableState which is responsible for to decide whether all the active mini columns of input is stable or not. After isInStableState turns to true, generated bitmaps for input 0 to input 99 which were used by spatial pattern learning eperiment. Here is the example output for input 20: </br>
![Bitmap of mini columns for input 20](https://github.com/TanzeemHasan/neocortexapi/blob/master/source/MySEProject/images/input%2020.png)</br>


#### Generating Bitmaps for Image Generation and Training in Spatial Pooler: 
In the Spatial Pooler (SP), the generation of inputs for image processing and training involves several steps that lead to the creation of Sparse Distributed Representations (SDRs). The code snippets outlines how inputs are prepared, processed, and used for training the Spatial Pooler as well as generating SDRs.

```csharp
// Define a folder for training files
string trainingFolder = @"..\..\..\TestFiles\Sdr";

// In the trainingImages variable, the input images for making binarized input is found 
var trainingImages = Directory.GetFiles(trainingFolder, "*.jpeg");

```
Each image file within the directory is intended to be processed individually to generate binary representations that can be fed into the Spatial Pooler.
The Image that is used for training is these:

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

On the right side the images generated from the binarized file of the input training image can be seen, on the left is the sdr representation by SpatialPooler after feeding the training image.

Here the Alphabet T and Neumeric 3 can be seen

![Screenshot 2024-03-22 000510](https://github.com/TanzeemHasan/neocortexapi/assets/110496336/4922914d-5bc9-4a6e-9d1b-6e79f2706928)


The full example can be found [here](https://github.com/TanzeemHasan/neocortexapi/blob/8de0bf4d823b94393381b63984c8c1c5a47e330d/source/UnitTestsProject/SdrRepresentation/SpatialPoolerColumnActivityTest.cs#L19).


## Conclusion

In conclusion, [This]() function of drawbitmap can be used for any input values which can then scale and used to generate the bitmap for each and every types of data. Visualizing SDRs as bitmap images can interprete the complex patterns encoded in HTM systems. </br>
In spatial pooler, it encoded the input bits and then when these values can pass through the spatial pooler that will active the mini colums. From there the program can find the active and inactive mini columns which then can also be represent using the that similar function. 
This approach not only aids in the analysis of HTM models but also paves the way for analyzing different types of data pattern from the input. Overall, this project can generate new sparse distribution and all together a process to run the experiment with bitmap generation of SDRs whether it would be 1D or 2D. 

   
