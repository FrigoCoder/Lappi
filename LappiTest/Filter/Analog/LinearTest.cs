using Lappi.Filter.Analog;

using NUnit.Framework;

namespace LappiTest.Filter.Analog {

    [TestFixture]
    public class LinearTest {

        private readonly Linear filter = new Linear();

        [TestCase]
        public void Linear_test () {
            Assert.That(filter.Function(-1.5), Is.EqualTo(0.0));
            Assert.That(filter.Function(-1.0), Is.EqualTo(0.0));
            Assert.That(filter.Function(-0.5), Is.EqualTo(0.5));
            Assert.That(filter.Function(0.0), Is.EqualTo(1.0));
            Assert.That(filter.Function(0.5), Is.EqualTo(0.5));
            Assert.That(filter.Function(1.0), Is.EqualTo(0.0));
            Assert.That(filter.Function(1.5), Is.EqualTo(0.0));
        }

    }

}
