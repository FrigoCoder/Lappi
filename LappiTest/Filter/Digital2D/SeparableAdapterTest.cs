using Lappi.Filter.Analog;
using Lappi.Filter.Digital;
using Lappi.Filter.Digital2D;

using NUnit.Framework;

namespace LappiTest.Filter.Digital2D {

    public class SeparableAdapterTest {

        [Test]
        public void Ranges_are_correct () {
            AssertRanges(new Linear(), 1.0, 0, 0, 0, 0);
            AssertRanges(new Linear(), 2.0, -1, 1, -1, 1);
            AssertRanges(new Linear(), 3.0, -2, 2, -2, 2);
            AssertRanges(new Linear(), 4.0, -3, 3, -3, 3);
        }

        [Test]
        public void Coefficients_are_correct () {
            AssertCoefficients(new Linear(), 1.0, new[,] {{1.0}});
            AssertCoefficients(new Linear(), 2.0, new[,] {{0.25, 0.5, 0.25}, {0.5, 1.0, 0.5}, {0.25, 0.5, 0.25}});
            AssertCoefficients(new Linear(), 3.0,
                new[,] {
                    {0.111111111111111, 0.222222222222222, 0.333333333333333, 0.222222222222222, 0.111111111111111},
                    {0.222222222222222, 0.444444444444444, 0.666666666666666, 0.444444444444444, 0.222222222222222},
                    {0.333333333333333, 0.666666666666666, 1.000000000000000, 0.666666666666666, 0.333333333333333},
                    {0.222222222222222, 0.444444444444444, 0.666666666666666, 0.444444444444444, 0.222222222222222},
                    {0.111111111111111, 0.222222222222222, 0.333333333333333, 0.222222222222222, 0.111111111111111}
                });
        }

        private void AssertRanges (AnalogFilter analog, double scale, int left, int right, int top, int bottom) {
            SeparableAdapter adapter = new SeparableAdapter(new DigitalFilter(analog, scale));
            Assert.That(adapter.Left, Is.EqualTo(left));
            Assert.That(adapter.Right, Is.EqualTo(right));
            Assert.That(adapter.Top, Is.EqualTo(top));
            Assert.That(adapter.Bottom, Is.EqualTo(bottom));
        }

        private void AssertCoefficients (AnalogFilter filter, double scale, double[,] coefficients) {
            Assert.That(new SeparableAdapter(new DigitalFilter(filter, scale)).Coefficients, Is.EqualTo(coefficients).Within(1E-15));
        }

    }

}
