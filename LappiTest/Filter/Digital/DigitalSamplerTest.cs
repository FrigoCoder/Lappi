﻿using System.Linq;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class DigitalSamplerTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36};

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
            double[] expected = {2, 4.5, 9.5, 16.5, 25.5, 32.333333333333333333333333333333};
            for( int i = 0; i < source.Length; i++ ) {
                Assert.That(sampler.Sample(source, i), Is.EqualTo(expected[i]));
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
            double[] expected = {2, 4.5, 9.5, 16.5, 25.5, 32.333333333333333333333333333333};
            Assert.That(sampler.Convolute(source), Is.EqualTo(expected));
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

        [TestCase]
        public void Convolution_with_lowpass_and_highpass_is_complementary () {
            DigitalSampler lowpass = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            DigitalSampler highpass = new DigitalSampler(new HighpassAdapter(new DigitalAdapter(new Linear(), 2.0)));
            double[] sum = lowpass.Convolute(source).Zip(highpass.Convolute(source), (x, y) => x + y).ToArray();
            Assert.That(sum, Is.EqualTo(source));
        }

        [TestCase]
        public void Convolution_with_lowpass_and_highpass_is_complementary_at_nonboundaries () {
            DigitalSampler lowpass = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            DigitalSampler highpass = new DigitalSampler(new HighpassAdapter(new DigitalAdapter(new Linear(), 2.0)));
            double[] sum = lowpass.Convolute(source).Zip(highpass.Convolute(source), (x, y) => x + y).ToArray();
            for( int i = 1; i < sum.Length - 1; i++ ) {
                Assert.That(sum[i], Is.EqualTo(source[i]));
            }
        }

        [TestCase]
        public void Downsample_with_factor_2_and_shift_0 () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            double[] expected = {2, 9.5, 25.5};
            Assert.That(sampler.Downsample(source, 2, 0), Is.EqualTo(expected));
        }

        [TestCase]
        public void Downsample_with_factor_2_and_shift_1 () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            double[] expected = {4.5, 16.5, 32.333333333333333333333333333333};
            Assert.That(sampler.Downsample(source, 2, 1), Is.EqualTo(expected));
        }

        [TestCase]
        public void Upsample_with_factor_2_and_shift_0 () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            double[] downsampled = {2, 9.5, 25.5};
            double[] expected = {1.3333333333333333, 2.875, 4.75, 8.75, 12.75, 8.5};
            Assert.That(sampler.Upsample(downsampled, 2, 0), Is.EqualTo(expected));
        }

        [TestCase]
        public void Upsample_with_factor_2_and_shift_1 () {
            DigitalSampler sampler = new DigitalSampler(new DigitalAdapter(new Linear(), 2.0));
            double[] downsampled = {4.5, 16.5, 32.333333333333333333333333333333};
            double[] expected = {1.5, 2.25, 5.25, 8.25, 12.2083333333333333, 21.5555555555555555};
            Assert.That(sampler.Upsample(downsampled, 2, 1), Is.EqualTo(expected));
        }

        [TestCase]
        public void Can_handle_float_arrays () {
            DigitalSampler<float> sampler = new DigitalSampler<float>(new DigitalAdapter(new Linear(), 2.0));
            float[] array = {1, 4, 9, 16, 25, 36};
            float[] expected = {2, 4.5f, 9.5f, 16.5f, 25.5f, 32.333333333333333333333333333333f};
            Assert.That(sampler.Convolute(array), Is.EqualTo(expected));
        }

    }

}
