using MyCloudProject.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyExperiment
{
    internal class ExerimentRequestMessage : IExerimentRequestMessage
    {
        public string ExperimentId { get; set; }
        public string InputFile { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public double Value3 { get; set; }

        public string ScalarEncoderAQI { get; set; }
        public string DateTimeDataRow { get; set; }
    }
}


/*
 
 {
    "ExperimentId": "sasa",
    "InputFile":"sasss",

}
 
 */ 