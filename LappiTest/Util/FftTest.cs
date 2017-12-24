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
            Complex[] time = Arrays.New<Complex>(8, t => t == 1 ? 1 : 0);
            Complex[] freq = {
                new Complex(0.125, 0), new Complex(0.088, -0.088), new Complex(0, -0.125), new Complex(-0.088, -0.088), new Complex(-0.125, 0),
                new Complex(-0.088, 0.088), new Complex(0, 0.125), new Complex(0.088, 0.088)
            };
            Assert.That(fft.Forward(time), Is.EqualTo(freq).Using(Complex.Within(1E-6)));
        }

        [Test]
        public void Random_signal () {
            for( int n = 1; n <= maxn; n *= 2 ) {
                Complex[] time = Arrays.New(n, t => new Complex(random.NextDouble(), random.NextDouble()));
                Complex[] freq = fft.Forward(time);
                AssertTimeFreq(time, freq);
            }
        }

        [Test]
        public void Constant_signal () {
            for( int n = 1; n <= maxn; n *= 2 ) {
                Complex[] time = Arrays.New<Complex>(n, t => 1);
                Complex[] freq = Arrays.New<Complex>(n, f => f == 0 ? 1 : 0);
                AssertTimeFreq(time, freq);
            }
        }

        [Test]
        public void Cis_signal () {
            for( int n = 16; n <= maxn; n *= 2 ) {
                Complex[] time = Arrays.New(n, t => Complex.Cis(2 * Math.PI * 10 * t / n));
                Complex[] freq = Arrays.New<Complex>(n, f => f == 10 ? 1 : 0);
                AssertTimeFreq(time, freq);
            }
        }

        [Test]
        public void Cos_signal () {
            for( int n = 16; n <= maxn; n *= 2 ) {
                Complex[] time = Arrays.New<Complex>(n, t => Math.Cos(2 * Math.PI * 10 * t / n));
                Complex[] freq = Arrays.New<Complex>(n, f => f == 10 ? 0.5 : f == n - 10 ? 0.5 : 0);
                AssertTimeFreq(time, freq);
            }
        }

        [Test]
        public void Sin_signal () {
            for( int n = 16; n <= maxn; n *= 2 ) {
                Complex[] time = Arrays.New<Complex>(n, t => Math.Sin(2 * Math.PI * 10 * t / n));
                Complex[] freq = Arrays.New(n, f => f == 10 ? new Complex(0, -0.5) : f == n - 10 ? new Complex(0, 0.5) : 0);
                AssertTimeFreq(time, freq);
            }
        }

        private void AssertTimeFreq (Complex[] time, Complex[] freq) {
            Assert.That(fft.Forward(time), Is.EqualTo(freq).Using(Complex.Within(1E-16)));
            Assert.That(fft.Inverse(freq), Is.EqualTo(time).Using(Complex.Within(1E-16)));
        }

    }

}
