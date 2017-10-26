using System;

namespace Lappi.Filter.Digital {

    public class Wavelet1D {

        private readonly DigitalSampler lowpassEven;
        private readonly DigitalSampler lowpassOdd;

        public Wavelet1D (DigitalFilter lowpassEven, DigitalFilter lowpassOdd) {
            this.lowpassEven = new DigitalSampler(lowpassEven);
            this.lowpassOdd = new DigitalSampler(lowpassOdd);
        }

        public Tuple<double[], double[]> Transform (double[] source) {
            double[] lowpass = lowpassEven.Downsample(source, 2, 0);
            double[] highpass = lowpassOdd.DownsampleHighpass(source, 2, 1);
            return Tuple.Create(lowpass, highpass);
        }

    }

}
