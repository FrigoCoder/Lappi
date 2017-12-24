using System;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;
using Lappi.Filter.Digital2D;
using Lappi.Image;

using NUnit.Framework;

namespace LappiTest.Filter.Digital2D {

    [TestFixture]
    public class LaplacianSeparableTest {

        private readonly Image<double> source = new Image<double>(new double[,]
            {{1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}});

        private readonly DigitalFilter analysis = new DigitalAdapter(new Linear(), 2.0);
        private readonly DigitalFilter synthesis = new DigitalAdapter(new Linear(), 2.0);
        private LaplacianSeparable<double> laplacian;

        [SetUp]
        public void SetUp () {
            laplacian = new LaplacianSeparable<double>(analysis, synthesis);
        }

        [TestCase]
        public void Forward_transform_produces_correct_downsampled_image () {
            Image<double> expected = new Image<double>(new[,] {{2, 9.5, 25.5}, {2, 9.5, 25.5}});
            AssertEquals(laplacian.Forward(source)[0], expected);
        }

        [TestCase]
        public void Forward_transform_produces_correct_difference_image () {
            Image<double> expected = new Image<double>(new[,] {
                {-2.5555555555555555, -3.6666666666666666, -3.6666666666666666, -7.3333333333333333, -9.0, 13.3333333333333333},
                {-1.6666666666666666, -1.7500000000000000, -0.5000000000000000, -1.5000000000000000, -0.5, 19.0000000000000000},
                {-1.6666666666666666, -1.7500000000000000, -0.5000000000000000, -1.5000000000000000, -0.5, 19.0000000000000000},
                {-0.7777777777777777, +0.1666666666666666, +2.6666666666666666, +4.3333333333333333, +8.0, 24.6666666666666666}
            });
            AssertEquals(laplacian.Forward(source)[1], expected);
        }

        [TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal () {
            AssertEquals(laplacian.Inverse(laplacian.Forward(source)), source);
        }

        [TestCase]
        public void Lenna_is_perfectly_reconstructed_after_5_steps () {
            LaplacianSeparable<YuvD> transform = new LaplacianSeparable<YuvD>(CDF97.AnalysisLowpass, CDF97.SynthesisLowpass);
            Image<YuvD> lenna = Image<YuvD>.Load("LappiTest\\Resources\\ImageTest\\Lenna.png");
            AssertEquals(transform.Inverse(transform.Forward(lenna, 5)), lenna);
        }

        private void AssertEquals<T> (Image<T> actual, Image<T> expected) {
            Assert.That(actual.Xs, Is.EqualTo(expected.Xs));
            Assert.That(actual.Ys, Is.EqualTo(expected.Ys));
            for( int x = 0; x < actual.Xs; x++ ) {
                for( int y = 0; y < actual.Ys; y++ ) {
                    Assert.That(actual[x, y], Is.EqualTo(expected[x, y]).Within(1E-14), "Fails at [" + x + ", " + y + "]");
                }
            }
        }

        private void AssertEquals (Image<YuvD> actual, Image<YuvD> expected) {
            Comparison<YuvD> comparer = (a, b) => Math.Abs(a.Y - b.Y) + Math.Abs(a.U - b.U) + Math.Abs(a.V - b.V) < 1E-15 ? 0 : 1;
            Assert.That(actual.Xs, Is.EqualTo(expected.Xs));
            Assert.That(actual.Ys, Is.EqualTo(expected.Ys));
            for( int x = 0; x < actual.Xs; x++ ) {
                for( int y = 0; y < actual.Ys; y++ ) {
                    Assert.That(actual[x, y], Is.EqualTo(expected[x, y]).Using(comparer));
                }
            }
        }

    }

}
