using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class Wavelet1D<T> where T : new() {

        private readonly Sampler1D<T> analysisLowpass;
        private readonly Sampler1D<T> analysisHighpass;
        private readonly Sampler1D<T> synthesisLowpass;
        private readonly Sampler1D<T> synthesisHighpass;

        public Wavelet1D (DigitalFilter analysisLowpass, DigitalFilter analysisHighpass, DigitalFilter synthesisLowpass,
            DigitalFilter synthesisHighpass) : this(new DigitalSampler<T>(analysisLowpass), new DigitalSampler<T>(analysisHighpass),
            new DigitalSampler<T>(synthesisLowpass), new DigitalSampler<T>(synthesisHighpass)) {
        }

        public Wavelet1D (Sampler1D<T> analysisLowpass, Sampler1D<T> analysisHighpass, Sampler1D<T> synthesisLowpass,
            Sampler1D<T> synthesisHighpass) {
            this.analysisLowpass = analysisLowpass;
            this.analysisHighpass = analysisHighpass;
            this.synthesisLowpass = synthesisLowpass;
            this.synthesisHighpass = synthesisHighpass;
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
