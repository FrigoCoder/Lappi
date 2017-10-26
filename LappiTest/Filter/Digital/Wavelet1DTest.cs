using System;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class Wavelet1DTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36};
        private readonly DigitalFilter filterEven = new DigitalAdapter(new Linear(), 2.0);
        private readonly DigitalFilter filterOdd = new DigitalAdapter(new Linear(), 2.0);
        private Wavelet1D wavelet;

        [SetUp]
        public void SetUp () {
            wavelet = new Wavelet1D(filterEven, filterOdd);
        }

        [TestCase]
        public void Transform_returns_correct_lowpass_vector () {
            double[] expected = {2, 9.5, 25.5};
            Tuple<double[], double[]> result = wavelet.Transform(source);
            Assert.That(result.Item1, Is.EqualTo(expected));
        }

        [TestCase]
        public void Transform_returns_correct_highpass_vector () {
            double[] expected = {-0.5, -0.5, 3.6666666666666666};
            Tuple<double[], double[]> result = wavelet.Transform(source);
            Assert.That(result.Item2, Is.EqualTo(expected).Within(1E-14));
        }

    }

}
