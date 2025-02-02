/*
 * BinaryEncoder Class Summary:
 * ----------------------------
 * The BinaryEncoder class is part of the MyExperiment.SEProject namespace and extends the EncoderBase class.
 * It is designed to convert a numeric input (provided as a string) into its binary representation.
 *
 * Key features include:
 * 1. Constructors:
 *    - A default constructor for simple instantiation.
 *    - A parameterized constructor that accepts a dictionary of encoder settings, which initializes the encoder.
 *
 * 2. Initialization:
 *    - AfterInitialize(): A method override intended for any post-initialization steps (currently empty).
 *
 * 3. Encoding Functionality:
 *    - Encode(object inputData): 
 *         - Validates the input ensuring it is not null.
 *         - Parses the input string to a double.
 *         - Converts the integer part of the parsed value to its binary string representation.
 *         - Pads the binary string with leading zeros to meet a specified width (N).
 *         - Converts the binary string into an array of integers, with each binary digit represented as 0 or 1.
 *
 * 4. Additional Members:
 *    - GetBucketValues<B>(): A placeholder method that throws NotImplementedException, indicating future support for bucket values.
 *    - Width: A property that returns the fixed bit-length (N) of the encoder.
 *    - IsDelta: A property indicating that delta encoding is not supported (always returns false).
 *
 * Overall, the BinaryEncoder class provides a modular approach to encoding numerical data into a binary format,
 * ensuring a consistent fixed-length binary representation suitable for applications that require such data processing.
 */

using NeoCortexApi.Encoders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExperiment.SEProject
{
    public class BinaryEncoder : EncoderBase
    {
        #region Private Fields
        // (Private fields can be declared here for internal use)
        #endregion

        #region Properties
        // (Properties specific to BinaryEncoder can be defined here)
        #endregion

        #region Private Methods
        // (Private helper methods for internal logic can be defined here)
        #endregion

        #region Public Methods

        // Default constructor
        public BinaryEncoder()
        {

        }

        // Constructor that initializes the encoder with a set of settings
        public BinaryEncoder(Dictionary<string, object> encoderSettings)
        {
            this.Initialize(encoderSettings);
        }

        // Post-initialization hook for additional setup (currently empty)
        public override void AfterInitialize()
        {

        }

        /// <summary>
        /// Encodes the specified value to a binary code sequence.
        /// </summary>
        /// <param name="inputData">The input data expected as a string representing a numeric value.</param>
        /// <returns>An array of integers representing the binary sequence.</returns>
        public override int[] Encode(object inputData)
        {
            // Ensure inputData is not null
            if (inputData == null)
                throw new ArgumentException("inputData cannot be empty!");

            double val;

            // Attempt to parse the input data (as a string) to a double using invariant culture formatting.
            if (!double.TryParse(inputData as string, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
                throw new ArgumentException($"Value {inputData} cannot be casted to integer.");

            // Convert the integer part of the parsed value into its binary string representation.
            string binary = Convert.ToString((int)val, 2);

            // Pad the binary string with zeros to ensure it meets the fixed width (N).
            binary = binary.PadLeft(this.N, '0');

            // Convert the binary string into an integer array.
            List<int> result = new List<int>();
            foreach (var chr in binary)
            {
                result.Add(chr == '1' ? 1 : 0);
            }

            return result.ToArray();
        }

        // Placeholder method to get bucket values, not yet implemented.
        public override List<B> GetBucketValues<B>()
        {
            throw new NotImplementedException();
        }

        // Returns the fixed width (bit-length) of the encoder.
        public override int Width
        {
            get
            {
                return this.N;
            }
        }

        // Indicates that delta encoding is not supported.
        public override bool IsDelta
        {
            get { return false; }
        }
        #endregion
    }
}
