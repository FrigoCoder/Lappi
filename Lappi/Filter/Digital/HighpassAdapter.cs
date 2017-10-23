using System;

namespace Lappi.Filter.Digital {

    public class HighpassAdapter : DigitalFilter {

        public int Left { get; }
        public int Right { get; }
        public int Radius { get; }
        public Func<int, double> Kernel => x => coefficients[x - Left];
        private readonly double[] coefficients;

        public HighpassAdapter (DigitalFilter filter) {
            Left = filter.Left;
            Right = filter.Right;
            Radius = filter.Radius;
            coefficients = CreateCoefficients(filter);
        }

        private static double[] CreateCoefficients (DigitalFilter filter) {
            double[] coefficients = new double[filter.Right - filter.Left + 1];
            double sum = Normalize(filter);
            for( int i = 0; i < coefficients.Length; i++ ) {
                coefficients[i] = -filter.Kernel(i + filter.Left) / sum;
            }
            coefficients[-filter.Left] = 1 - filter.Kernel(0) / sum;
            return coefficients;
        }

        private static double Normalize (DigitalFilter filter) {
            double sum = 0;
            for( int i = filter.Left; i <= filter.Right; i++ ) {
                sum += filter.Kernel(i);
            }
            return sum;
        }

    }

}
