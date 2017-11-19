using System;

using Lappi.Image;

namespace Lappi.Filter.Digital {

    public class DigitalSampler2D<T> where T : new() {

        private readonly Filter2D filter;
        private readonly double[,] coefficients;

        public DigitalSampler2D (Filter2D filter) {
            this.filter = filter;
            coefficients = filter.Coefficients;
        }

        public virtual T Sample (Image<T> image, int cx, int cy) {
            T result = new T();
            double sum = 0;
            for( int x = Math.Max(0, cx + filter.Left); x <= Math.Min(cx + filter.Right, image.Xs - 1); x++ ) {
                for( int y = Math.Max(0, cy + filter.Top); y <= Math.Min(cy + filter.Bottom, image.Ys - 1); y++ ) {
                    double w = coefficients[x - cx - filter.Left, y - cy - filter.Top];
                    result += (dynamic) image[x, y] * w;
                    sum += w;
                }
            }
            return (dynamic) result / sum;
        }

        public Image<T> Convolute (Image<T> image) {
            Image<T> result = new Image<T>(image.Xs, image.Ys);
            for( int x = 0; x < result.Xs; x++ ) {
                for( int y = 0; y < result.Ys; y++ ) {
                    result[x, y] = Sample(image, x, y);
                }
            }
            return result;
        }

    }

}
