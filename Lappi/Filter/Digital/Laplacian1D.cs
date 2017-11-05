using System;

namespace Lappi.Filter.Digital {

    public class Laplacian1D<T> where T : new() {

        private readonly DigitalSampler<T> analysis;
        private readonly DigitalSampler<T> synthesis;

        public Laplacian1D (DigitalFilter analysis, DigitalFilter synthesis) {
            this.analysis = new DigitalSampler<T>(analysis);
            this.synthesis = new DigitalSampler<T>(synthesis);
        }

        public Tuple<T[], T[]> Forward (T[] source) {
            T[] downsampled = analysis.Downsample(source, 2, 0);
            T[] upsampled = synthesis.Upsample(downsampled, 2, 0, source.Length);
            T[] difference = new T[source.Length];
            for( int i = 0; i < difference.Length; i++ ) {
                difference[i] = (dynamic) source[i] - upsampled[i];
            }
            return Tuple.Create(downsampled, difference);
        }

        public T[] Inverse (Tuple<T[], T[]> tuple) => Inverse(tuple.Item1, tuple.Item2);

        public T[] Inverse (T[] downsampled, T[] difference) {
            T[] upsampled = synthesis.Upsample(downsampled, 2, 0, difference.Length);
            T[] result = new T[difference.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = (dynamic) upsampled[i] + difference[i];
            }
            return result;
        }

    }

}
