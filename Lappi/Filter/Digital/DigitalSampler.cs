using System;
using System.Diagnostics.CodeAnalysis;

namespace Lappi.Filter.Digital {

    public class DigitalSampler {

        private readonly DigitalFilter filter;

        public DigitalSampler (DigitalFilter filter) {
            this.filter = filter;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public double Sample (double[] v, int center) {
            double result = 0;
            double sum = 0;
            int left = Math.Max(center + filter.Left, 0);
            int right = Math.Min(center + filter.Right, v.Length - 1);
            Func<int, double> function = filter.Function;
            for( int index = left; index <= right; index++ ) {
                double weight = function(index - center);
                result += v[index] * weight;
                sum += Math.Abs(weight);
            }
            return sum == 0 ? 0 : result / sum;
        }

        public double[] Convolute (double[] source) {
            double[] result = new double[source.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = Sample(source, i);
            }
            return result;
        }

    }

}
