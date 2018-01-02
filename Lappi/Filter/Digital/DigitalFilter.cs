using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Lappi.Filter.Analog;
using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class DigitalFilter {

        public readonly int Left;
        public readonly int Right;
        public readonly int Radius;
        public readonly double[] Coefficients;

        public DigitalFilter (int center, double[] coefficients) {
            Left = 0 - center;
            Right = coefficients.Length - 1 - center;
            Radius = Math.Max(Math.Abs(Left), Math.Abs(Right));
            Coefficients = coefficients;
        }

        public DigitalFilter (AnalogFilter analog, double scale) {
            Left = NonZeroLeft(analog, scale);
            Right = NonZeroRight(analog, scale);
            Radius = Math.Max(Math.Abs(Left), Math.Abs(Right));
            Coefficients = Arrays.New(Right - Left + 1, i => analog[(i + Left) / scale]);
        }

        public DigitalFilter Normalize () {
            double sum = Coefficients.Sum();
            return new DigitalFilter(-Left, Coefficients.Select(x => x / sum).ToArray());
        }

        public string ToString () => GetType().Name + "{Left = " + Left + ", Right = " + Right + ", Radius = " + Radius + ", Coefficients=[" +
            string.Join(", ", Coefficients) + "]}";

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
