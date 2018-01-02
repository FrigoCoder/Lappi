using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    public class DigitalFilterTest {

        [TestCase]
        public void Coefficient_adapter_constructor () {
            AssertFilter(new DigitalFilter(1, new[] {0.5, 1.0, 0.5}), -1, 1, new[] {0.5, 1.0, 0.5});
            AssertFilter(new DigitalFilter(1, new[] {0.1, 0.5, 1.0, 0.5, 0.1}), -1, 3, new[] {0.1, 0.5, 1.0, 0.5, 0.1});
        }

        [TestCase]
        public void Analog_adapter_constructor () {
            AssertFilter(new DigitalFilter(new Linear(), 1.0), 0, 0, new[] {1.0});
            AssertFilter(new DigitalFilter(new Linear(), 2.0), -1, 1, new[] {0.5, 1.0, 0.5});
            AssertFilter(new DigitalFilter(new Linear(), 3.0), -2, 2, new[] {1.0 / 3.0, 2.0 / 3.0, 1, 2.0 / 3.0, 1.0 / 3.0});
            AssertFilter(new DigitalFilter(new Linear(), 4.0), -3, 3, new[] {0.25, 0.5, 0.75, 1.0, 0.75, 0.5, 0.25});
        }

        [TestCase]
        public void ToString_is_correct () {
            DigitalFilter filter = new DigitalFilter(new Linear(), 2.0);
            Assert.That(filter.ToString(), Is.EqualTo("DigitalFilter{Left = -1, Right = 1, Radius = 1, Coefficients=[0,5, 1, 0,5]}"));
        }

        private void AssertFilter (DigitalFilter filter, int left, int right, double[] coefficients) {
            Assert.That(filter.Left, Is.EqualTo(left));
            Assert.That(filter.Right, Is.EqualTo(right));
            Assert.That(filter.Coefficients, Is.EqualTo(coefficients).Within(1E-15));
        }

    }

}
