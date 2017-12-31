using System;

namespace Lappi.Util {

    /**
 * Discrete Fourier Transform implemented according to <a
 * href=http://en.wikipedia.org/wiki/Discrete_Fourier_transform#Definition>Wikipedia</a>. The forward transform
 * normalizes the result by 1/n, so the transform of an array of [1.0, 1.0, ...] will result in an array of [1.0, 0.0,
 * ...]. The inverse transform does not normalize anything.
 */

    public class Dft {

        public Complex[] Forward (Complex[] time) {
            Complex[] freq = Core(time, -1.0);
            Normalize(freq);
            return freq;
        }

        public Complex[] Inverse (Complex[] freq) => Core(freq, 1.0);

        private Complex[] Core (Complex[] time, double sign) {
            int n = time.Length;
            Complex[] freq = new Complex[n];
            for( int f = 0; f < n; f++ ) {
                Complex root = Complex.Cis(sign * 2.0 * Math.PI * f / n);
                Complex twiddle = 1;
                freq[f] = 0;
                for( int t = 0; t < n; t++ ) {
                    freq[f] += time[t] * twiddle;
                    twiddle *= root;
                }
            }
            return freq;
        }

        private void Normalize (Complex[] freq) {
            for( int f = 0; f < freq.Length; f++ ) {
                freq[f] /= freq.Length;
            }
        }

    }

}
