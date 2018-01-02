using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    public class DirichletBoundaryHandlerTest {

        [Test]
        public void Dirichlet_2_in_11_spaces () {
            DirichletBoundaryHandler filters = new DirichletBoundaryHandler(2);
            AssertFilter(filters.GetFilter(0, 11), 0, 1, new[] {0.5, 0.5});
            AssertFilter(filters.GetFilter(1, 11), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(2, 11), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(3, 11), -3, 3,
                new[] {-0.051776695296636886, 0, 0.30177669529663687, 0.5, 0.30177669529663687, 0, -0.051776695296636886});
            AssertFilter(filters.GetFilter(4, 11), -3, 3,
                new[] {-0.051776695296636886, 0, 0.30177669529663687, 0.5, 0.30177669529663687, 0, -0.051776695296636886});
            AssertFilter(filters.GetFilter(5, 11), -3, 3,
                new[] {-0.051776695296636886, 0, 0.30177669529663687, 0.5, 0.30177669529663687, 0, -0.051776695296636886});
            AssertFilter(filters.GetFilter(6, 11), -3, 3,
                new[] {-0.051776695296636886, 0, 0.30177669529663687, 0.5, 0.30177669529663687, 0, -0.051776695296636886});
            AssertFilter(filters.GetFilter(7, 11), -3, 3,
                new[] {-0.051776695296636886, 0, 0.30177669529663687, 0.5, 0.30177669529663687, 0, -0.051776695296636886});
            AssertFilter(filters.GetFilter(8, 11), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(9, 11), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(10, 11), -1, 0, new[] {0.5, 0.5});
        }

        [Test]
        public void Dirichlet_3_in_11_spaces () {
            DirichletBoundaryHandler filters = new DirichletBoundaryHandler(3);
            AssertFilter(filters.GetFilter(0, 11), 0, 1, new[] {0.5, 0.5});
            AssertFilter(filters.GetFilter(1, 11), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(2, 11), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(3, 11), -3, 3,
                new[] {-0.051776695296636886, 0, 0.30177669529663687, 0.5, 0.30177669529663687, 0, -0.051776695296636886});
            AssertFilter(filters.GetFilter(4, 11), -3, 3,
                new[] {-0.051776695296636886, 0, 0.30177669529663687, 0.5, 0.30177669529663687, 0, -0.051776695296636886});
            AssertFilter(filters.GetFilter(5, 11), -5, 5,
                new[] {
                    0.022329099369260221, 0, -0.083333333333333343, 0, 0.31100423396407323, 0.5, 0.31100423396407323, 0, -0.083333333333333343, 0,
                    0.022329099369260221
                });
            AssertFilter(filters.GetFilter(6, 11), -3, 3,
                new[] {-0.051776695296636886, 0, 0.30177669529663687, 0.5, 0.30177669529663687, 0, -0.051776695296636886});
            AssertFilter(filters.GetFilter(7, 11), -3, 3,
                new[] {-0.051776695296636886, 0, 0.30177669529663687, 0.5, 0.30177669529663687, 0, -0.051776695296636886});
            AssertFilter(filters.GetFilter(8, 11), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(9, 11), -1, 1, new[] {0.25, 0.5, 0.25});
            AssertFilter(filters.GetFilter(10, 11), -1, 0, new[] {0.5, 0.5});
        }

        private void AssertFilter (DigitalFilter filter, int left, int right, double[] coefficients) {
            Assert.That(filter.Left, Is.EqualTo(left));
            Assert.That(filter.Right, Is.EqualTo(right));
            Assert.That(filter.Coefficients, Is.EqualTo(coefficients).Within(1E-15));
        }

    }

}
