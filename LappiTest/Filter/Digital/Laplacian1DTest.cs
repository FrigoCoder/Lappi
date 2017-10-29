﻿using System;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    using Laplacian1D = Laplacian1D<double>;

    [TestFixture]
    public class Laplacian1DTest {

        private readonly double[] source = {1, 4, 9, 16, 25, 36};
        private readonly DigitalFilter analysis = new DigitalAdapter(new Linear(), 2.0);
        private readonly DigitalFilter synthesis = new DigitalAdapter(new Linear(), 2.0);
        private Laplacian1D laplacian;

        [SetUp]
        public void SetUp () {
            laplacian = new Laplacian1D(analysis, synthesis);
        }

        [TestCase]
        public void Downsample_works_correctly () {
            double[] expected = {2, 9.5, 25.5};
            Assert.That(laplacian.Forward(source).Item1, Is.EqualTo(expected));
        }

        [TestCase]
        public void Difference_is_correct () {
            double[] expected = {-1.6666666666666666, -1.75, -0.5, -1.5, -0.5, 19};
            Assert.That(laplacian.Forward(source).Item2, Is.EqualTo(expected).Within(1E-15));
        }

        [TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal () {
            Assert.That(laplacian.Inverse(laplacian.Forward(source)), Is.EqualTo(source));
        }

        [TestCase]
        public void Forward_transform_can_handle_float_arrays () {
            Laplacian1D<float> transform = new Laplacian1D<float>(analysis, synthesis);
            float[] array = {1, 4, 9, 16, 25, 36};
            float[] low = {2f, 9.5f, 25.5f};
            float[] high = {-1.6666666666666666f, -1.75f, -0.5f, -1.5f, -0.5f, 19f};
            Assert.That(transform.Forward(array).Item1, Is.EqualTo(low));
            Assert.That(transform.Forward(array).Item2, Is.EqualTo(high).Within(1E-6));
        }

        [TestCase]
        public void Inverse_transform_can_handle_float_arrays () {
            Laplacian1D<float> transform = new Laplacian1D<float>(analysis, synthesis);
            float[] array = {1, 4, 9, 16, 25, 36};
            float[] low = {2f, 9.5f, 25.5f};
            float[] high = {-1.6666666666666666f, -1.75f, -0.5f, -1.5f, -0.5f, 19f};
            Assert.That(transform.Inverse(Tuple.Create(low, high)), Is.EqualTo(array).Within(1E-6));
        }

    }

}
