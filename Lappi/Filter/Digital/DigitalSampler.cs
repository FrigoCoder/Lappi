using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class DigitalSampler<T> where T : new() {

        private readonly DigitalFilter filter;
        private readonly double[] coefficients;
        private readonly IntegralArray<double> sums;
        private readonly IntegralArray<double> altSums;

        public DigitalSampler (DigitalFilter filter) {
            this.filter = filter;
            coefficients = filter.Coefficients;
            sums = new IntegralArray<double>(coefficients);
            altSums = new IntegralArray<double>(coefficients.Select((x, i) => i % 2 == 0 ? x : -x).ToArray());
        }

        public virtual T Sample (T[] source, int center) {
            Preconditions.Require(0 <= center && center < source.Length);
            if( 0 <= center + filter.Left && center + filter.Right < coefficients.Length ) {
                return SampleInner(source, center);
            }
            return SampleBoundary(source, center);
        }

        public T[] Convolute (T[] source) => Arrays.New(source.Length, i => Sample(source, i));

        public T[] Downsample (T[] source, int factor, int shift) =>
            Arrays.New((source.Length - shift + factor - 1) / factor, i => Sample(source, i * factor + shift));

        public T[] Upsample (T[] source, int factor, int shift) => Upsample(source, factor, shift, source.Length * factor);

        public T[] Upsample (T[] source, int factor, int shift, int length) {
            T[] v = Arrays.New(length, new T());
            for( int i = 0; i < source.Length; i++ ) {
                v[i * factor + shift] = (dynamic) source[i] * factor;
            }
            return Convolute(v);
        }

        private T SampleInner (T[] source, int center) {
            int shift = center + filter.Left;
            T sum = new T();
            for( int i = 0; i < coefficients.Length; i++ ) {
                sum += (dynamic) source[shift + i] * coefficients[i];
            }
            sum /= (dynamic) Normalize(0, coefficients.Length - 1);
            return sum;
        }

        private T SampleBoundary (T[] source, int center) {
            int shift = center + filter.Left;
            int left = Math.Max(0, -shift);
            int right = Math.Min(coefficients.Length - 1, source.Length - 1 - shift);
            T sum = new T();
            for( int i = left; i <= right; i++ ) {
                sum += (dynamic) source[i + shift] * coefficients[i];
            }
            sum /= (dynamic) Normalize(left, right);
            return sum;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private double Normalize (int left, int right) {
            double sum = Math.Abs(sums.Sum(left, right));
            double altSum = Math.Abs(altSums.Sum(left, right));
            double factor = Math.Max(Math.Abs(sum), Math.Abs(altSum));
            return factor == 0 ? 1 : factor;
        }

    }

}
