using Lappi.Image;

namespace Lappi.Filter.Digital2D {

    public interface Sampler2D<T> where T : new() {

        T Sample (Image<T> source, int cx, int cy);

        Image<T> Convolute (Image<T> source);

        Image<T> Downsample (Image<T> source);

        Image<T> Upsample (Image<T> source, int xs, int ys);

    }

}
