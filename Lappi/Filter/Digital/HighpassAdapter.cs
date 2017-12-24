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
            double[] coefficients = new double[filter.Right - filter.Left + 1];
            double sum = Normalize(filter);
            for( int i = 0; i < coefficients.Length; i++ ) {
                coefficients[i] = -filter[i + filter.Left] / sum;
            }
            coefficients[-filter.Left] = 1 - filter[0] / sum;
            return coefficients;
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
