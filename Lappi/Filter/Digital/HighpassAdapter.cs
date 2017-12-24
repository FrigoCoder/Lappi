using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class HighpassAdapter : DigitalFilter {

        public int Left { get; }
        public int Right { get; }
        public int Radius { get; }
        public double[] Coefficients { get; }

        public HighpassAdapter (DigitalFilter filter) {
            Left = filter.Left;
            Right = filter.Right;
            Radius = filter.Radius;
            Coefficients = CreateCoefficients(filter);
        }

        public double this [int x] => Coefficients[x - Left];

        private static double[] CreateCoefficients (DigitalFilter filter) {
            double sum = Normalize(filter);
            return Arrays.New(filter.Right - filter.Left + 1, i => i == -filter.Left ? 1 - filter[0] / sum : -filter[i + filter.Left] / sum);
        }

        private static double Normalize (DigitalFilter filter) {
            double sum = 0;
            for( int i = filter.Left; i <= filter.Right; i++ ) {
                sum += filter[i];
            }
            return sum;
        }

    }

}
