using System;
using System.Linq;

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
            scales[0] = source;
            for( int i = 0; i < scales.Length - 1; i++ ) {
                scales[i + 1] = analysisLowpass.Downsample(scales[i], 2, 0);
                scales[i] = analysisHighpass.Downsample(scales[i], 2, 1);
            }
            return scales.Reverse().ToArray();
        }

        public T[] Inverse (T[][] scales) {
            for( int i = 1; i < scales.Length; i++ ) {
                scales[i] = InverseStep(scales[i - 1], scales[i]);
            }
            return scales[scales.Length - 1];
        }

        private T[] InverseStep (T[] low, T[] high) {
            T[] u = synthesisLowpass.Upsample(low, 2, 0, low.Length + high.Length);
            T[] v = synthesisHighpass.Upsample(high, 2, 1, low.Length + high.Length);
            return Add(u, v);
        }

        private static T[] Add (T[] u, T[] v) {
            if( u.Length != v.Length ) {
                throw new ArgumentException();
            }
            T[] result = new T[u.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = (dynamic) u[i] + v[i];
            }
            return result;
        }

    }

}
