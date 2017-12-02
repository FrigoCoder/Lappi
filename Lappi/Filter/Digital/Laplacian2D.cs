using System.Linq;

using Lappi.Image;

namespace Lappi.Filter.Digital {

    public class Laplacian2D<T> where T : new() {

        private readonly DigitalSampler2D<T> analysis;
        private readonly DigitalSampler2D<T> synthesis;

        public Laplacian2D (Filter2D analysis, Filter2D synthesis) {
            this.analysis = new DigitalSampler2D<T>(analysis);
            this.synthesis = new DigitalSampler2D<T>(synthesis);
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
