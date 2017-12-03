using System;

namespace Lappi {

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
            if( v.Length == 0 || (v.Length & (v.Length - 1)) != 0 ) {
                throw new ArgumentException("Array length must be power of two");
            }
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
            uint p = NumberOfLeadingZeros((uint) (v.Length - 1));
            for( uint i = 0; i < v.Length; i++ ) {
                uint j = Reverse(i) >> (int) p;
                if( i < j ) {
                    Swap(v, i, j);
                }
            }
        }

        private uint NumberOfLeadingZeros (uint i) {
            if( i == 0 ) {
                return 32;
            }
            uint n = 0;
            if( i >> 16 == 0 ) {
                n += 16;
                i <<= 16;
            }
            if( i >> 24 == 0 ) {
                n += 8;
                i <<= 8;
            }
            if( i >> 28 == 0 ) {
                n += 4;
                i <<= 4;
            }
            if( i >> 30 == 0 ) {
                n += 2;
                i <<= 2;
            }
            if( i >> 31 == 0 ) {
                n++;
            }
            return n;
        }

        private uint Reverse (uint i) {
            i = ((i & 0x55555555) << 1) | ((i >> 1) & 0x55555555);
            i = ((i & 0x33333333) << 2) | ((i >> 2) & 0x33333333);
            i = ((i & 0x0f0f0f0f) << 4) | ((i >> 4) & 0x0f0f0f0f);
            i = ((i & 0x00ff00ff) << 8) | ((i >> 8) & 0x00ff00ff);
            i = ((i & 0x0000ffff) << 16) | ((i >> 16) & 0x0000ffff);
            return i;
        }

        private void Swap (Complex[] v, uint i, uint j) {
            Complex t = v[i];
            v[i] = v[j];
            v[j] = t;
        }

        private void Normalize (Complex[] F) {
            for( int f = 0; f < F.Length; f++ ) {
                F[f] /= F.Length;
            }
        }

    }

}
