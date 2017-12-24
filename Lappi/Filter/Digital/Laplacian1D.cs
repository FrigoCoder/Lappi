using System.Linq;

using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class Laplacian1D<T> where T : new() {

        private readonly DigitalSampler<T> analysis;
        private readonly DigitalSampler<T> synthesis;

        public Laplacian1D (DigitalFilter analysis, DigitalFilter synthesis) {
            this.analysis = new DigitalSampler<T>(analysis);
            this.synthesis = new DigitalSampler<T>(synthesis);
        }

        public T[][] Forward (T[] source, int steps = 1) {
            T[][] scales = new T[steps + 1][];
            scales[0] = source;
            for( int i = 0; i < scales.Length - 1; i++ ) {
                scales[i + 1] = analysis.Downsample(source, 2, 0);
                scales[i] = scales[i].Sub(analysis.Upsample(scales[i + 1], 2, 0, scales[i].Length));
            }
            return scales.Reverse().ToArray();
        }

        public T[] Inverse (T[][] scales) {
            for( int i = 1; i < scales.Length; i++ ) {
                scales[i] = synthesis.Upsample(scales[i - 1], 2, 0, scales[i].Length).Add(scales[i]);
            }
            return scales[scales.Length - 1];
        }

    }

}
