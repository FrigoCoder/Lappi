using Lappi.Filter.Analog;
using Lappi.Filter.Digital;
using Lappi.Filter.Digital2D;

using NUnit.Framework;

namespace LappiTest.Filter.Digital2D {

    public class DigitalFilter2DTest {

        [Test]
        public void Coefficient_adapter_constructor () {
            AssertFilter(new DigitalFilter2D(0, 1, new double[,] {{1, 2}, {3, 4}}), 0, 1, -1, 0, new double[,] {{1, 2}, {3, 4}});
        }

        [Test]
        public void Separable_adapter_constructor () {
            AssertFilter(new DigitalFilter2D(new DigitalFilter(new Linear(), 1)), 0, 0, 0, 0, new[,] {{1.0}});
            AssertFilter(new DigitalFilter2D(new DigitalFilter(new Linear(), 2)), -1, 1, -1, 1,
                new[,] {{0.25, 0.5, 0.25}, {0.5, 1.0, 0.5}, {0.25, 0.5, 0.25}});
            AssertFilter(new DigitalFilter2D(new DigitalFilter(new Linear(), 3)), -2, 2, -2, 2,
                new[,] {
                    {0.111111111111111, 0.222222222222222, 0.333333333333333, 0.222222222222222, 0.111111111111111},
                    {0.222222222222222, 0.444444444444444, 0.666666666666666, 0.444444444444444, 0.222222222222222},
                    {0.333333333333333, 0.666666666666666, 1.000000000000000, 0.666666666666666, 0.333333333333333},
                    {0.222222222222222, 0.444444444444444, 0.666666666666666, 0.444444444444444, 0.222222222222222},
                    {0.111111111111111, 0.222222222222222, 0.333333333333333, 0.222222222222222, 0.111111111111111}
                });
        }

        private void AssertFilter (DigitalFilter2D filter, int left, int right, int top, int bottom, double[,] coefficients) {
            Assert.That(filter.Left, Is.EqualTo(left));
            Assert.That(filter.Right, Is.EqualTo(right));
            Assert.That(filter.Top, Is.EqualTo(top));
            Assert.That(filter.Bottom, Is.EqualTo(bottom));
            Assert.That(filter.Coefficients, Is.EqualTo(coefficients).Within(1E-15));
        }

    }

}
