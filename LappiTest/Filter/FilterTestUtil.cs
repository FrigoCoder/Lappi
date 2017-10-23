using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter {

    public static class FilterTestUtil {

        public static void AssertCoefficients (AnalogFilter filter, double scale, double[] coeffs) {
            AssertCoefficients(new DigitalAdapter(filter, scale), coeffs);
        }

        public static void AssertCoefficients (DigitalFilter filter, double[] coeffs) {
            double[] actual = new double[filter.Right - filter.Left + 1];
            for( int x = filter.Left; x <= filter.Right; x++ ) {
                actual[x - filter.Left] = filter.Kernel(x);
            }
            Assert.That(actual, Is.EqualTo(coeffs).Within(1E-15));
        }

        public static void AssertFiltersEqual (AnalogFilter actual, AnalogFilter expected, double granularity) {
            Assert.That(actual.Left, Is.EqualTo(expected.Left));
            Assert.That(actual.Right, Is.EqualTo(expected.Right));
            Assert.That(actual.Radius, Is.EqualTo(expected.Radius));
            for( double x = actual.Left - granularity; x <= actual.Right + granularity; x += granularity ) {
                Assert.That(actual.Kernel(x), Is.EqualTo(expected.Kernel(x)).Within(1E-15));
            }
        }

    }

}
