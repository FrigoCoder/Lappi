using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class DigitalSampler<T> where T : new() {

        private readonly BoundaryHandler filters;

        public DigitalSampler (DigitalFilter filter) : this(new SumBoundaryHandler(filter)) {
        }

        public DigitalSampler (BoundaryHandler filters) {
            this.filters = filters;
        }

        public virtual T Sample (T[] source, int center) {
            DigitalFilter filter = filters.GetFilter(center, source.Length);
            T sum = new T();
            for( int i = filter.Left; i <= filter.Right; i++ ) {
                sum += (dynamic) source[center + i] * filter[i];
            }
            return sum;
        }

        public T[] Convolute (T[] source) => Arrays.New(source.Length, i => Sample(source, i));

        public T[] Downsample (T[] source, int factor, int shift) =>
            Arrays.New((source.Length - shift + factor - 1) / factor, i => Sample(source, i * factor + shift));

        public T[] Upsample (T[] source, int factor, int shift) => Upsample(source, factor, shift, source.Length * factor);

        public T[] Upsample (T[] source, int factor, int shift, int length) {
            T[] v = Arrays.New(length, new T());
            for( int i = 0; i < source.Length; i++ ) {
                v[i * factor + shift] = (dynamic) source[i] * factor;
            }
            return Convolute(v);
        }

    }

}
