# ML23/24-06 Bitmap Representation of Sparse Distributed Representations (SDRs) - Azure Cloud Implementation
## Introduction
In the world of data science and machine learning, Sparse Distributed Representations (SDRs) are a powerful way to encode and represent information. But turning these SDRs into visual formats, like bitmaps, can be a complex process. To make this easier for others to understand and use, our project, "Improving Documentation of SDR to Bitmap," was born.

The goal was simple: make it easier for people to grasp how SDRs are converted into bitmaps. This involved not just improving the documentation, but also enhancing how these methods are implemented and used. We realized that clear, accessible documentation is essential for anyone who wants to work with SDRs, especially those who might not be experts in the field.

We took advantage of the cloud’s ability to scale and provide easy access to powerful computing resources. This means that now, instead of needing high-end hardware to run these processes, anyone can do it from virtually anywhere, using the cloud. This is especially useful when working with large datasets or complex visualizations that require more processing power.

Moreover, I uploaded SDRs to a training container in the cloud. So now, users can easily access, train, and visualize their data directly in the cloud, making the workflow smoother and more efficient.

## Recap (Software Engineering Project)
If you need to obtain a copy of our project on your own system, use these links in order to carry out development and testing. Look at the notes on how to deploy the project and experiment with it on a live system. These are the relevant links:

