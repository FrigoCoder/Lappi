using System.Linq;

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

        public Laplacian2D (DigitalFilter analysis, DigitalFilter synthesis) : this(new DigitalSampler2DSeparable<T>(new DigitalSampler<T>(analysis)),
            new DigitalSampler2DSeparable<T>(new DigitalSampler<T>(synthesis))) {
        }

        public Image<T>[] Forward (Image<T> image, int steps = 1) {
            Image<T>[] scales = new Image<T>[steps + 1];
            scales[0] = image;
            for( int i = 0; i < scales.Length - 1; i++ ) {
                scales[i + 1] = analysis.Downsample(scales[i], 2, 0);
                scales[i] -= synthesis.Upsample(scales[i + 1], 2, 0, scales[i].Xs, scales[i].Ys);
            }
            return scales.Reverse().ToArray();
        }

        public Image<T> Inverse (Image<T>[] scales) {
            Image<T>[] result = (Image<T>[]) scales.Clone();
            for( int i = 1; i < result.Length; i++ ) {
                result[i] += synthesis.Upsample(result[i - 1], 2, 0, result[i].Xs, result[i].Ys);
            }
            return result[result.Length - 1];
        }

    }

}
