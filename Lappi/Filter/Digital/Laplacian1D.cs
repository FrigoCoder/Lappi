using System;
using System.Linq;

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
            double[] difference = source.Zip(upsampled, (x, y) => x - y).ToArray();
            return Tuple.Create(downsampled, difference);
        }

    }

}
