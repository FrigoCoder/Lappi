using System;
using System.Drawing;
using System.IO;

using Lappi;

using NUnit.Framework;

namespace LappiTest {

    [TestFixture]
    public class ImageTest {

        private readonly Random random = new Random();

        [Test]
        public void Can_load_640_by_480_white_image () {
            Image<Rgb8> image = LoadImage<Rgb8>("640x480_white.png");
            AssertDimensions(image, 640, 480);
            AssertSolidColor(image, Color.White);
        }

        [Test]
        public void Can_load_800_by_600_black_image () {
            Image<Rgb8> image = LoadImage<Rgb8>("800x600_black.png");
            AssertDimensions(image, 800, 600);
            AssertSolidColor(image, Color.Black);
        }

        [Test]
        public void Can_load_512_by_512_red_image () {
            Image<Rgb8> image = LoadImage<Rgb8>("512x512_red.png");
            AssertDimensions(image, 512, 512);
            AssertSolidColor(image, Color.Red);
        }

        [Test]
        public void Can_load_512_by_512_green_image () {
            Image<Rgb8> image = LoadImage<Rgb8>("512x512_green.png");
            AssertDimensions(image, 512, 512);
            AssertSolidColor(image, Color.Lime);
        }

        [Test]
        public void Can_load_512_by_512_blue_image () {
            Image<Rgb8> image = LoadImage<Rgb8>("512x512_blue.png");
            AssertDimensions(image, 512, 512);
            AssertSolidColor(image, Color.Blue);
        }

        [Test]
        public void Corners_have_correct_coordinates () {
            Image<Rgb8> image = LoadImage<Rgb8>("black_red_green_blue.png");
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
                AssertEquals(saved, loaded);
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
            for( int x = 0; x < 3; x++ ) {
                for( int y = 0; y < 2; y++ ) {
                    Assert.That(image[x, y], Is.EqualTo(pixels[y, x]));
                }
            }
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

        private Image<T> LoadImage<T> (string filename) {
            return Image<T>.Load("LappiTest\\Resources\\ImageTest\\" + filename);
        }

        private Image<T> CreateRandomImage<T> (int xs, int ys) {
            Image<T> result = new Image<T>(xs, ys);
            for( int x = 0; x < xs; x++ ) {
                for( int y = 0; y < ys; y++ ) {
                    Color color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                    result[x, y] = (T) Activator.CreateInstance(typeof(T), color);
                }
            }
            return result;
        }

        private void AssertDimensions<T> (Image<T> image, int xs, int ys) {
            Assert.That(image.Xs, Is.EqualTo(xs));
            Assert.That(image.Ys, Is.EqualTo(ys));
        }

        private void AssertPixel<T> (Image<T> image, int x, int y, Color color) {
            T value = (T) Activator.CreateInstance(typeof(T), color);
            Assert.That(image[x, y], Is.EqualTo(value));
        }

        private void AssertSolidColor<T> (Image<T> image, Color color) {
            T value = (T) Activator.CreateInstance(typeof(T), color);
            for( int x = 0; x < image.Xs; x++ ) {
                for( int y = 0; y < image.Ys; y++ ) {
                    Assert.That(image[x, y], Is.EqualTo(value));
                }
            }
        }

        private void AssertEquals<T> (Image<T> expected, Image<T> actual) {
            Assert.That(actual.Xs, Is.EqualTo(expected.Xs));
            Assert.That(actual.Ys, Is.EqualTo(expected.Ys));
            for( int x = 0; x < actual.Xs; x++ ) {
                for( int y = 0; y < actual.Ys; y++ ) {
                    Assert.That(actual[x, y], Is.EqualTo(expected[x, y]));
                }
            }
        }

    }

}
