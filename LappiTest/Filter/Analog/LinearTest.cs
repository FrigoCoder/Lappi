using Lappi.Filter.Analog;

using NUnit.Framework;

namespace LappiTest.Filter.Analog {

    [TestFixture]
    public class LinearTest {

        private readonly Linear filter = new Linear();

        [TestCase]
        public void Linear_test () {
            Assert.That(filter.Kernel(-1.5), Is.EqualTo(0.0));
            Assert.That(filter.Kernel(-1.0), Is.EqualTo(0.0));
            Assert.That(filter.Kernel(-0.5), Is.EqualTo(0.5));
            Assert.That(filter.Kernel(0.0), Is.EqualTo(1.0));
            Assert.That(filter.Kernel(0.5), Is.EqualTo(0.5));
            Assert.That(filter.Kernel(1.0), Is.EqualTo(0.0));
            Assert.That(filter.Kernel(1.5), Is.EqualTo(0.0));
        }

    }

}
