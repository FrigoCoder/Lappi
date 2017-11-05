using System;

using Lappi;
using Lappi.Filter.Analog;
using Lappi.Filter.Digital;
using Lappi.Image;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class Laplacian2DTest {

        private readonly Image<double> source = new Image<double>(new double[,]
            {{1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}});

        private readonly DigitalFilter analysis = new DigitalAdapter(new Linear(), 2.0);
        private readonly DigitalFilter synthesis = new DigitalAdapter(new Linear(), 2.0);
        private Laplacian2D<double> laplacian;

        [SetUp]
        public void SetUp () {
            laplacian = new Laplacian2D<double>(analysis, synthesis);
        }

        [TestCase]
        public void Forward_transform_produces_correct_downsampled_image () {
            Image<double> expected = new Image<double>(new[,] {{2, 9.5, 25.5}, {2, 9.5, 25.5}});
            AssertEquals(laplacian.Forward(source).Item1, expected);
        }

        [TestCase]
        public void Forward_transform_produces_correct_difference_image () {
            Image<double> expected = new Image<double>(new[,] {
                {-2.5555555555555555, -3.6666666666666666, -3.6666666666666666, -7.3333333333333333, -9.0, 13.3333333333333333},
                {-1.6666666666666666, -1.7500000000000000, -0.5000000000000000, -1.5000000000000000, -0.5, 19.0000000000000000},
                {-1.6666666666666666, -1.7500000000000000, -0.5000000000000000, -1.5000000000000000, -0.5, 19.0000000000000000},
                {-0.7777777777777777, +0.1666666666666666, +2.6666666666666666, +4.3333333333333333, +8.0, 24.6666666666666666}
            });
            AssertEquals(laplacian.Forward(source).Item2, expected);
        }

        [TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal () {
            AssertEquals(laplacian.Inverse(laplacian.Forward(source)), source);
        }

        [TestCase]
        public void Lenna_is_perfectly_reconstructed_after_5_steps () {
            Laplacian2D<YuvD> transform = new Laplacian2D<YuvD>(CDF97.AnalysisLowpass, CDF97.SynthesisLowpass);
            Image<YuvD> lenna = Image<YuvD>.Load("LappiTest\\Resources\\ImageTest\\Lenna.png");
            Tuple<Image<YuvD>, Image<YuvD>> level1 = transform.Forward(lenna);
            Tuple<Image<YuvD>, Image<YuvD>> level2 = transform.Forward(level1.Item1);
            Tuple<Image<YuvD>, Image<YuvD>> level3 = transform.Forward(level2.Item1);
            Tuple<Image<YuvD>, Image<YuvD>> level4 = transform.Forward(level3.Item1);
            Tuple<Image<YuvD>, Image<YuvD>> level5 = transform.Forward(level4.Item1);

            level4.Item2.Save("c:\\test.png");

            level4 = Tuple.Create(transform.Inverse(level5), level4.Item2);
            level3 = Tuple.Create(transform.Inverse(level4), level3.Item2);
            level2 = Tuple.Create(transform.Inverse(level3), level2.Item2);
            level1 = Tuple.Create(transform.Inverse(level2), level1.Item2);

            AssertEquals(transform.Inverse(level1), lenna);
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

    }

}
