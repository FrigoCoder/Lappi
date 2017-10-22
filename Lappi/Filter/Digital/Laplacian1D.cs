using System;

namespace Lappi.Filter.Digital {

    public class Laplacian1D {

        private readonly DigitalSampler analysis;
        private DigitalSampler synthesis;

        public Laplacian1D (DigitalFilter analysis, DigitalFilter synthesis) {
            this.analysis = new DigitalSampler(analysis);
            this.synthesis = new DigitalSampler(synthesis);
        }

        public Tuple<double[], double[]> Transform (double[] source) {
            double[] downsampled = analysis.Downsample(source, 2, 0);
            return Tuple.Create(downsampled, new double[]{});
        }

    }

}
