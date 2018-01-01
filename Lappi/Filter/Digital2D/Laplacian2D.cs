using Lappi.Filter.Digital;
using Lappi.Image;

namespace Lappi.Filter.Digital2D {

    public class Laplacian2D<T> where T : new() {

        private readonly Sampler2D<T> analysis;
        private readonly Sampler2D<T> synthesis;

        public Laplacian2D (Sampler2D<T> analysis, Sampler2D<T> synthesis) {
            this.analysis = analysis;
            this.synthesis = synthesis;
        }

        public Laplacian2D (DigitalFilter analysis, DigitalFilter synthesis) : this(new SeparableSampler<T>(new DigitalSampler<T>(analysis)),
            new SeparableSampler<T>(new DigitalSampler<T>(synthesis))) {
        }

        public Laplacian2D (DigitalFilter2D analysis, DigitalFilter2D synthesis) : this(new NonSeparableSampler<T>(analysis),
            new NonSeparableSampler<T>(synthesis)) {
        }

        public Image<T>[] Forward (Image<T> image, int steps = 1) {
            Image<T>[] scales = new Image<T>[steps + 1];
            scales[scales.Length - 1] = image;
            for( int i = scales.Length - 1; i >= 1; i-- ) {
                scales[i - 1] = analysis.Downsample(scales[i]);
                scales[i] -= synthesis.Upsample(scales[i - 1], scales[i].Xs, scales[i].Ys);
            }
            return scales;
        }

        public Image<T> Inverse (Image<T>[] scales) {
            Image<T>[] result = (Image<T>[]) scales.Clone();
            for( int i = 1; i < result.Length; i++ ) {
                result[i] += synthesis.Upsample(result[i - 1], result[i].Xs, result[i].Ys);
            }
            return result[result.Length - 1];
        }

    }

}
