using System;

using Lappi.Image;

namespace Lappi.Filter.Digital2D {

    public class NonSeparableSampler<T> : Sampler2D<T> where T : new() {

        private readonly Filter2D filter;

        public NonSeparableSampler (Filter2D filter) {
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

        public Image<T> Downsample (Image<T> source) =>
            new Image<T>((source.Xs + 1) / 2, (source.Ys + 1) / 2, (x, y) => Sample(source, x * 2, y * 2));

        public Image<T> Upsample (Image<T> source, int xs, int ys) {
            Image<T> result = new Image<T>(xs, ys);
            source.ForEach((x, y) => result[x * 2, y * 2] = (dynamic) source[x, y] * 4);
            return Convolute(result);
        }

    }

}
