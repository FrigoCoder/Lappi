using System;

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
            Left = AnalogFilters.NonZeroLeft(filter, scale);
            Right = AnalogFilters.NonZeroRight(filter, scale);
            Top = AnalogFilters.NonZeroLeft(filter, scale);
            Bottom = AnalogFilters.NonZeroRight(filter, scale);
            Coefficients = Arrays.New(Right - Left + 1, Bottom - Top + 1,
                (x, y) => filter[Math.Sqrt(Lang.Sqr(x + Left) + Lang.Sqr(y + Top)) / scale]);
        }

        public double this [int x, int y] => Coefficients[x - Left, y - Top];

    }

}
