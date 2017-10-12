using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class DigitalAdapterTest {

        [TestCase]
        public void Linear_filter_with_scale_1 () {
            DigitalFilter adapter = new DigitalAdapter(new Linear(), 1.0);
            AssertAdapter(adapter, 0, 0, 0, new[] {1.0});
        }

        [TestCase]
        public void Linear_filter_with_scale_2 () {
            DigitalFilter adapter = new DigitalAdapter(new Linear(), 2.0);
            AssertAdapter(adapter, -1, +1, 1, new[] {0.5, 1.0, 0.5});
        }

        [TestCase]
        public void Linear_filter_with_scale_3 () {
            DigitalFilter adapter = new DigitalAdapter(new Linear(), 3.0);
            AssertAdapter(adapter, -2, +2, 2, new[] {1.0 / 3.0, 2.0 / 3.0, 1, 2.0 / 3.0, 1.0 / 3.0});
        }

        [TestCase]
        public void Linear_filter_with_scale_4 () {
            DigitalFilter adapter = new DigitalAdapter(new Linear(), 4.0);
            AssertAdapter(adapter, -3, +3, 3, new[] {0.25, 0.5, 0.75, 1.0, 0.75, 0.5, 0.25});
        }

        private void AssertAdapter (DigitalFilter adapter, int left, int right, int radius, double[] coeffs) {
            Assert.That(adapter.Left, Is.EqualTo(left));
            Assert.That(adapter.Right, Is.EqualTo(right));
            Assert.That(adapter.Radius, Is.EqualTo(radius));
            for( int x = left; x <= right; x++ ) {
                Assert.That(adapter.Function(x), Is.EqualTo(coeffs[x - left]).Within(1E-15));
            }
        }

    }

}
