using System;

namespace Lappi.Util {

    public class Fft {

        public Complex[] Forward (Complex[] T) {
            Complex[] F = (Complex[]) T.Clone();
            Core(F, -1.0);
            Normalize(F);
            return F;
        }

        public Complex[] Inverse (Complex[] F) {
            Complex[] T = (Complex[]) F.Clone();
            Core(T, 1.0);
            return T;
        }

        private void Core (Complex[] v, double sign) {
            Preconditions.Require<ArgumentException>(v.Length != 0 && (v.Length & (v.Length - 1)) == 0);
            for( int blockSize = v.Length; blockSize > 1; blockSize /= 2 ) {
                Complex root = Complex.Cis(sign * 2.0 * Math.PI / blockSize);
                Complex twiddle = 1;
                for( int i = 0; i < blockSize / 2; i++ ) {
                    for( int x1 = i; x1 < v.Length; x1 += blockSize ) {
                        int x2 = x1 + blockSize / 2;
                        Complex e = v[x1];
                        Complex f = v[x2];
                        v[x1] = e + f;
                        v[x2] = (e - f) * twiddle;
                    }
                    twiddle *= root;
                }
            }
            BitReverse(v);
        }

        private void BitReverse (Complex[] v) {
            int mul = v.Length.Reverse() * 2;
            for( int i = 0; i < v.Length; i++ ) {
                int j = (i * mul).Reverse();
                if( i < j ) {
                    Lang.Swap(ref v[i], ref v[j]);
                }
            }
        }

        private void Normalize (Complex[] F) {
            for( int f = 0; f < F.Length; f++ ) {
                F[f] /= F.Length;
            }
        }

    }

}
