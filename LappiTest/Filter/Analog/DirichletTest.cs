using System;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Analog {

    [TestFixture]
    public class DirichletTest {

        [TestCase]
        public void Dirichlet_downsampling_coefficients_are_appropriate () {
            AssertCoefficients(new Dirichlet(1), 2.0, new[] {0.5, 1.0, 0.5});
            AssertCoefficients(new Dirichlet(2), 2.0,
                new[] {-0.10355339059327377, 0, 0.60355339059327384, 1.0, 0.60355339059327384, 0, -0.10355339059327377});
            AssertCoefficients(new Dirichlet(3), 2.0,
                new[] {
                    0.044658198738520435, 0, -0.16666666666666666, 0, 0.62200846792814635, 1.0, 0.62200846792814635, 0, -0.16666666666666666, 0,
                    0.044658198738520435
                });
            AssertCoefficients(new Dirichlet(4), 2.0,
                new[] {
                    -0.024864045922457247, 0, 0.083522329739912374, 0, -0.18707572033318615, 0, 0.62841743651573101, 1.0, 0.62841743651573101, 0,
                    -0.18707572033318615, 0, 0.083522329739912374, 0, -0.024864045922457247
                });
        }

        [TestCase]
        public void Dirichlet_same_as_naive_implementation () {
            AssertFiltersEqual(new Dirichlet(1), new DirichletNaive(1), 0.125);
            AssertFiltersEqual(new Dirichlet(2), new DirichletNaive(2), 0.125);
            AssertFiltersEqual(new Dirichlet(3), new DirichletNaive(3), 0.125);
            AssertFiltersEqual(new Dirichlet(4), new DirichletNaive(4), 0.125);
        }

        private class DirichletNaive : AnalogFilter {

            public double Left => -n;
            public double Right => n;
            public double Radius => n;

            private readonly int n;

            public DirichletNaive (int n) {
                this.n = n;
            }

            public double this [double x] {
                get {
                    if( Math.Abs(x) >= n ) {
                        return 0;
                    }

                    double z = x / n * Math.PI;

                    double result = 0.5;
                    for( int k = 1; k < n; k++ ) {
                        result += Math.Cos(k * z);
                    }
                    result += 0.5 * Math.Cos(n * z);
                    return result / n;
                }
            }

        }

        private static void AssertCoefficients (AnalogFilter filter, double scale, double[] coeffs) {
            Assert.That(new Filter1D(filter, scale).Coefficients, Is.EqualTo(coeffs).Within(1E-15));
        }

        private static void AssertFiltersEqual (AnalogFilter actual, AnalogFilter expected, double granularity) {
            Assert.That(actual.Left, Is.EqualTo(expected.Left));
            Assert.That(actual.Right, Is.EqualTo(expected.Right));
            Assert.That(actual.Radius, Is.EqualTo(expected.Radius));
            for( double x = actual.Left - granularity; x <= actual.Right + granularity; x += granularity ) {
                Assert.That(actual[x], Is.EqualTo(expected[x]).Within(1E-15));
            }
        }

    }

}
