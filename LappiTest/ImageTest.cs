using System;
using System.Drawing;
using System.IO;

using Lappi;

using NUnit.Framework;

namespace LappiTest {

    using T = RGB8;

    [TestFixture]
    public class ImageTest {

        private Image<T> Image;
        private readonly Random Random = new Random();

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
        public void Corners_have_correct_coordinates () {
            LoadImage("black_red_green_blue.png");
            AssertPixel(0, 0, Color.Black);
            AssertPixel(1, 0, Color.Red);
            AssertPixel(0, 1, Color.Lime);
            AssertPixel(1, 1, Color.Blue);
        }

        [Test]
        public void Saved_image_is_same_as_loaded_image () {
            string fileName = Path.GetRandomFileName();
            try {
                Image<T> saved = CreateRandomImage(512, 512);
                saved.Save(fileName);
                Image<T> loaded = Image<T>.Load(fileName);
                AssertEquals(saved, loaded);
            } finally {
                new FileInfo(fileName).Delete();
            }
        }

        private void LoadImage (string filename) {
            Image = Image<T>.Load("LappiTest\\Resources\\ImageTest\\" + filename);
        }

        private Image<T> CreateRandomImage (int xs, int ys) {
            Image<T> result = new Image<T>(xs, ys);
            for( int x = 0; x < xs; x++ ) {
                for( int y = 0; y < ys; y++ ) {
                    Color color = Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256));
                    result[x, y] = (T) Activator.CreateInstance(typeof(T), color);
                }
            }
            return result;
        }

        private void AssertDimensions (int xs, int ys) {
            Assert.That(Image.Xs, Is.EqualTo(xs));
            Assert.That(Image.Ys, Is.EqualTo(ys));
        }

        private void AssertPixel (int x, int y, Color color) {
            T value = (T) Activator.CreateInstance(typeof(T), color);
            Assert.That(Image[x, y], Is.EqualTo(value));
        }

        private void AssertSolidColor (Color color) {
            T value = (T) Activator.CreateInstance(typeof(T), color);
            for( int x = 0; x < Image.Xs; x++ ) {
                for( int y = 0; y < Image.Ys; y++ ) {
                    Assert.That(Image[x, y], Is.EqualTo(value));
                }
            }
        }

        private void AssertEquals (Image<T> expected, Image<T> actual) {
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
