using System.Linq;

namespace Lappi.Filter.Digital {

    using DigitalSampler = DigitalSampler<double>;
    using HighpassSampler = HighpassSampler<double>;

    public class Wavelet1D {

        private readonly DigitalSampler analysisLowpass;
        private readonly DigitalSampler analysisHighpass;
        private readonly DigitalSampler synthesisLowpass;
        private readonly DigitalSampler synthesisHighpass;

        public Wavelet1D (DigitalFilter analysisLowpass, DigitalFilter analysisHighpass, DigitalFilter synthesisLowpass,
            DigitalFilter synthesisHighpass) {
            this.analysisLowpass = new DigitalSampler(analysisLowpass);
            this.analysisHighpass = new DigitalSampler(analysisHighpass);
            this.synthesisLowpass = new DigitalSampler(synthesisLowpass);
            this.synthesisHighpass = new DigitalSampler(synthesisHighpass);
        }

        public double[][] Forward (double[] source, int steps = 1) {
            double[][] scales = new double[steps + 1][];
            scales[0] = source;
            for( int i = 0; i < scales.Length - 1; i++ ) {
                scales[i + 1] = analysisLowpass.Downsample(scales[i], 2, 0);
                scales[i] = analysisHighpass.Downsample(scales[i], 2, 1);
            }
            return scales.Reverse().ToArray();
        }

        public double[] Inverse (double[][] scales) {
            for( int i = 1; i < scales.Length; i++ ) {
                double[] low = scales[i - 1];
                double[] high = scales[i];
                double[] u = synthesisLowpass.Upsample(low, 2, 0, low.Length + high.Length);
                double[] v = synthesisHighpass.Upsample(high, 2, 1, low.Length + high.Length);
                scales[i] = u.Zip(v, (x, y) => x + y).ToArray();
            }
            return scales[scales.Length - 1];
        }

    }

}
