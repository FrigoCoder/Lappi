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
            Left = AnalogFilters.NonZeroLeft(analog, scale);
            Right = AnalogFilters.NonZeroRight(analog, scale);
            Radius = Math.Max(Math.Abs(Left), Math.Abs(Right));
            Coefficients = Arrays.New(Right - Left + 1, i => analog[(i + Left) / scale]);
        }

        public override string ToString () => GetType().Name + "{Left = " + Left + ", Right = " + Right + ", Radius = " + Radius +
            ", Coefficients=[" + string.Join(", ", Coefficients) + "]}";

        public double this [int x] => Coefficients[x - Left];

    }

}
