using System;
using System.Drawing;

using Lappi;

using NUnit.Framework;

namespace LappiTest {

    using T = RGB8;

    [TestFixture]
    public class ImageTest {

        private Image<T> image;

        [Test]
        public void Can_load_640_by_480_white_image () {
            LoadImage("640x480_white.png");
            AssertDimensions(640, 480);
            AssertSolidColor(Color.White);
        }

        [Test]
        public void Can_load_800_by_600_black_image () {
            LoadImage("800x600_black.png");
            AssertDimensions(800, 600);
            AssertSolidColor(Color.Black);
        }

        [Test]
        public void Can_load_512_by_512_red_image () {
            LoadImage("512x512_red.png");
            AssertDimensions(512, 512);
            AssertSolidColor(Color.Red);
        }

        [Test]
        public void Can_load_512_by_512_green_image () {
            LoadImage("512x512_green.png");
            AssertDimensions(512, 512);
            AssertSolidColor(Color.Lime);
        }

        [Test]
        public void Can_load_512_by_512_blue_image () {
            LoadImage("512x512_blue.png");
            AssertDimensions(512, 512);
            AssertSolidColor(Color.Blue);
        }

        [Test]
        public void corners_have_correct_coordinates () {
            LoadImage("black_red_green_blue.png");
            AssertPixel(0, 0, Color.Black);
            AssertPixel(1, 0, Color.Red);
            AssertPixel(0, 1, Color.Lime);
            AssertPixel(1, 1, Color.Blue);
        }

        private void LoadImage (string filename) {
            image = Image<T>.Load("LappiTest\\Resources\\ImageTest\\" + filename);
        }

        private void AssertDimensions (int xs, int ys) {
            Assert.That(image.xs, Is.EqualTo(xs));
            Assert.That(image.ys, Is.EqualTo(ys));
        }

        private void AssertSolidColor (Color color) {
            T value = (T) Activator.CreateInstance(typeof(T), color);
            for( int x = 0; x < image.xs; x++ ) {
                for( int y = 0; y < image.ys; y++ ) {
                    Assert.That(image[x, y], Is.EqualTo(value));
                }
            }
        }

        private void AssertPixel (int x, int y, Color color) {
            T value = (T) Activator.CreateInstance(typeof(T), color);
            Assert.That(image[x, y], Is.EqualTo(value));
        }

    }

}
