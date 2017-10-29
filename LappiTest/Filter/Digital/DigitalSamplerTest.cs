using System;
using System.Linq;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class DigitalSamplerTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36};
        private readonly DigitalSampler linear1 = new DigitalSampler(new DigitalAdapter(new Linear(), 1.0));
        private readonly DigitalSampler linear2 = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));

        [TestCase]
        public void Sample_with_scale_1_linear_filter_returns_array_value () {
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(linear1.Sample(source, i), Is.EqualTo(source[i]));
            }
        }

        [TestCase]
        public void Sample_with_scale_2_linear_filter_blurs_linearly () {
            double[] expected = {2, 4.5, 9.5, 16.5, 25.5, 32.333333333333333333333333333333};
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(linear2.Sample(source, i), Is.EqualTo(expected[i]));
            }
        }

        [TestCase]
        public void Sample_beyond_left_boundary_returns_0 () {
            Assert.That(linear2.Sample(source, -2), Is.EqualTo(0));
        }

        [TestCase]
        public void Sample_beyond_right_boundary_returns_0 () {
            Assert.That(linear2.Sample(source, source.Length + 1), Is.EqualTo(0));
        }

        [TestCase]
        public void SampleHighpass_returns_difference_between_original_and_blurred () {
            double[] expected = {-1, -0.5, -0.5, -0.5, -0.5, 3.6666666666666666};
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(linear2.SampleHighpass(source, i), Is.EqualTo(expected[i]).Within(1E-14));
            }
        }

        [TestCase]
        public void Convolute_with_scale_1_linear_filter_returns_array () {
            Assert.That(linear1.Convolute(source), Is.EqualTo(source));
        }

        [TestCase]
        public void Convolute_with_scale_2_linear_filter_blurs_linearly () {
            double[] expected = {2, 4.5, 9.5, 16.5, 25.5, 32.333333333333333333333333333333};
            Assert.That(linear2.Convolute(source), Is.EqualTo(expected));
        }

        [TestCase]
        public void Convolute_with_lowpass_filter_preserves_constant_array () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Dirichlet(4.0), 2.0));
            double[] constant = {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2};
            Assert.That(sampler.Convolute(constant), Is.EqualTo(constant));
        }

        [TestCase]
        public void Convolute_with_highpass_filter_preserves_nyquist_array () {
            DigitalSampler sampler = new DigitalSampler(new HighpassAdapter(new DigitalAdapter(new Dirichlet(4.0), 2.0)));
            double[] nyquist = {2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2};
            Assert.That(sampler.Convolute(nyquist), Is.EqualTo(nyquist).Within(1E-15));
        }

        [Ignore("#1: DigitalSampler normalization bug - Lowpass and highpass filters are inconsistent due to boundary handling"), TestCase]
        public void Convolute_with_lowpass_and_highpass_is_complementary () {
            DigitalSampler highpass = new DigitalSampler(new HighpassAdapter(new DigitalAdapter(new Linear(), 2.0)));
            double[] sum = linear2.Convolute(source).Zip(highpass.Convolute(source), (x, y) => x + y).ToArray();
            Assert.That(sum, Is.EqualTo(source));
        }

        [TestCase]
        public void Convolute_with_lowpass_and_highpass_is_complementary_at_nonboundaries () {
            DigitalSampler highpass = new DigitalSampler(new HighpassAdapter(new DigitalAdapter(new Linear(), 2.0)));
            double[] sum = linear2.Convolute(source).Zip(highpass.Convolute(source), (x, y) => x + y).ToArray();
            for( int i = 1; i < sum.Length - 1; i++ ) {
                Assert.That(sum[i], Is.EqualTo(source[i]));
            }
        }

        [TestCase]
        public void Convolute_can_handle_float_arrays () {
            DigitalSampler<float> sampler = new DigitalSampler<float>(new DigitalAdapter(new Linear(), 2.0));
            float[] array = {1, 4, 9, 16, 25, 36};
            float[] expected = {2, 4.5f, 9.5f, 16.5f, 25.5f, 32.333333333333333333333333333333f};
            Assert.That(sampler.Convolute(array), Is.EqualTo(expected));
        }

        [TestCase]
        public void ConvoluteHighpass_returns_difference_between_original_and_blurred () {
            double[] expected = {-1, -0.5, -0.5, -0.5, -0.5, 3.6666666666666666};
            Assert.That(linear2.ConvoluteHighpass(source), Is.EqualTo(expected).Within(1E-14));
        }

        [TestCase]
        public void Convolute_and_ConvoluteHighPass_are_complementary () {
            double[] sum = linear2.Convolute(source).Zip(linear2.ConvoluteHighpass(source), (x, y) => x + y).ToArray();
            Assert.That(sum, Is.EqualTo(source));
        }

        [TestCase]
        public void ConvoluteHighpass_can_handle_float_arrays_even_if_imprecise_as_fuck () {
            DigitalSampler<float> sampler = new DigitalSampler<float>(new DigitalAdapter(new Linear(), 2.0));
            float[] array = {1, 4, 9, 16, 25, 36};
            float[] expected = {-1f, -0.5f, -0.5f, -0.5f, -0.5f, 3.6666666666666666f};
            Assert.That(sampler.ConvoluteHighpass(array), Is.EqualTo(expected).Within(1E-5));
        }

        [TestCase]
        public void Downsample_with_factor_2_and_shift_0 () {
            double[] expected = {2, 9.5, 25.5};
            Assert.That(linear2.Downsample(source, 2, 0), Is.EqualTo(expected));
        }

        [TestCase]
        public void Downsample_with_factor_2_and_shift_1 () {
            double[] expected = {4.5, 16.5, 32.333333333333333333333333333333};
            Assert.That(linear2.Downsample(source, 2, 1), Is.EqualTo(expected));
        }

        [TestCase]
        public void DownsampleHighpass_with_factor_2_and_shift_0 () {
            double[] expected = {-1, -0.5, -0.5};
            Assert.That(linear2.DownsampleHighpass(source, 2, 0), Is.EqualTo(expected));
        }

        [TestCase]
        public void DownsampleHighpass_with_factor_2_and_shift_1 () {
            double[] expected = {-0.5, -0.5, 3.6666666666666666};
            Assert.That(linear2.DownsampleHighpass(source, 2, 1), Is.EqualTo(expected).Within(1E-14));
        }

        [TestCase]
        public void Upsample_with_factor_2_and_shift_0 () {
            double[] downsampled = {2, 9.5, 25.5};
            double[] expected = {1.3333333333333333, 2.875, 4.75, 8.75, 12.75, 8.5};
            Assert.That(linear2.Upsample(downsampled, 2, 0), Is.EqualTo(expected));
        }

        [TestCase]
        public void Upsample_with_factor_2_and_shift_1 () {
            double[] downsampled = {4.5, 16.5, 32.333333333333333333333333333333};
            double[] expected = {1.5, 2.25, 5.25, 8.25, 12.2083333333333333, 21.5555555555555555};
            Assert.That(linear2.Upsample(downsampled, 2, 1), Is.EqualTo(expected));
        }

        [TestCase]
        public void UpsampleHighpass_with_factor_2_and_shift_0 () {
            double[] downsampled = {2, 9.5, 25.5};
            double[] expected = {0.66666666666666666, -2.875, 4.75, -8.75, 12.75, -8.5};
            double[] actual = linear2.UpsampleHighpass(downsampled, 2, 0);
            Console.WriteLine(string.Join(", ", actual));
            Assert.That(actual, Is.EqualTo(expected).Within(1E-14));
        }

        [TestCase]
        public void UpsampleHighpass_with_factor_2_and_shift_1 () {
            double[] downsampled = {4.5, 16.5, 32.333333333333333333333333333333};
            double[] expected = {-1.5, 2.25, -5.25, 8.25, -12.20833333333333333333333333, 10.777777777777777777};
            Assert.That(linear2.UpsampleHighpass(downsampled, 2, 1), Is.EqualTo(expected));
        }

    }

}
