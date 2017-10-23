using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class DigitalAdapterTest {

        [TestCase]
        public void Linear_filter_ranges_are_correct () {
            AssertRanges(new Linear(), 1.0, 0, 0, 0);
            AssertRanges(new Linear(), 2.0, -1, 1, 1);
            AssertRanges(new Linear(), 3.0, -2, 2, 2);
            AssertRanges(new Linear(), 4.0, -3, 3, 3);
        }

        [TestCase]
        public void Linear_filter_coefficients_are_correct () {
            AssertCoefficients(new Linear(), 1.0, new[] {1.0});
            AssertCoefficients(new Linear(), 2.0, new[] {0.5, 1.0, 0.5});
            AssertCoefficients(new Linear(), 3.0, new[] {1.0 / 3.0, 2.0 / 3.0, 1, 2.0 / 3.0, 1.0 / 3.0});
            AssertCoefficients(new Linear(), 4.0, new[] {0.25, 0.5, 0.75, 1.0, 0.75, 0.5, 0.25});
        }

        private void AssertRanges (AnalogFilter filter, double scale, int left, int right, int radius) {
            DigitalAdapter adapter = new DigitalAdapter(filter, scale);
            Assert.That(adapter.Left, Is.EqualTo(left));
            Assert.That(adapter.Right, Is.EqualTo(right));
            Assert.That(adapter.Radius, Is.EqualTo(radius));
        }

        private void AssertCoefficients (AnalogFilter filter, double scale, double[] coefficients) {
            Assert.That(new DigitalAdapter(filter, scale).Coefficients, Is.EqualTo(coefficients).Within(1E-15));
        }

    }

}
