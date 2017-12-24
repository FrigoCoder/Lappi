using Lappi.Image;

namespace Lappi.Filter.Digital2D {

    public interface Sampler2D<T> where T : new() {

        T Sample (Image<T> source, int cx, int cy);

        Image<T> Convolute (Image<T> source);

        Image<T> Downsample (Image<T> source, int factor = 2, int shift = 0);

        Image<T> Upsample (Image<T> source, int factor, int shift);

        Image<T> Upsample (Image<T> source, int factor, int shift, int xs, int ys);

    }

}