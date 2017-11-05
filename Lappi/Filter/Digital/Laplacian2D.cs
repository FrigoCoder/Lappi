using System;

using Lappi.Image;

namespace Lappi.Filter.Digital {

    public class Laplacian2D<T> where T : new() {

        private readonly DigitalSampler<T> analysis;
        private readonly DigitalSampler<T> synthesis;

        public Laplacian2D (DigitalFilter analysis, DigitalFilter synthesis) {
            this.analysis = new DigitalSampler<T>(analysis);
            this.synthesis = new DigitalSampler<T>(synthesis);
        }

        public Tuple<Image<T>, Image<T>> Forward (Image<T> image) {
            Image<T> downsampled = DownSample(image);
            Image<T> upsampled = UpSample(downsampled, image.Xs, image.Ys);
            Image<T> difference = image - upsampled;
            return Tuple.Create(downsampled, difference);
        }

        public Image<T> Inverse (Tuple<Image<T>, Image<T>> tuple) => Inverse(tuple.Item1, tuple.Item2);

        public Image<T> Inverse (Image<T> downsampled, Image<T> difference) {
            Image<T> upsampled = UpSample(downsampled, difference.Xs, difference.Ys);
            return upsampled + difference;
        }

        private Image<T> DownSample (Image<T> image) {
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

        private Image<T> UpSample (Image<T> quarter, int xs, int ys) {
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
