using Jacobi.Vst3.Core;

namespace Jacobi.Vst3.Plugin
{
    public class ParameterValueInfo
    {
        protected ParameterValueInfo()
        {
        }

        public ParameterValueInfo(int precision = 4)
        {
            MinValue = 0.0;
            MaxValue = 1.0;
            Precision = precision;
        }

        public ParameterInfo ParameterInfo;

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public int Precision { get; set; }
    }
}
