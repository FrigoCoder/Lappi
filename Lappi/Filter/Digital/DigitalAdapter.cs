using System;

using Lappi.Filter.Analog;
using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class DigitalAdapter : DigitalFilter {

        public int Left { get; }
        public int Right { get; }
        public int Radius { get; }
        public double[] Coefficients { get; }

        public DigitalAdapter (AnalogFilter analog, double scale) {
            Left = DigitalFilters.NonZeroLeft(analog, scale);
            Right = DigitalFilters.NonZeroRight(analog, scale);
            Radius = Math.Max(Math.Abs(Left), Math.Abs(Right));
            Coefficients = Arrays.New(Right - Left + 1, i => analog[(i + Left) / scale]);
        }

        public double this [int x] => Coefficients[x - Left];

    }

}
