using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class DigitalSamplerTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36};
        private readonly double[] blurred = {2, 4.5, 9.5, 16.5, 25.5, 32.333333333333333333333333333333};

        [TestCase]
        public void Sample_with_scale_1_linear_filter_returns_array_value () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 1.0));
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(sampler.Sample(source, i), Is.EqualTo(source[i]));
            }
        }

        [TestCase]
        public void Sample_with_scale_2_linear_filter_blurs_linearly () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(sampler.Sample(source, i), Is.EqualTo(blurred[i]));
            }
        }

        [TestCase]
        public void Sample_beyond_left_boundary_returns_0 () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            Assert.That(sampler.Sample(source, -2), Is.EqualTo(0));
        }

        [TestCase]
        public void Sample_beyond_right_boundary_returns_0 () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            Assert.That(sampler.Sample(source, source.Length + 1), Is.EqualTo(0));
        }

        [TestCase]
        public void Convolute_with_scale_1_linear_filter_returns_array () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 1.0));
            Assert.That(sampler.Convolute(source), Is.EqualTo(source));
        }

        [TestCase]
        public void Convolute_with_scale_2_linear_filter_blurs_linearly () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            Assert.That(sampler.Convolute(source), Is.EqualTo(blurred));
        }

        [TestCase]
        public void Downsample_with_factor_2_and_shift_0 () {
            double[] expected = {2, 9.5, 25.5};
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            Assert.That(sampler.Downsample(source, 2, 0), Is.EqualTo(expected));
        }

        [TestCase]
        public void Downsample_with_factor_2_and_shift_1 () {
            double[] expected = {4.5, 16.5, 32.333333333333333333333333333333};
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            Assert.That(sampler.Downsample(source, 2, 1), Is.EqualTo(expected));
        }

        [TestCase]
        public void Can_handle_float_arrays () {
            float[] array = {1, 4, 9, 16, 25, 36};
            float[] expected = {2, 4.5f, 9.5f, 16.5f, 25.5f, 32.333333333333333333333333333333f};
            DigitalSampler<float> sampler = new DigitalSampler<float>(new DigitalAdapter(new Linear(), 2.0));
            Assert.That(sampler.Convolute(array), Is.EqualTo(expected));
        }

    }

}