- Project Documentation: [Documentation](https://github.com/RahmanKM/neocortexapi/blob/master/source/MySEProject/Documentation/ML_23-24-06_Improve_Samples_and_Documentation_for_SDR_representation_SDR-to-Bitmap-TeamKT.pdf)  

## What is this experiment about
This project explores the necessity and methodology of adapting the Spatial Distributed Representation (SDR) to Bitmap conversion methods to function optimally in cloud environments. This adaptation is crucial for leveraging scalable cloud resources to handle intensive computational tasks more efficiently.

## Why Adapt SDR to Bitmap for the Cloud?
Running SDR to Bitmap conversions on local machines limits the scalability and efficiency of processing large datasets. By migrating these operations to the cloud, we harness the power of distributed computing, leading to faster processing times and reduced local resource consumption.

## Methodology

### Stream-Based File Handling
Originally, our methods involved direct file handling, which was not optimal for cloud-based execution. We transitioned to stream-based processing to enhance flexibility and performance. This change facilitates direct interactions with cloud storage without the need for intermediate storage, reducing I/O overhead.

#### Modified Method Example
```csharp
public byte[] EncodeFullDateTimeTest(int w, double r, Object input, int[] expectedOutput) {
    // Initializations and encoder settings
    var encoder = new DateTimeEncoder(getFullEncoderSettings(), DateTimeEncoder.Precision.Days);
    var result = encoder.Encode(DateTimeOffset.Parse(input.ToString()));

    // Handling 2D drawing limitation
    if ((result.Length % 2) != 0)
        throw new ArgumentException("Only odd number of bits is allowed.");

    // Stream-based bitmap generation
    using (MemoryStream memoryStream = new MemoryStream()) {
        DrawBitmap(result, memoryStream);
        return memoryStream.ToArray();
    }
}

```
We used memorystream to hold the temprarily before uploading to cloud 
```csharp
// Utilizing MemoryStream to hold data temporarily
using (MemoryStream memoryStream = new MemoryStream()) {
    // Drawing the bitmap directly into the memory stream
    NeoCortexUtils1.DrawBitmap(twoDimArray, 1024, 1024, memoryStream, Color.Black, Color.Yellow);
    // Converting the memory stream to an array for further use
    return memoryStream.ToArray();
}
```

#### Cloud Storage Integration
This facilitates secure and scalable handling of large data volumes by directly interacting with Azure Blob Storage.
```csharp
// Async method to upload the converted bitmap data to Azure Blob Storage
public async Task UploadResultFile(string fileName, byte[] data) {
    // Creating a blob client instance to interact with the blob storage
    BlobClient blobClient = new BlobContainerClient(connectionString, containerName).GetBlobClient(fileName);
    // Uploading the data asynchronously
    await blobClient.UploadAsync(new BinaryData(data));
}

```

#### Queue Listener Implementation
Handles incoming tasks, processes them, and uploads results to cloud storage, ensuring efficient task management.
```csharp
// Method to listen and process messages from the cloud queue
public async Task RunQueueListener(CancellationToken cancelToken) {
    // Establish connection to the queue
    QueueClient queueClient = new QueueClient(this.config.StorageConnectionString, this.config.Queue);

    // Continuously process messages until a cancellation request is made
    while (!cancelToken.IsCancellationRequested) {
        // Receive a message from the queue
        QueueMessage message = await queueClient.ReceiveMessageAsync();

        if (message != null) {
            // Decode the message text
            string msgTxt = Encoding.UTF8.GetString(message.Body.ToArray());
            // Log the received message
            this.logger?.LogInformation($"Received the message {msgTxt}");

            // Deserialize the message to get details for further processing
            ExerimentRequestMessage request = JsonSerializer.Deserialize<ExerimentRequestMessage>(msgTxt);
            // Perform the experiment based on the received file details
            IExperimentResult result = await this.Run(request.InputFile);

            // SE project code starts executing from here
            await this.seProject(dateTimeFile, scalarEncoderAQIFile);

            // Upload the results to azure cloud table
            await storageProvider.UploadExperimentResult(result);

            // Deleting the message post-processing to prevent reprocessing
            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        } else {
            // If no messages, wait before the next check
            await Task.Delay(500);
            logger?.LogTrace("Queue empty...");
        }
    }
}

```
The queue message trigger the ```seProject``` method and then in turns process all the sdr to bitmap method and upload all the bitmaps to azure blob storage
```csharp
/// <summary>
        /// Executes a series of encoding and visualization tests, generates bitmap images, 
        /// and uploads them to the cloud storage. This method runs multiple encoding tests including:
        /// 1. Encoding and visualizing a single scalar value.
        /// 2. Generating a 1D bitmap using a scalar encoder.
        /// 3. Running DateTime encoding tests.
        /// 4. Running scalar encoding tests with AQI data.
        /// 5. Generating and uploading geospatial data visualizations.
        /// </summary>
        /// <param name="dateTimeFileName">The JSON file name containing the DateTime data for encoding tests.</param>
        /// <param name="aqiFileName">The JSON file name containing the AQI data for scalar encoding tests.</param>
        public async Task seProject(string dateTimeFileName, string aqiFileName)
        {
            // Generate bitmap with binary encoder
            SdrToBitmap sdrToBitmap = new SdrToBitmap();
            byte[] bitmapData = sdrToBitmap.EncodeAndVisualizeSingleValueTest();

            // Upload the bitmap to the blob container
            string bitmapFileName = "EncodedValueVisualization_ScalarEncoder_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";
            await storageProvider.UploadResultFile(bitmapFileName, bitmapData);

            // Generate 1D Bitmap with binary encoder
            byte[] bitmapData1D = sdrToBitmap.EncodeAndVisualizeSingleValueTest3();
            string bitmapFileName1D = "Draw1DBitmap_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";
            await storageProvider.UploadResultFile(bitmapFileName1D, bitmapData1D);

            // DateTime encoder test 
            await this.RunEncodeTestsAsync(dateTimeFileName);

            // ScalarEncoder AQI Test Bitmap run
            await this.RunScalarAQITestsAsync(aqiFileName);

            // GeoSpatial Data
            byte[] bitmapGeoSpatialData = sdrToBitmap.GeoSpatialEncoderTestDrawBitMap();

            // Upload the bitmap to the blob container
            string bitmapGeoSpatialFileName = "GeoSpatialBitmap_" + DateTimeOffset.UtcNow.ToString("yyyyMMdd_HHmmss_fff") + ".png";
            await storageProvider.UploadResultFile(bitmapGeoSpatialFileName, bitmapGeoSpatialData);
        }
```

In this experiment we have implemented our Software Engineering project in Azure cloud. Below is the total algorithm of the project:

![image](images/mushfiqs_diagram.png)

## Information about our Azure accounts and their components

|  |  |  |
| --- | --- | --- |
| Resource Group | ```RG-Rahman-Khan``` | --- |
| Container Registry | ```rahmankmc``` | --- |
| Container Registry server | ```rahmankmc.azurecr.io``` | --- |
| Container Instance | ```rahmankm3rd``` | --- |
| Storage account | ```ccrahmankm``` | --- |
| Queue storage | ```rahmanqueue``` | Queue which containes trigger message |
| Training container | ```rahmantrainingcontainer``` | Container used to store training data|
| Result container | ```rahmanresultcontainer``` | Container used to store result data|
| Table storage | ```rahmantable``` | Table used to store all output datas and results |

The experiment Docker image can be pulled from the Azure Container Registry using the instructions below.
~~~
docker login rahmankm.azurecr.io -u rahmankmc -p VUzpuJyxzCkdY+gktuALRAYxp6sfPpGVfSPVlR7OSo+ACRDYM1TB
~~~
~~~
docker pull rahmankmc.azurecr.io/mycloudproject:tag-rahmankm
~~~

## How to run the experiment
## Step1 : Message input from azure portal
add a message to queues inside Azure storage account.
p.s Uncheck "Encode the message body in Base64"

**How to add message :** 

Azure portal > Home > RG-Rahman-Khan | Queues > rahmanqueue> Add message
![image](images/queue_message.png)

### Queue Message that will trigger the experiment:
~~~json
{
  "ExperimentId": "1",
  "InputFile": "runccproject",
  "Description": "SDR to Bitmap",
  "ProjectName": "ML23/24-06. Improve samples and documentation for SDR representation",
  "GroupName": "rahmanKM",
  "Students": [ "Rahman Shahriar Khan" ],
  "DateTimeDataRow": "DateTimeDataRow.json",
  "ScalarEncoderAQI": "ScalarEncoderAQI.json"
}
~~~

Go to "rahmankm3rd ," "Containers," and "logs" to make sure the experiment is being run from a container instance. 

![image](images/container_instance_started.png)

when the experiment  is successful bellow message(Experiment complete successfully) will be shown. Experiment successfully

![image](images/container_ran_successfully.png)

## Step2: Describe the Experiment Training Input Container

Before the experiments are starting, the input files are stored in ```rahmantrainingcontainer``` 

After the queue message received, this files are read from the container and the project is started.

![image](images/training_container_file_storage.png)

## Step3: Describe the Experiment Result Output Container

after the experiments are completed, the result file is stored in Azure storage blob containers 

![image](images/result_container_file_storage.png)

the result data are also subsequently uploaded into a database table named "teamastable"

![image](images//table_storage.png)
