using System;
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

        public Tuple<double[], double[]> Forward (double[] source) {
            double[] low = analysisLowpass.Downsample(source, 2, 0);
            double[] high = analysisHighpass.Downsample(source, 2, 1);
            return Tuple.Create(low, high);
        }

        public double[] Inverse (Tuple<double[], double[]> tuple) {
            return Inverse(tuple.Item1, tuple.Item2);
        }

        public double[] Inverse (double[] low, double[] high) {
            double[] v1 = synthesisLowpass.Upsample(low, 2, 0, low.Length + high.Length);
            double[] v2 = synthesisHighpass.Upsample(high, 2, 1, low.Length + high.Length);
            return v1.Zip(v2, (x, y) => x + y).ToArray();
        }

    }

}
