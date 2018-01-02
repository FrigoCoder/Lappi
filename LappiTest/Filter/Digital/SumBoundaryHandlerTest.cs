using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    public class SumBoundaryHandlerTest {

        [Test]
        public void Linear_filter_3_tap_in_5_spaces () {
            Filter1D filter = new Filter1D(1, new[] {0.5, 1.0, 0.5});
            SumBoundaryHandler filters = new SumBoundaryHandler(filter);
            AssertFilter(filters.GetFilter(0, 5), 0, 1, new[] {2 / 3d, 1 / 3d});
            AssertFilter(filters.GetFilter(1, 5), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(2, 5), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(3, 5), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(4, 5), -1, 0, new[] {1 / 3d, 2 / 3d});
        }

        [Test]
        public void Linear_filter_5_tap_in_7_spaces () {
            Filter1D filter = new Filter1D(2, new[] {1 / 3d, 2 / 3d, 3 / 3d, 2 / 3d, 1 / 3d});
            SumBoundaryHandler filters = new SumBoundaryHandler(filter);
            AssertFilter(filters.GetFilter(0, 7), 0, 2, new[] {3 / 6d, 2 / 6d, 1 / 6d});
            AssertFilter(filters.GetFilter(1, 7), -1, 2, new[] {2 / 8d, 3 / 8d, 2 / 8d, 1 / 8d});
            AssertFilter(filters.GetFilter(2, 7), -2, 2, new[] {1 / 9d, 2 / 9d, 3 / 9d, 2 / 9d, 1 / 9d});
            AssertFilter(filters.GetFilter(3, 7), -2, 2, new[] {1 / 9d, 2 / 9d, 3 / 9d, 2 / 9d, 1 / 9d});
            AssertFilter(filters.GetFilter(4, 7), -2, 2, new[] {1 / 9d, 2 / 9d, 3 / 9d, 2 / 9d, 1 / 9d});
            AssertFilter(filters.GetFilter(5, 7), -2, 1, new[] {1 / 8d, 2 / 8d, 3 / 8d, 2 / 8d});
            AssertFilter(filters.GetFilter(6, 7), -2, 0, new[] {1 / 6d, 2 / 6d, 3 / 6d});
        }

        private void AssertFilter (Filter1D filter, int left, int right, double[] coefficients) {
            Assert.That(filter.Left, Is.EqualTo(left));
            Assert.That(filter.Right, Is.EqualTo(right));
            Assert.That(filter.Coefficients, Is.EqualTo(coefficients).Within(1E-15));
        }

    }

}
