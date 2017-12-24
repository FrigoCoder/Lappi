using System;
using System.Diagnostics.CodeAnalysis;

using Lappi.Filter.Analog;

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
                    this[x, y] = filter[Math.Sqrt(x * x + y * y) / scale];
                }
            }
        }

        public double this [int x, int y] {
            get => Coefficients[x - Left, y - Top];
            private set => Coefficients[x - Left, y - Top] = value;
        }

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
