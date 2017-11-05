using System.Drawing;

using Lappi;

using NUnit.Framework;

namespace LappiTest.Image {

    [TestFixture]
    public class YuvDTest {

        [Test]
        public void Color_conversion_to_YUV () {
            Assert.That(new YuvD(Color.Red).Y, Is.EqualTo(0.299));
            Assert.That(new YuvD(Color.Red).U, Is.EqualTo(-0.14713769751693002257336343115124));
            Assert.That(new YuvD(Color.Red).V, Is.EqualTo(0.615));

            Assert.That(new YuvD(Color.Lime).Y, Is.EqualTo(0.587).Within(1E-15));
            Assert.That(new YuvD(Color.Lime).U, Is.EqualTo(-0.28886230248306997742663656884876));
            Assert.That(new YuvD(Color.Lime).V, Is.EqualTo(-0.51498573466476462196861626248217));

            Assert.That(new YuvD(Color.Blue).Y, Is.EqualTo(0.114));
            Assert.That(new YuvD(Color.Blue).U, Is.EqualTo(0.436));
            Assert.That(new YuvD(Color.Blue).V, Is.EqualTo(-0.10001426533523537803138373751783));
        }

        [Test]
        public void YUV_conversion_to_Color () {
            Assert.That(new YuvD(1, 0, 0).ToColor().ToArgb, Is.EqualTo(Color.White.ToArgb()));
            Assert.That(new YuvD(0.299, -0.147, 0.615).ToColor().ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
            Assert.That(new YuvD(0.587, -0.288, -0.514).ToColor().ToArgb(), Is.EqualTo(Color.Lime.ToArgb()));
            Assert.That(new YuvD(0.114, 0.436, -0.100).ToColor().ToArgb(), Is.EqualTo(Color.Blue.ToArgb()));
        }

        [Test]
        public void Color_conversion_to_YUV_and_back () {
            Assert.That(new YuvD(Color.Red).ToColor().ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
            Assert.That(new YuvD(Color.Lime).ToColor().ToArgb(), Is.EqualTo(Color.Lime.ToArgb()));
            Assert.That(new YuvD(Color.Blue).ToColor().ToArgb(), Is.EqualTo(Color.Blue.ToArgb()));
        }

    }

}
