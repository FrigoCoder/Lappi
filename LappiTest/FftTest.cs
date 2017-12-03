using System;

using Lappi;

using NUnit.Framework;

namespace LappiTest {

    public class FftTest {

        private readonly int n = 1024;
        private readonly Random random = new Random();
        private readonly Fft fft = new Fft();

        [Test]
        public void Forward_transform_correctly_transforms_constant_signal () {
            Complex[] T = new Complex[n];
            for( int t = 0; t < n; t++ ) {
                T[t] = 1;
            }
            Complex[] F = fft.Forward(T);
            for( int f = 0; f < n; f++ ) {
                Complex expected = f == 0 ? 1 : 0;
                Assert.That(F[f], Is.EqualTo(expected));
            }
        }

        [Test]
        public void Forward_transform_correctly_transforms_cis_signal () {
            Complex[] T = new Complex[n];
            for( int t = 0; t < n; t++ ) {
                T[t] = Complex.Cis(2 * Math.PI * 10 * t / n);
            }
            Complex[] F = fft.Forward(T);
            for( int f = 0; f < n; f++ ) {
                Complex expected = f == 10 ? 1 : 0;
                Assert.That(F[f], Is.EqualTo(expected).Using(Complex.Within(1E-16)));
            }
        }

        [Test]
        public void Inverse_transform_correctly_restores_constant_signal () {
            Complex[] F = new Complex[n];
            F[0] = 1;
            Complex[] T = fft.Inverse(F);
            for( int t = 0; t < n; t++ ) {
                Complex expected = 1;
                Assert.That(T[t], Is.EqualTo(expected));
            }
        }

        [Test]
        public void Inverse_transform_correctly_restores_cis_signal () {
            Complex[] F = new Complex[n];
            F[10] = 1;
            Complex[] T = fft.Inverse(F);
            for( int t = 0; t < n; t++ ) {
                Complex expected = Complex.Cis(2 * Math.PI * 10 * t / n);
                Assert.That(T[t], Is.EqualTo(expected).Using(Complex.Within(1E-16)));
            }
        }

        [Test]
        public void Forward_transform_followed_by_inverse_transform_correctly_restores_random_array () {
            Complex[] T = new Complex[n];
            for( int t = 0; t < T.Length; t++ ) {
                T[t] = new Complex(random.NextDouble(), random.NextDouble());
            }
            Assert.That(fft.Inverse(fft.Forward(T)), Is.EqualTo(T).Using(Complex.Within(1E-16)));
        }

    }

}
