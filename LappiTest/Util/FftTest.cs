using System;

using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    public class FftTest {

        private readonly int maxn = 1024;
        private readonly Random random = new Random();
        private readonly Fft fft = new Fft();

        /// <summary>
        ///     Second example from http://www.sccon.ca/sccon/fft/fft3.htm
        /// </summary>
        [Test]
        public void Simple_example () {
            Complex[] T = new Complex[8];
            T[1] = 1;
            Complex[] expected = {
                new Complex(0.125, 0), new Complex(0.088, -0.088), new Complex(0, -0.125), new Complex(-0.088, -0.088), new Complex(-0.125, 0),
                new Complex(-0.088, 0.088), new Complex(0, 0.125), new Complex(0.088, 0.088)
            };
            Complex[] F = fft.Forward(T);
            Assert.That(F, Is.EqualTo(expected).Using(Complex.Within(1E-6)));
        }

        [Test]
        public void Full_transform_correctly_restores_random_array () {
            for( int n = 1; n <= maxn; n *= 2 ) {
                Complex[] T = new Complex[n];
                for( int t = 0; t < T.Length; t++ ) {
                    T[t] = new Complex(random.NextDouble(), random.NextDouble());
                }
                Assert.That(fft.Inverse(fft.Forward(T)), Is.EqualTo(T).Using(Complex.Within(1E-16)));
            }
        }

        [Test]
        public void Forward_transform_correctly_transforms_constant_signal () {
            for( int n = 1; n <= maxn; n *= 2 ) {
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
        }

        [Test]
        public void Inverse_transform_correctly_restores_constant_signal () {
            for( int n = 1; n <= maxn; n *= 2 ) {
                Complex[] F = new Complex[n];
                F[0] = 1;
                Complex[] T = fft.Inverse(F);
                for( int t = 0; t < n; t++ ) {
                    Complex expected = 1;
                    Assert.That(T[t], Is.EqualTo(expected));
                }
            }
        }

        [Test]
        public void Forward_transform_correctly_transforms_cis_signal () {
            for( int n = 16; n <= maxn; n *= 2 ) {
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
        }

        [Test]
        public void Inverse_transform_correctly_restores_cis_signal () {
            for( int n = 16; n <= maxn; n *= 2 ) {
                Complex[] F = new Complex[n];
                F[10] = 1;
                Complex[] T = fft.Inverse(F);
                for( int t = 0; t < n; t++ ) {
                    Complex expected = Complex.Cis(2 * Math.PI * 10 * t / n);
                    Assert.That(T[t], Is.EqualTo(expected).Using(Complex.Within(1E-16)));
                }
            }
        }

        [Test]
        public void Forward_transform_correctly_transforms_cos_signal () {
            for( int n = 16; n <= maxn; n *= 2 ) {
                Complex[] T = new Complex[n];
                for( int t = 0; t < n; t++ ) {
                    T[t] = Math.Cos(2 * Math.PI * 10 * t / n);
                }
                Complex[] F = fft.Forward(T);
                for( int f = 0; f < n; f++ ) {
                    Complex expected = f == 10 ? 0.5 : f == n - 10 ? 0.5 : 0;
                    Assert.That(F[f], Is.EqualTo(expected).Using(Complex.Within(1E-16)));
                }
            }
        }

        [Test]
        public void Inverse_transform_correctly_restores_cos_signal () {
            for( int n = 16; n <= maxn; n *= 2 ) {
                Complex[] F = new Complex[n];
                F[10] = 0.5;
                F[n - 10] = 0.5;
                Complex[] T = fft.Inverse(F);
                for( int t = 0; t < n; t++ ) {
                    Complex expected = Math.Cos(2 * Math.PI * 10 * t / n);
                    Assert.That(T[t], Is.EqualTo(expected).Using(Complex.Within(1E-16)));
                }
            }
        }

        [Test]
        public void Inverse_transform_correctly_restores_sin_signal () {
            for( int n = 16; n <= maxn; n *= 2 ) {
                Complex[] F = new Complex[n];
                F[10] = new Complex(0, -0.5);
                F[n - 10] = new Complex(0, 0.5);
                Complex[] T = fft.Inverse(F);
                for( int t = 0; t < n; t++ ) {
                    Complex expected = Math.Sin(2 * Math.PI * 10 * t / n);
                    Assert.That(T[t], Is.EqualTo(expected).Using(Complex.Within(1E-16)));
                }
            }
        }

        [Test]
        public void Forward_transform_correctly_transforms_sin_signal () {
            for( int n = 16; n <= maxn; n *= 2 ) {
                Complex[] T = new Complex[n];
                for( int t = 0; t < n; t++ ) {
                    T[t] = Math.Sin(2 * Math.PI * 10 * t / n);
                }
                Complex[] F = fft.Forward(T);
                for( int f = 0; f < n; f++ ) {
                    Complex expected = f == 10 ? new Complex(0, -0.5) : f == n - 10 ? new Complex(0, 0.5) : 0;
                    Assert.That(F[f], Is.EqualTo(expected).Using(Complex.Within(1E-16)));
                }
            }
        }

    }

}
