using System.Linq;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    using DigitalSampler = DigitalSampler<double>;
    using HighpassSampler = HighpassSampler<double>;

    [TestFixture]
    public class HighpassSamplerTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36};
        private readonly double[] blurred = {2, 4.5, 9.5, 16.5, 25.5, 32.333333333333333333333333333333};
        private readonly DigitalSampler linear2 = new HighpassSampler(new DigitalAdapter(new Linear(), 2.0));
        private readonly DigitalSampler linear2Lowpass = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
        private readonly DigitalSampler dirichlet4 = new HighpassSampler(new DigitalAdapter(new Dirichlet(4.0), 2.0));

        [TestCase]
        public void Sample_returns_difference_between_original_and_blurred () {
            double[] expected = source.Zip(blurred, (x, y) => x - y).ToArray();
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(linear2.Sample(source, i), Is.EqualTo(expected[i]).Within(1E-14));
            }
        }

        [TestCase]
        public void Convolute_returns_difference_between_original_and_blurred () {
            double[] expected = source.Zip(blurred, (x, y) => x - y).ToArray();
            Assert.That(linear2.Convolute(source), Is.EqualTo(expected).Within(1E-14));
        }

        [TestCase]
        public void Lowpass_Convolute_and_highpass_Convolute_are_complementary () {
            double[] low = linear2Lowpass.Convolute(source);
            double[] high = linear2.Convolute(source);
            double[] sum = low.Zip(high, (x, y) => x + y).ToArray();
            Assert.That(sum, Is.EqualTo(source));
        }

        [TestCase]
        public void Convolute_can_handle_float_arrays_even_if_imprecise_as_fuck () {
            DigitalSampler<float> sampler = new HighpassSampler<float>(new DigitalAdapter(new Linear(), 2.0));
            float[] array = {1, 4, 9, 16, 25, 36};
            float[] expected = {-1f, -0.5f, -0.5f, -0.5f, -0.5f, 3.6666666666666666f};
            Assert.That(sampler.Convolute(array), Is.EqualTo(expected).Within(1E-5));
        }

        [TestCase]
        public void Downsample_with_factor_2_and_shift_0 () {
            double[] expected = {-1, -0.5, -0.5};
            Assert.That(linear2.Downsample(source, 2, 0), Is.EqualTo(expected));
        }

        [TestCase]
        public void Downsample_with_factor_2_and_shift_1 () {
            double[] expected = {-0.5, -0.5, 3.6666666666666666};
            Assert.That(linear2.Downsample(source, 2, 1), Is.EqualTo(expected).Within(1E-14));
        }

        [TestCase]
        public void Upsample_with_factor_2_and_shift_0 () {
            double[] downsampled = {2, 9.5, 25.5};
            double[] expected = {0.66666666666666666, -2.875, 4.75, -8.75, 12.75, -8.5};
            double[] actual = linear2.Upsample(downsampled, 2, 0);
            Assert.That(actual, Is.EqualTo(expected).Within(1E-14));
        }

        [TestCase]
        public void Upsample_with_factor_2_and_shift_1 () {
            double[] downsampled = {4.5, 16.5, 32.333333333333333333333333333333};
            double[] expected = {-1.5, 2.25, -5.25, 8.25, -12.20833333333333333333333333, 10.777777777777777777};
            Assert.That(linear2.Upsample(downsampled, 2, 1), Is.EqualTo(expected));
        }

    }

}
