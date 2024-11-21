using System;
using System.Collections.Generic;
using System.Text;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the contract for the experiment request message.
    /// This interface is used to structure the input data required to execute an experiment.
    /// It includes details such as experiment ID, input file, descriptive metadata, and various parameter values.
    /// </summary>
    public interface IExperimentRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the experiment request.
        /// This is used to distinguish between multiple experiment requests.
        /// </summary>
        string ExperimentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the experiment.
        /// Provides a human-readable identifier for the experiment.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the experiment.
        /// Offers additional context about the purpose or configuration of the experiment.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the first configurable value for the experiment 1.
        /// Used for parameters that control specific aspects of the experiment.
        /// </summary>
        string Value1 { get; set; }

        /// <summary>
        /// Gets or sets the second configurable value for the experiment 2.
        /// Similar to Value1, this is another parameter for experiment configuration.
        /// </summary>
        string Value2 { get; set; }

        /// <summary>
        /// Gets or sets the third configurable value for the experiment 3.
        /// Unlike Value1 and Value2, this is a double value used for numeric configurations.
        /// </summary>
        double Value3 { get; set; }

        /// <summary>
        /// Gets or sets the scalar encoder parameter for AQI (Air Quality Index).
        /// Used to preprocess or encode AQI data for input into the experiment model.
        /// </summary>
        string ScalarEncoderAQI { get; set; }

        /// <summary>
        /// Gets or sets the date and time information for the data row being processed.
        /// This is useful for time-series data or experiments dependent on temporal information.
        /// </summary>
        string DateTimeDataRow { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the queue message associated with the experiment request.
        /// </summary>
        string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the pop receipt acknowledging the retrieval of the queue message.
        /// </summary>
        string PopReceipt { get; set; }
    }
}

