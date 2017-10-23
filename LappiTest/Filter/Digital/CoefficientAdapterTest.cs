using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class CoefficientAdapterTest {

        [TestCase]
        public void Test_3_tap_filter () {
            int center = 1;
            double[] coefficients = {0.5, 1.0, 0.5};
            DigitalFilter filter = new CoefficientAdapter(center, coefficients);
            Assert.That(filter.Left, Is.EqualTo(-1));
            Assert.That(filter.Right, Is.EqualTo(1));
            Assert.That(filter.Radius, Is.EqualTo(1));
            Assert.That(filter.Coefficients, Is.EqualTo(coefficients));
        }

        [TestCase]
        public void Test_5_tap_filter_offcenter () {
            int center = 1;
            double[] coefficients = {0.1, 0.5, 1.0, 0.5, 0.1};
            DigitalFilter filter = new CoefficientAdapter(center, coefficients);
            Assert.That(filter.Left, Is.EqualTo(-1));
            Assert.That(filter.Right, Is.EqualTo(3));
            Assert.That(filter.Radius, Is.EqualTo(3));
            Assert.That(filter.Coefficients, Is.EqualTo(coefficients));
        }

    }

}
