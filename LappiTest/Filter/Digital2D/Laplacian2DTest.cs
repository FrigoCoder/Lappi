using System.IO;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;
using Lappi.Filter.Digital2D;
using Lappi.Image;

using LappiTest.Image;

using NUnit.Framework;

namespace LappiTest.Filter.Digital2D {

    [TestFixture]
    public class Laplacian2DTest {

        private readonly Image<double> source = new Image<double>(new double[,]
            {{1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}});

        private readonly Filter1D analysis = new Filter1D(new Linear(), 2.0);
        private readonly Filter1D synthesis = new Filter1D(new Linear(), 2.0);
        private Laplacian2D<double> laplacian;

        [SetUp]
        public void SetUp () {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory + "\\Resources\\ImageTest");
            laplacian = new Laplacian2D<double>(analysis, synthesis);
        }

        [TestCase]
        public void Forward_transform_produces_correct_downsampled_image () {
            Image<double> expected = new Image<double>(new[,] {{2, 9.5, 25.5}, {2, 9.5, 25.5}});
            ImageTest.AssertEquals(laplacian.Forward(source)[0], expected);
        }

        [TestCase]
        public void Forward_transform_produces_correct_difference_image () {
            Image<double> expected = new Image<double>(new[,] {
                {-2.5555555555555555, -3.6666666666666666, -3.6666666666666666, -7.3333333333333333, -9.0, 13.3333333333333333},
                {-1.6666666666666666, -1.7500000000000000, -0.5000000000000000, -1.5000000000000000, -0.5, 19.0000000000000000},
                {-1.6666666666666666, -1.7500000000000000, -0.5000000000000000, -1.5000000000000000, -0.5, 19.0000000000000000},
                {-0.7777777777777777, +0.1666666666666666, +2.6666666666666666, +4.3333333333333333, +8.0, 24.6666666666666666}
            });
            ImageTest.AssertEquals(laplacian.Forward(source)[1], expected);
        }

        [TestCase]
        public void Inverse_transform_perfectly_reconstructs_signal () {
            ImageTest.AssertEquals(laplacian.Inverse(laplacian.Forward(source)), source);
        }

        [TestCase]
        public void Lenna_is_perfectly_reconstructed_after_5_steps () {
            Laplacian2D<YuvD> transform = new Laplacian2D<YuvD>(new Filter1D(new Dirichlet(3), 2), new Filter1D(new Dirichlet(2), 2));
            Image<YuvD> lenna = Image<YuvD>.Load("Lenna.png");
            ImageTest.AssertEquals(transform.Inverse(transform.Forward(lenna, 5)), lenna);
        }

    }

}
