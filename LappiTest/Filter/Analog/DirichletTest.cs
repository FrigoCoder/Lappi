using System;

using Lappi.Filter.Analog;

using NUnit.Framework;

namespace LappiTest.Filter.Analog {

    [TestFixture]
    public class DirichletTest {

        [TestCase]
        public void Dirichlet_same_as_naive_implementation () {
            AssertFiltersEqual(new Dirichlet(1), new DirichletNaive(1), 0.125);
            AssertFiltersEqual(new Dirichlet(2), new DirichletNaive(2), 0.125);
            AssertFiltersEqual(new Dirichlet(3), new DirichletNaive(3), 0.125);
            AssertFiltersEqual(new Dirichlet(4), new DirichletNaive(4), 0.125);
        }

        private void AssertFiltersEqual (AnalogFilter actual, AnalogFilter expected, double granularity) {
            Assert.That(actual.Left, Is.EqualTo(expected.Left));
            Assert.That(actual.Right, Is.EqualTo(expected.Right));
            Assert.That(actual.Radius, Is.EqualTo(expected.Radius));
            for( double x = actual.Left - granularity; x <= actual.Right + granularity; x += granularity ) {
                Assert.That(actual.Function(x), Is.EqualTo(expected.Function(x)).Within(1E-15));
            }
        }

        private class DirichletNaive : AnalogFilter, ResamplingFilter {

            private readonly int _n;
            public double Left => -_n;
            public double Right => _n;
            public double Radius => _n;

            public Func<double, double> Function => x => {
                if( Math.Abs(x) >= _n ) {
                    return 0;
                }

                double z = x / _n * Math.PI;

                double result = 0.5;
                for( int k = 1; k < _n; k++ ) {
                    result += Math.Cos(k * z);
                }
                result += 0.5 * Math.Cos(_n * z);
                return result / _n;
            };

            public DirichletNaive (int n) {
                _n = n;
            }

        }

    }

}
