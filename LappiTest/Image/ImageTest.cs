using System;
using System.Drawing;
using System.IO;

using Lappi.Image;

using NUnit.Framework;

namespace LappiTest.Image {

    [TestFixture]
    public class ImageTest {

        public static void AssertEquals<T> (Image<T> actual, Image<T> expected) where T : new() {
            Assert.That(actual.Xs, Is.EqualTo(expected.Xs));
            Assert.That(actual.Ys, Is.EqualTo(expected.Ys));
            actual.ForEach((x, y) => Assert.That(actual[x, y], Is.EqualTo(expected[x, y]).Within(1E-14), "Fails at [" + x + ", " + y + "]"));
        }

        public static void AssertEquals (Image<YuvD> actual, Image<YuvD> expected) {
            Comparison<YuvD> comparer = (a, b) => Math.Abs(a.Y - b.Y) + Math.Abs(a.U - b.U) + Math.Abs(a.V - b.V) < 1E-15 ? 0 : 1;
            Assert.That(actual.Xs, Is.EqualTo(expected.Xs));
            Assert.That(actual.Ys, Is.EqualTo(expected.Ys));
            actual.ForEach((x, y) => Assert.That(actual[x, y], Is.EqualTo(expected[x, y]).Using(comparer), "Fails at [" + x + ", " + y + "]"));
        }

        private readonly Random random = new Random();

        [SetUp]
        public void SetUp () {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory + "\\Resources\\ImageTest");
        }

        [Test]
        public void Can_load_640_by_480_white_image () {
            Image<Rgb8> image = Image<Rgb8>.Load("640x480_white.png");
            AssertDimensions(image, 640, 480);
            AssertSolidColor(image, Color.White);
        }

        [Test]
        public void Can_load_800_by_600_black_image () {
            Image<Rgb8> image = Image<Rgb8>.Load("800x600_black.png");
            AssertDimensions(image, 800, 600);
            AssertSolidColor(image, Color.Black);
        }

        [Test]
        public void Can_load_512_by_512_red_image () {
            Image<Rgb8> image = Image<Rgb8>.Load("512x512_red.png");
            AssertDimensions(image, 512, 512);
            AssertSolidColor(image, Color.Red);
        }

        [Test]
        public void Can_load_512_by_512_green_image () {
            Image<Rgb8> image = Image<Rgb8>.Load("512x512_green.png");
            AssertDimensions(image, 512, 512);
            AssertSolidColor(image, Color.Lime);
        }

        [Test]
        public void Can_load_512_by_512_blue_image () {
            Image<Rgb8> image = Image<Rgb8>.Load("512x512_blue.png");
            AssertDimensions(image, 512, 512);
            AssertSolidColor(image, Color.Blue);
        }

        [Test]
        public void Corners_have_correct_coordinates () {
            Image<Rgb8> image = Image<Rgb8>.Load("black_red_green_blue.png");
            AssertPixel(image, 0, 0, Color.Black);
            AssertPixel(image, 1, 0, Color.Red);
            AssertPixel(image, 0, 1, Color.Lime);
            AssertPixel(image, 1, 1, Color.Blue);
        }

        [Test]
        public void Saved_image_is_same_as_loaded_image () {
            string fileName = Path.GetRandomFileName();
            try {
                Image<Rgb8> saved = CreateRandomImage<Rgb8>(512, 512);
                saved.Save(fileName);
                Image<Rgb8> loaded = Image<Rgb8>.Load(fileName);
                AssertEquals(loaded, saved);
            } finally {
                new FileInfo(fileName).Delete();
            }
        }

        [Test]
        public void Image_initialization_from_double_matrix () {
            double[,] pixels = {{1, 2, 3}, {4, 5, 6}};
            Image<double> image = new Image<double>(pixels);
            Assert.That(image.Xs, Is.EqualTo(3));
            Assert.That(image.Ys, Is.EqualTo(2));
            image.ForEach((x, y) => Assert.That(image[x, y], Is.EqualTo(pixels[y, x])));
        }

        [Test]
        public void Image_rows_can_be_indexed () {
            Image<double> image = new Image<double>(3, 2);
            double[] array = {1, 2, 3};
            image.Rows[0] = array;
            Assert.That(image.Rows[0], Is.EqualTo(array));
            for( int x = 0; x < image.Xs; x++ ) {
                Assert.That(image[x, 0], Is.EqualTo(array[x]));
            }
        }

        [Test]
        public void Image_columns_can_be_indexed () {
            Image<double> image = new Image<double>(3, 2);
            double[] array = {1, 2};
            image.Columns[0] = array;
            Assert.That(image.Columns[0], Is.EqualTo(array));
            for( int y = 0; y < image.Ys; y++ ) {
                Assert.That(image[0, y], Is.EqualTo(array[y]));
            }
        }

        [Test]
        public void Same_images_are_equal () {
            Image<double> image1 = new Image<double>(new double[,] {{1, 2, 3}, {4, 5, 6}});
            Image<double> image2 = new Image<double>(new double[,] {{1, 2, 3}, {4, 5, 6}});
            Assert.True(image1.Equals(image2));
            Assert.True(image1 == image2);
            Assert.False(image1 != image2);
        }

        [Test]
        public void Different_images_are_not_equal () {
            Image<double> image1 = new Image<double>(new double[,] {{1, 2, 3}, {4, 5, 6}});
            Image<double> image2 = new Image<double>(new double[,] {{1, 2, 3}, {4, 5, 7}});
            Assert.False(image1.Equals(image2));
            Assert.False(image1 == image2);
            Assert.True(image1 != image2);
        }

        [Test]
        public void Images_can_be_added () {
            Image<double> image1 = new Image<double>(new double[,] {{1, 1, 1}, {1, 1, 1}});
            Image<double> image2 = new Image<double>(new double[,] {{1, 2, 3}, {4, 5, 6}});
            Image<double> expected = new Image<double>(new double[,] {{2, 3, 4}, {5, 6, 7}});
            Assert.That(image1 + image2, Is.EqualTo(expected));
        }

        [Test]
        public void Images_can_be_subtracted () {
            Image<double> image1 = new Image<double>(new double[,] {{2, 3, 4}, {5, 6, 7}});
            Image<double> image2 = new Image<double>(new double[,] {{1, 1, 1}, {1, 1, 1}});
            Image<double> expected = new Image<double>(new double[,] {{1, 2, 3}, {4, 5, 6}});
            Assert.That(image1 - image2, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_works_correctly () {
            Image<double> image = new Image<double>(new double[,] {{1, 2, 3}, {4, 5, 6}});
            Assert.That(image.ToString(), Is.EqualTo("{{1, 2, 3}, {4, 5, 6}}"));
        }

        private Image<T> CreateRandomImage<T> (int xs, int ys) where T : new() =>
            new Image<T>(xs, ys, (x, y) => Colors.To<T>(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256))));

        private void AssertDimensions<T> (Image<T> image, int xs, int ys) where T : new() {
            Assert.That(image.Xs, Is.EqualTo(xs));
            Assert.That(image.Ys, Is.EqualTo(ys));
        }

        private void AssertPixel<T> (Image<T> image, int x, int y, Color color) where T : new() {
            Assert.That(image[x, y], Is.EqualTo(Colors.To<T>(color)));
        }

        private void AssertSolidColor<T> (Image<T> image, Color color) where T : new() {
            image.ForEach((x, y) => Assert.That(image[x, y], Is.EqualTo(Colors.To<T>(color))));
        }

    }

}
