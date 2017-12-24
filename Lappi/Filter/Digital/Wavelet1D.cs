using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class Wavelet1D<T> where T : new() {

        private readonly DigitalSampler<T> analysisLowpass;
        private readonly DigitalSampler<T> analysisHighpass;
        private readonly DigitalSampler<T> synthesisLowpass;
        private readonly DigitalSampler<T> synthesisHighpass;

        public Wavelet1D (DigitalFilter analysisLowpass, DigitalFilter analysisHighpass, DigitalFilter synthesisLowpass,
            DigitalFilter synthesisHighpass) {
            this.analysisLowpass = new DigitalSampler<T>(analysisLowpass);
            this.analysisHighpass = new DigitalSampler<T>(analysisHighpass);
            this.synthesisLowpass = new DigitalSampler<T>(synthesisLowpass);
            this.synthesisHighpass = new DigitalSampler<T>(synthesisHighpass);
        }

        public T[][] Forward (T[] source, int steps = 1) {
            T[][] scales = new T[steps + 1][];
            scales[scales.Length - 1] = source;
            for( int i = scales.Length - 1; i >= 1; i-- ) {
                scales[i - 1] = analysisLowpass.Downsample(scales[i], 2, 0);
                scales[i] = analysisHighpass.Downsample(scales[i], 2, 1);
            }
            return scales;
        }

        public T[] Inverse (T[][] scales) {
            T[][] result = (T[][]) scales.Clone();
            for( int i = 1; i < result.Length; i++ ) {
                result[i] = InverseStep(result[i - 1], result[i]);
            }
            return result[result.Length - 1];
        }

        private T[] InverseStep (T[] low, T[] high) {
            T[] u = synthesisLowpass.Upsample(low, 2, 0, low.Length + high.Length);
            T[] v = synthesisHighpass.Upsample(high, 2, 1, low.Length + high.Length);
            return u.Add(v);
        }

    }

}
