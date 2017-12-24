using System;

namespace Lappi.Filter.Digital {

    public class CoefficientAdapter : DigitalFilter {

        public int Left { get; }
        public int Right { get; }
        public int Radius { get; }
        public double[] Coefficients { get; }

        public CoefficientAdapter (int center, double[] coefficients) {
            Left = 0 - center;
            Right = coefficients.Length - 1 - center;
            Radius = Math.Max(Math.Abs(Left), Math.Abs(Right));
            Coefficients = coefficients;
        }

        public double this [int x] => Coefficients[x - Left];

    }

}
