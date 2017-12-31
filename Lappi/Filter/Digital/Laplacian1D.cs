using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class Laplacian1D<T> where T : new() {

        private readonly Sampler1D<T> analysis;
        private readonly Sampler1D<T> synthesis;

        public Laplacian1D (DigitalFilter analysis, DigitalFilter synthesis) :
            this(new DigitalSampler<T>(analysis), new DigitalSampler<T>(synthesis)) {
        }

        public Laplacian1D (Sampler1D<T> analysis, Sampler1D<T> synthesis) {
            this.analysis = analysis;
            this.synthesis = synthesis;
        }

        public T[][] Forward (T[] source, int steps = 1) {
            T[][] scales = new T[steps + 1][];
            scales[scales.Length - 1] = source;
            for( int i = scales.Length - 1; i >= 1; i-- ) {
                scales[i - 1] = analysis.Downsample(scales[i], 2, 0);
                scales[i] = scales[i].Sub(synthesis.Upsample(scales[i - 1], 2, 0, scales[i].Length));
            }
            return scales;
        }

        public T[] Inverse (T[][] scales) {
            T[][] result = (T[][]) scales.Clone();
            for( int i = 1; i < result.Length; i++ ) {
                result[i] = synthesis.Upsample(result[i - 1], 2, 0, result[i].Length).Add(result[i]);
            }
            return result[result.Length - 1];
        }

    }

}
