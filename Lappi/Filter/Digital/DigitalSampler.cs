using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lappi.Filter.Digital {

    public class DigitalSampler : DigitalSampler<double> {

        public DigitalSampler (DigitalFilter filter) : base(filter) {
        }

    }

    public class DigitalSampler<T> where T : new() {

        private readonly DigitalFilter filter;

        public DigitalSampler (DigitalFilter filter) {
            this.filter = filter;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public T Sample (T[] source, int center) {
            T result = new T();
            double sum = 0;
            int left = Math.Max(center + filter.Left, 0);
            int right = Math.Min(center + filter.Right, source.Length - 1);
            Func<int, double> function = filter.Kernel;
            for( int index = left; index <= right; index++ ) {
                double weight = function(index - center);
                result += (dynamic) source[index] * weight;
                sum += Math.Abs(weight);
            }
            return sum == 0 ? 0 : (dynamic) result / sum;
        }

        public T[] Convolute (T[] source) {
            T[] result = new T[source.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = Sample(source, i);
            }
            return result;
        }

        public T[] Downsample (T[]source, int factor, int shift) {
            List<T> result = new List<T>();
            for( int i = shift; i < source.Length; i += factor ) {
                result.Add(Sample(source, i));
            }
            return result.ToArray();
        }

    }

}
