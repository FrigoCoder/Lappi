namespace Lappi.Filter.Digital {

    public interface Sampler1D<T> where T : new() {

        T Sample (T[] source, int center);

        T[] Convolute (T[] source);

        T[] Downsample (T[] source, int factor, int shift);

        T[] Upsample (T[] source, int factor, int shift);

        T[] Upsample (T[] source, int factor, int shift, int length);

    }

}