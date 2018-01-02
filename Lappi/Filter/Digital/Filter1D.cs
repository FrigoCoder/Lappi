using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Lappi.Filter.Analog;
using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class Filter1D {

        public readonly int Left;
        public readonly int Right;
        public readonly double[] Coefficients;

        public Filter1D (int center, double[] coefficients) {
            Left = 0 - center;
            Right = coefficients.Length - 1 - center;
            Coefficients = coefficients;
        }

        public Filter1D (AnalogFilter analog, double scale) {
            Left = NonZeroLeft(analog, scale);
            Right = NonZeroRight(analog, scale);
            Coefficients = Arrays.New(Right - Left + 1, i => analog[(i + Left) / scale]);
        }

        public Filter1D Normalize () {
            double sum = Coefficients.Sum();
            return new Filter1D(-Left, Coefficients.Select(x => x / sum).ToArray());
        }

        public override string ToString () => $"{GetType().Name}{{Left = {Left}, Right = {Right}, Coefficients=[{string.Join(", ", Coefficients)}]}}";

        public double this [int x] => Coefficients[x - Left];

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static int NonZeroLeft (AnalogFilter analog, double scale) {
            int left = Convert.ToInt32(Math.Ceiling(analog.Left * scale));
            while( analog[left / scale] == 0 ) {
                left++;
            }
            return left;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static int NonZeroRight (AnalogFilter analog, double scale) {
            int right = Convert.ToInt32(Math.Floor(analog.Right * scale));
            while( analog[right / scale] == 0 ) {
                right--;
            }
            return right;
        }

    }

}
