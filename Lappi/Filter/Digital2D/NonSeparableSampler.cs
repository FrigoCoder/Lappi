using System;

using Lappi.Image;

namespace Lappi.Filter.Digital2D {

    public class NonSeparableSampler<T> : Sampler2D<T> where T : new() {

        private readonly DigitalFilter2D filter;

        public NonSeparableSampler (DigitalFilter2D filter) {
            this.filter = filter;
        }

        public T Sample (Image<T> source, int cx, int cy) {
            T result = new T();
            double sum = 0;
            for( int x = Math.Max(0, cx + filter.Left); x <= Math.Min(cx + filter.Right, source.Xs - 1); x++ ) {
                for( int y = Math.Max(0, cy + filter.Top); y <= Math.Min(cy + filter.Bottom, source.Ys - 1); y++ ) {
                    double w = filter[x - cx, y - cy];
                    result += (dynamic) source[x, y] * w;
                    sum += w;
                }
            }
            return (dynamic) result / sum;
        }

        public Image<T> Convolute (Image<T> source) => new Image<T>(source.Xs, source.Ys, (x, y) => Sample(source, x, y));

        public Image<T> Downsample (Image<T> source, int factor = 2, int shift = 0) => new Image<T>((source.Xs - shift + factor - 1) / factor,
            (source.Ys - shift + factor - 1) / factor, (x, y) => Sample(source, x * factor + shift, y * factor + shift));

        public Image<T> Upsample (Image<T> source, int factor, int shift) => Upsample(source, factor, shift, source.Xs * factor, source.Ys * factor);

        public Image<T> Upsample (Image<T> source, int factor, int shift, int xs, int ys) {
            Image<T> result = new Image<T>(xs, ys);
            for( int x = 0; x < source.Xs; x++ ) {
                for( int y = 0; y < source.Ys; y++ ) {
                    result[x * factor + shift, y * factor + shift] = (dynamic) source[x, y] * factor * factor;
                }
            }
            return Convolute(result);
        }

    }

}
