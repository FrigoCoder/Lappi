using System.Linq;

using Lappi.Image;

namespace Lappi.Filter.Digital {

    public class Laplacian2D<T> where T : new() {

        private readonly DigitalSampler<T> analysis;
        private readonly DigitalSampler<T> synthesis;

        public Laplacian2D (DigitalFilter analysis, DigitalFilter synthesis) {
            this.analysis = new DigitalSampler<T>(analysis);
            this.synthesis = new DigitalSampler<T>(synthesis);
        }

        public Image<T>[] Forward (Image<T> image, int steps = 1) {
            Image<T>[] scales = new Image<T>[steps + 1];
            scales[0] = image;
            for( int i = 0; i < scales.Length - 1; i++ ) {
                scales[i + 1] = Downsample(scales[i]);
                scales[i] -= Upsample(scales[i + 1], scales[i].Xs, scales[i].Ys);
            }
            return scales.Reverse().ToArray();
        }

        public Image<T> Inverse (Image<T>[] scales) {
            Image<T>[] result = (Image<T>[]) scales.Clone();
            for( int i = 1; i < result.Length; i++ ) {
                result[i] += Upsample(result[i - 1], result[i].Xs, result[i].Ys);
            }
            return result[result.Length - 1];
        }

        private Image<T> Downsample (Image<T> image) {
            Image<T> half = new Image<T>(image.Xs / 2, image.Ys);
            for( int y = 0; y < image.Ys; y++ ) {
                half.Rows[y] = analysis.Downsample(image.Rows[y], 2, 0);
            }

            Image<T> quarter = new Image<T>(image.Xs / 2, image.Ys / 2);
            for( int x = 0; x < image.Xs / 2; x++ ) {
                quarter.Columns[x] = analysis.Downsample(half.Columns[x], 2, 0);
            }
            return quarter;
        }

        private Image<T> Upsample (Image<T> quarter, int xs, int ys) {
            Image<T> half = new Image<T>(quarter.Xs, ys);
            for( int x = 0; x < half.Xs; x++ ) {
                half.Columns[x] = synthesis.Upsample(quarter.Columns[x], 2, 0, ys);
            }

            Image<T> full = new Image<T>(xs, ys);
            for( int y = 0; y < full.Ys; y++ ) {
                full.Rows[y] = synthesis.Upsample(half.Rows[y], 2, 0, xs);
            }
            return full;
        }

    }

}
