using System;
using System.Collections.Generic;
using System.Text;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the contract for the message request that will run your experiment.
    /// </summary>
    public interface IExerimentRequestMessage
    {
        string ExperimentId { get; set; }

        string InputFile { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        string Value1 { get; set; }
        string Value2 { get; set; }
        double Value3 { get; set; }

        string ScalarEncoderAQI { get; set; }
        string DateTimeDataRow { get; set; }

    }
}
