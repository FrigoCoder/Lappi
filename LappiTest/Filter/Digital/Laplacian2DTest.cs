using Lappi;
using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

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
                {-1.6666666666666666, -1.75, -0.5, -1.5, -0.5, 19}, {-1.6666666666666666, -1.75, -0.5, -1.5, -0.5, 19},
                {-1.6666666666666666, -1.75, -0.5, -1.5, -0.5, 19}, {-1.6666666666666666, -1.75, -0.5, -1.5, -0.5, 19}
            });
            AssertEquals(laplacian.Forward(source).Item2, expected);
        }

        [TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal () {
            AssertEquals(laplacian.Inverse(laplacian.Forward(source)), source);
        }

        private void AssertEquals (Image<double> actual, Image<double> expected) {
            Assert.That(actual.Xs, Is.EqualTo(expected.Xs));
            Assert.That(actual.Ys, Is.EqualTo(expected.Ys));
            for( int x = 0; x < actual.Xs; x++ ) {
                for( int y = 0; y < actual.Ys; y++ ) {
                    Assert.That(actual[x, y], Is.EqualTo(expected[x, y]), "Fails at [" + x + ", " + y + "]");
                }
            }
        }

    }

}
