using System;

namespace Lappi.Filter.Digital {

    public class Laplacian1D {

        private readonly DigitalSampler analysis;
        private readonly DigitalSampler synthesis;

        public Laplacian1D (DigitalFilter analysis, DigitalFilter synthesis) {
            this.analysis = new DigitalSampler(analysis);
            this.synthesis = new DigitalSampler(synthesis);
        }

        public Tuple<double[], double[]> Transform (double[] source) {
            double[] downsampled = analysis.Downsample(source, 2, 0);
            double[] upsampled = synthesis.Upsample(downsampled, 2, 0);
            double[] difference = GetDifference(source, upsampled);
            return Tuple.Create(downsampled, difference);
        }

        private double[] GetDifference (double[] source, double[] upsampled) {
            double[] difference = new double[source.Length];
            for( int i = 0; i < difference.Length; i++ ) {
                difference[i] = source[i] - upsampled[i];
            }
            return difference;
        }

    }

}
