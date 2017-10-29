using System;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class Wavelet1DTest {

        [TestCase]
        public void Transform_returns_correct_lowpass_vector () {
            double[] source = {1, 4, 9, 16, 25, 36};
            DigitalFilter lowpass = new DigitalAdapter(new Linear(), 2.0);
            DigitalFilter highpass = new DigitalAdapter(new Linear(), 2.0);
            Wavelet1D wavelet = new Wavelet1D(lowpass, highpass);

            double[] expected = {2, 9.5, 25.5};
            Tuple<double[], double[]> result = wavelet.Forward(source);
            Assert.That(result.Item1, Is.EqualTo(expected));
        }

        [TestCase]
        public void Transform_returns_correct_highpass_vector () {
            double[] source = {1, 4, 9, 16, 25, 36};
            DigitalFilter lowpass = new DigitalAdapter(new Linear(), 2.0);
            DigitalFilter highpass = new DigitalAdapter(new Linear(), 2.0);
            Wavelet1D wavelet = new Wavelet1D(lowpass, highpass);

            double[] expected = {-0.5, -0.5, 3.6666666666666666};
            Tuple<double[], double[]> result = wavelet.Forward(source);
            Assert.That(result.Item2, Is.EqualTo(expected).Within(1E-14));
        }

        [Ignore("Fuck wavelets, perfect reconstruction, and boundary conditions"), TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal_with_CDF_5_3_filters () {
            double[] source = {1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 144};
            DigitalFilter lowpass = new CoefficientAdapter(2, new[] {-0.125, 0.25, 0.75, 0.25, -0.125});
            DigitalFilter highpass = new CoefficientAdapter(1, new[] {0.25, 0.5, 0.25});
            Wavelet1D wavelet = new Wavelet1D(lowpass, highpass);
            double[] actual = wavelet.Inverse(wavelet.Forward(source));
            Assert.That(actual, Is.EqualTo(source));
        }

        [Ignore("Fuck wavelets, perfect reconstruction, and boundary conditions"), TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal_with_CDF_9_7_filters () {
            double[] source = {1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 144};
            DigitalFilter lowpass = new CoefficientAdapter(4,
                new[] {
                    0.02674874108097600, -0.01686411844287495, -0.07822326652898785, 0.26686411844287230, 0.60294901823635790, 0.26686411844287230,
                    -0.07822326652898785, -0.01686411844287495, 0.02674874108097600
                });
            DigitalFilter highpass = new CoefficientAdapter(3,
                new[] {
                    -0.045635881557124740, -0.028771763114249785, 0.295635881557123500, 0.557543526228497000, 0.295635881557123500,
                    -0.028771763114249785, -0.045635881557124740
                });
            Wavelet1D wavelet = new Wavelet1D(lowpass, highpass);
            double[] actual = wavelet.Inverse(wavelet.Forward(source));
            Assert.That(actual, Is.EqualTo(source));
        }

    }

}
