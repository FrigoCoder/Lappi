using System;
using System.Diagnostics.CodeAnalysis;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

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
            Coefficients = new double[Right - Left + 1, Bottom - Top + 1];
            for( int x = Left; x <= Right; x++ ) {
                for( int y = Top; y <= Bottom; y++ ) {
                    double d = Math.Sqrt(x * x + y * y);
                    Coefficients[x - Left, y - Top] = filter.Kernel(d / scale);
                }
            }
        }

        public double this [int x, int y] => Coefficients[x - Left, y - Top];

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static int NonZeroLeft (AnalogFilter analog, double scale) {
            int left = Convert.ToInt32(Math.Ceiling(analog.Left * scale));
            while( analog.Kernel(left / scale) == 0 ) {
                left++;
            }
            return left;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static int NonZeroRight (AnalogFilter analog, double scale) {
            int right = Convert.ToInt32(Math.Floor(analog.Right * scale));
            while( analog.Kernel(right / scale) == 0 ) {
                right--;
            }
            return right;
        }

    }

}
