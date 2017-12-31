using System;

using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class Dirichlet3Sampler<T> : Sampler1D<T> where T : new() {

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
                case 3:
                case 4:
                    result += ((dynamic) source[center - 1] + source[center + 1]) * 0.25;
                    result += ((dynamic) source[center - 1] + source[center + 1] - source[center - 3] - source[center + 3]) * 0.0517766952966369;
                    return result;
                default:
                    result += ((dynamic) source[center - 1] + source[center + 1]) * 0.311004233964073;
                    result += ((dynamic) source[center - 3] + source[center + 3]) * -0.0833333333333333;
                    result += ((dynamic) source[center - 5] + source[center + 5]) * 0.0223290993692602;
                    return result;
            }
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
