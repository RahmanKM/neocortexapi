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

        #endregion

        #region Properties

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public BinaryEncoder()
        {

        }

        public BinaryEncoder(Dictionary<string, object> encoderSettings)
        {
            this.Initialize(encoderSettings);
        }

        public override void AfterInitialize()
        {

        }

        /// <summary>
        /// Sample encoder. It encodes specified value to the binary code sequence.
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public override int[] Encode(object inputData)
        {
            if (inputData == null)
                throw new ArgumentException("inputData cannot be empty!");

            double val;

            if (!double.TryParse(inputData as string, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
                throw new ArgumentException($"Value {inputData} cannot be casted to integer.");

            string binary = Convert.ToString((int)val, 2);

            binary = binary.PadLeft(this.N, '0');

            List<int> result = new List<int>();
            foreach (var chr in binary)
            {
                result.Add(chr == '1' ? 1 : 0);
            }

            return result.ToArray();
        }

        public override List<B> GetBucketValues<B>()
        {
            throw new NotImplementedException();
        }

        public override int Width
        {
            get
            {
                return this.N;
            }
        }


        public override bool IsDelta
        {
            get { return false; }
        }
        #endregion
    }
}
