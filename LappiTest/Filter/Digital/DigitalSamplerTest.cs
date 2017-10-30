using System;
using System.Linq;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    using DigitalSampler = DigitalSampler<double>;

    [TestFixture]
    public class DigitalSamplerTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36};
        private readonly double[] blurred = {2, 4.5, 9.5, 16.5, 25.5, 32.333333333333333333333333333333};
        private readonly double[] zeroes = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        private readonly double[] constant = {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2};
        private readonly double[] shortConstant = {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2};

        private readonly double[] nyquist =
            {2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2, 2, -2};

        private readonly DigitalSampler linear1 = new DigitalSampler(new DigitalAdapter(new Linear(), 1.0));
        private readonly DigitalSampler linear2 = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
        private readonly DigitalSampler linear2Highpass = new DigitalSampler(new HighpassAdapter(new DigitalAdapter(new Linear(), 2.0)));
        private readonly DigitalSampler dirichlet4 = new DigitalSampler(new DigitalAdapter(new Dirichlet(4.0), 2.0));
        private readonly DigitalSampler dirichlet4Highpass = new DigitalSampler(new HighpassAdapter(new DigitalAdapter(new Dirichlet(4.0), 2.0)));

        [TestCase]
        public void Sample_with_scale_1_linear_filter_returns_array_value () {
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(linear1.Sample(source, i), Is.EqualTo(source[i]));
            }
        }

        [TestCase]
        public void Sample_with_scale_2_linear_filter_blurs_linearly () {
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(linear2.Sample(source, i), Is.EqualTo(blurred[i]));
            }
        }

        [TestCase]
        public void Sample_beyond_left_boundary_throws_exception () {
            Assert.That(() => linear2.Sample(source, -2), Throws.TypeOf<IndexOutOfRangeException>());
            Assert.That(() => linear2.Sample(source, -1), Throws.TypeOf<IndexOutOfRangeException>());
        }

        [TestCase]
        public void Sample_beyond_right_boundary_throws_exception () {
            Assert.That(() => linear2.Sample(source, source.Length), Throws.TypeOf<IndexOutOfRangeException>());
            Assert.That(() => linear2.Sample(source, source.Length + 1), Throws.TypeOf<IndexOutOfRangeException>());
        }

        [TestCase]
        public void Convolute_with_scale_1_linear_filter_returns_array () {
            Assert.That(linear1.Convolute(source), Is.EqualTo(source));
        }

        [TestCase]
        public void Convolute_with_scale_2_linear_filter_blurs_linearly () {
            Assert.That(linear2.Convolute(source), Is.EqualTo(blurred));
        }

        [TestCase]
        public void Convolute_with_lowpass_filter_preserves_constant_array () {
            Assert.That(dirichlet4.Convolute(constant), Is.EqualTo(constant));
        }

        [Ignore("#1: DigitalSampler normalization bug - Lowpass and highpass filters are inconsistent due to boundary handling")]
        [TestCase]
        public void Convolute_with_lowpass_filter_nulls_nyquist_array () {
            Assert.That(dirichlet4.Convolute(nyquist), Is.EqualTo(zeroes));
        }

        [Ignore("#1: DigitalSampler normalization bug - Lowpass and highpass filters are inconsistent due to boundary handling")]
        [TestCase]
        public void Convolute_with_highpass_filter_nulls_constant_array () {
            Assert.That(dirichlet4Highpass.Convolute(constant), Is.EqualTo(zeroes));
        }

        [TestCase]
        public void Convolute_with_highpass_filter_preserves_nyquist_array () {
            Assert.That(dirichlet4Highpass.Convolute(nyquist), Is.EqualTo(nyquist).Within(1E-15));
        }

        [Ignore("#1: DigitalSampler normalization bug - Lowpass and highpass filters are inconsistent due to boundary handling")]
        [TestCase]
        public void Convolute_with_lowpass_and_highpass_is_complementary () {
            double[] low = linear2.Convolute(source);
            double[] high = linear2Highpass.Convolute(source);
            double[] sum = low.Zip(high, (x, y) => x + y).ToArray();
            Assert.That(sum, Is.EqualTo(source));
        }

        [TestCase]
        public void Convolute_with_lowpass_and_highpass_is_complementary_at_nonboundaries () {
            double[] low = linear2.Convolute(source);
            double[] high = linear2Highpass.Convolute(source);
            double[] sum = low.Zip(high, (x, y) => x + y).ToArray();
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
        public void Downsample_preserves_constant_array () {
            Assert.That(linear2.Downsample(constant, 2, 0), Is.EqualTo(shortConstant));
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
        public void Downsample_can_handle_odd_length_arrays_with_shift_0 () {
            double[] odd = {1, 4, 9, 16, 25};
            double[] expected = {2, 9.5, 22};
            Assert.That(linear2.Downsample(odd, 2, 0), Is.EqualTo(expected));
        }

        [TestCase]
        public void Downsample_can_handle_odd_length_arrays_with_shift_1 () {
            double[] odd = {1, 4, 9, 16, 25};
            double[] expected = {4.5, 16.5};
            Assert.That(linear2.Downsample(odd, 2, 1), Is.EqualTo(expected));
        }

        [Ignore("#1: DigitalSampler normalization bug - Lowpass and highpass filters are inconsistent due to boundary handling")]
        [TestCase]
        public void Upsample_preserves_constant_array () {
            Assert.That(linear2.Upsample(shortConstant, 2, 1), Is.EqualTo(shortConstant));
        }

        [TestCase]
        public void Upsample_preserves_constant_array_at_nonboundaries () {
            double[] upsampled = linear2.Upsample(shortConstant, 2, 0);
            for( int i = 1; i < constant.Length - 1; i++ ) {
                Assert.That(upsampled[i], Is.EqualTo(constant[i]));
            }
        }

        [TestCase]
        public void Upsample_with_factor_2_and_shift_0 () {
            double[] downsampled = {2, 9.5, 25.5};
            double[] expected = {2.6666666666666666, 5.75, 9.5, 17.5, 25.5, 17};
            Assert.That(linear2.Upsample(downsampled, 2, 0), Is.EqualTo(expected));
        }

        [TestCase]
        public void Upsample_with_factor_2_and_shift_1 () {
            double[] downsampled = {4.5, 16.5, 32.333333333333333333333333333333};
            double[] expected = {3, 4.5, 10.5, 16.5, 24.4166666666666666, 43.1111111111111111};
            Assert.That(linear2.Upsample(downsampled, 2, 1), Is.EqualTo(expected));
        }

        [Ignore("#1: DigitalSampler normalization bug - Lowpass and highpass filters are inconsistent due to boundary handling")]
        [TestCase]
        public void Downsample_upsample_preserves_constant_array () {
            double[] downsampled = linear2.Downsample(constant, 2, 0);
            double[] upsampled = linear2.Upsample(downsampled, 2, 0);
            Assert.That(upsampled, Is.EqualTo(constant));
        }

        [TestCase]
        public void Downsample_upsample_preserves_constant_array_at_nonboundaries () {
            double[] downsampled = linear2.Downsample(constant, 2, 0);
            double[] upsampled = linear2.Upsample(downsampled, 2, 0);
            for( int i = 1; i < upsampled.Length - 1; i++ ) {
                Assert.That(upsampled[i], Is.EqualTo(constant[i]));
            }
        }

    }

}
