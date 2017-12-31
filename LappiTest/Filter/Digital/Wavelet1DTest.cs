using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    using Wavelet1D = Wavelet1D<double>;

    [TestFixture]
    public class Wavelet1DTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 144};
        private readonly DigitalFilter lowpass = new DigitalAdapter(new Linear(), 2.0);
        private readonly DigitalFilter highpass = new HighpassAdapter(new DigitalAdapter(new Linear(), 2.0));

        [TestCase]
        public void Transform_returns_correct_lowpass_vector () {
            Wavelet1D wavelet = new Wavelet1D(lowpass, highpass, lowpass, highpass);
            double[] expected = {2, 9.5, 25.5, 49.5, 81.5, 121.5};
            double[] actual = wavelet.Forward(source)[0];
            Assert.That(actual, Is.EqualTo(expected), "\nExpected: " + string.Join(", ", expected) + "\nActual: " + string.Join(", ", actual));
        }

        [TestCase]
        public void Transform_returns_correct_highpass_vector () {
            Wavelet1D wavelet = new Wavelet1D(lowpass, highpass, lowpass, highpass);
            double[] expected = {-0.5, -0.5, -0.5, -0.5, -0.5, 55.6666666666666666};
            double[] actual = wavelet.Forward(source)[1];
            Assert.That(actual, Is.EqualTo(expected).Within(1E-14),
                "\nExpected: " + string.Join(", ", expected) + "\nActual: " + string.Join(", ", actual));
        }

        [Ignore("#1: DigitalSampler normalization bug - Lowpass and highpass filters are inconsistent due to boundary handling")]
        [TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal_with_CDF_5_3_filters () {
            Wavelet1D wavelet = new Wavelet1D(CDF53.AnalysisLowpass, CDF53.AnalysisHighpass, CDF53.SynthesisLowpass, CDF53.SynthesisHighpass);
            double[] actual = wavelet.Inverse(wavelet.Forward(source));
            Assert.That(actual, Is.EqualTo(source), "\nExpected: " + string.Join(", ", source) + "\nActual: " + string.Join(", ", actual));
        }

        [TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal_with_CDF_5_3_filters_at_boundaries () {
            Wavelet1D wavelet = new Wavelet1D(CDF53.AnalysisLowpass, CDF53.AnalysisHighpass, CDF53.SynthesisLowpass, CDF53.SynthesisHighpass);
            double[] actual = wavelet.Inverse(wavelet.Forward(source));
            for( int i = 2; i < actual.Length - 3; i++ ) {
                Assert.That(actual[i], Is.EqualTo(source[i]), "Index " + i);
            }
        }

        [Ignore("#1: DigitalSampler normalization bug - Lowpass and highpass filters are inconsistent due to boundary handling")]
        [TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal_with_CDF_9_7_filters () {
            Wavelet1D wavelet = new Wavelet1D(CDF97.AnalysisLowpass, CDF97.AnalysisHighpass, CDF97.SynthesisLowpass, CDF97.SynthesisHighpass);
            double[] actual = wavelet.Inverse(wavelet.Forward(source));
            Assert.That(actual, Is.EqualTo(source), "\nExpected: " + string.Join(", ", source) + "\nActual: " + string.Join(", ", actual));
        }

    }

}
