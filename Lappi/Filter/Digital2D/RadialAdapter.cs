using System;
using System.Diagnostics.CodeAnalysis;

using Lappi.Filter.Analog;
using Lappi.Util;

namespace Lappi.Filter.Digital2D {

    public class RadialAdapter : DigitalFilter2D {

        public int Left { get; }
        public int Right { get; }
        public int Top { get; }
        public int Bottom { get; }
        public double[,] Coefficients { get; }

        public RadialAdapter (AnalogFilter filter, double scale) {
            Left = NonZeroLeft(filter, scale);
            Right = NonZeroRight(filter, scale);
            Top = NonZeroLeft(filter, scale);
            Bottom = NonZeroRight(filter, scale);
            Coefficients = Arrays.New(Right - Left + 1, Bottom - Top + 1,
                (x, y) => filter[Math.Sqrt(Lang.Sqr(x + Left) + Lang.Sqr(y + Top)) / scale]);
        }

        public double this [int x, int y] => Coefficients[x - Left, y - Top];

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
