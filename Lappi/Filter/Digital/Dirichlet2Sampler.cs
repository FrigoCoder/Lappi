using System;

using Lappi.Image;
using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class Dirichlet2Sampler<T> : Sampler1D<T> where T : new() {

        public T Sample (T[] source, int center) {
            dynamic result = (dynamic) source[center] * 0.5;
            if( center == source.Length - 1 ) {
                result += (dynamic) source[center - 1] * 0.5;
                return result;
            }
            switch( Math.Min(center, source.Length - 1 - center) ) {
                case 0:
                    result += (dynamic) source[center + 1] * 0.5;
                    return result;
                case 1:
                case 2:
                    result += ((dynamic) source[center - 1] + source[center + 1]) * 0.25;
                    return result;
                default:
                    result += ((dynamic) source[center - 1] + source[center + 1]) * 0.25;
                    result += ((dynamic) source[center - 1] + source[center + 1] - source[center - 3] - source[center + 3]) * 0.0517766952966369;
                    return result;
            }
        }

        public T[] Convolute (T[] source) => Arrays.New(source.Length, i => Sample(source, i));

        public T[] Downsample (T[] source, int factor, int shift) =>
            Arrays.New((source.Length - shift + factor - 1) / factor, i => Sample(source, i * factor + shift));

        public T[] Upsample (T[] source, int factor, int shift) => Upsample(source, factor, shift, source.Length * factor);

        public T[] Upsample (T[] source, int factor, int shift, int length) {
            Preconditions.Require(factor == 2 && shift == 0);
            T[] v = Arrays.New(length, new T());
            for( int i = 0; i < source.Length; i++ ) {
                v[i * factor + shift] = (dynamic) source[i] * factor;
            }
            return Arrays.New(length, i => i % 2 == 0 ? (T) ((dynamic) v[i] * 0.5) : Sample(v, i));
        }

    }

    public class Dirichlet2SamplerYuvD : Sampler1D<YuvD> {

        public YuvD Sample (YuvD[] source, int center) {
            YuvD result = source[center] * 0.5;
            if( center == source.Length - 1 ) {
                result += source[center - 1] * 0.5;
                return result;
            }
            switch( Math.Min(center, source.Length - 1 - center) ) {
                case 0:
                    result += source[center + 1] * 0.5;
                    return result;
                case 1:
                case 2:
                    result += (source[center - 1] + source[center + 1]) * 0.25;
                    return result;
                default:
                    result += (source[center - 1] + source[center + 1]) * 0.25;
                    result += (source[center - 1] + source[center + 1] - source[center - 3] - source[center + 3]) * 0.0517766952966369;
                    return result;
            }
        }

        public YuvD[] Convolute (YuvD[] source) => Arrays.New(source.Length, i => Sample(source, i));

        public YuvD[] Downsample (YuvD[] source, int factor, int shift) =>
            Arrays.New((source.Length - shift + factor - 1) / factor, i => Sample(source, i * factor + shift));

        public YuvD[] Upsample (YuvD[] source, int factor, int shift) => Upsample(source, factor, shift, source.Length * factor);

        public YuvD[] Upsample (YuvD[] source, int factor, int shift, int length) {
            Preconditions.Require(factor == 2 && shift == 0);
            YuvD[] v = Arrays.New(length, new YuvD());
            for( int i = 0; i < source.Length; i++ ) {
                v[i * factor + shift] = source[i] * factor;
            }
            return Arrays.New(length, i => i % 2 == 0 ? v[i] * 0.5 : Sample(v, i));
        }

    }

}
