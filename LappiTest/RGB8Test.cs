using System.Drawing;

using Lappi;

using NUnit.Framework;

namespace LappiTest {

    [TestFixture]
    public class RGB8Test {

        [Test]
        public void Conversion_from_Color () {
            Assert.That(new RGB8(Color.Red), Is.EqualTo(new RGB8(255, 0, 0)));
            Assert.That(new RGB8(Color.Lime), Is.EqualTo(new RGB8(0, 255, 0)));
            Assert.That(new RGB8(Color.Blue), Is.EqualTo(new RGB8(0, 0, 255)));
        }

        [Test]
        public void Conversion_to_Color () {
            Assert.That(new RGB8(255, 0, 0).ToColor().ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
            Assert.That(new RGB8(0, 255, 0).ToColor().ToArgb(), Is.EqualTo(Color.Lime.ToArgb()));
            Assert.That(new RGB8(0, 0, 255).ToColor().ToArgb(), Is.EqualTo(Color.Blue.ToArgb()));
        }

    }

}
