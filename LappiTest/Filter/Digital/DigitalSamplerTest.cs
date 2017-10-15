using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class DigitalSamplerTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36};

        [TestCase]
        public void Sampling_with_scale_1_linear_filter_should_return_array_value () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 1.0));
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(sampler.Sample(source, i), Is.EqualTo(source[i]));
            }
        }

        [TestCase]
        public void Sampling_with_scale_2_linear_filter_should_blur_linearly () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            double[] expected = {2, 4.5, 9.5, 16.5, 25.5, 32.333333333333333333333333333333};
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(sampler.Sample(source, i), Is.EqualTo(expected[i]));
            }
        }

        [TestCase]
        public void Sampling_beyond_left_boundary_returns_0 () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            Assert.That(sampler.Sample(source, -2), Is.EqualTo(0));
        }

        [TestCase]
        public void Sampling_beyond_right_boundary_returns_0 () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            Assert.That(sampler.Sample(source, source.Length + 1), Is.EqualTo(0));
        }

    }

}
