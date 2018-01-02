using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class DigitalSampler<T> : Sampler1D<T> where T : new() {

        private readonly BoundaryHandler filters;

        public DigitalSampler (Filter1D filter) : this(new SumBoundaryHandler(filter)) {
        }

        public DigitalSampler (BoundaryHandler filters) {
            this.filters = filters;
        }

        public virtual T Sample (T[] source, int center) {
            Filter1D filter = filters.GetFilter(center, source.Length);
            T sum = new T();
            for( int i = filter.Left; i <= filter.Right; i++ ) {
                sum += (dynamic) source[center + i] * filter[i];
            }
            return sum;
        }

        public T[] Convolute (T[] source) => Arrays.New(source.Length, i => Sample(source, i));

        public T[] Downsample (T[] source) => Arrays.New((source.Length + 1) / 2, i => Sample(source, i * 2));

        public T[] Upsample (T[] source, int length) {
            T[] v = Arrays.New(length, new T());
            for( int i = 0; i < source.Length; i++ ) {
                v[i * 2] = (dynamic) source[i] * 2;
            }
            return Convolute(v);
        }

    }

}
