using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter {

    public static class FilterTestUtil {

        public static void AssertCoefficients (AnalogFilter analog, double scale, double[] coeffs) {
            AssertCoefficients(new DigitalAdapter(analog, scale), coeffs);
        }

        public static void AssertCoefficients (DigitalFilter adapter, double[] coeffs) {
            double[] actual = new double[adapter.Right - adapter.Left + 1];
            for( int x = adapter.Left; x <= adapter.Right; x++ ) {
                actual[x - adapter.Left] = adapter.Function(x);
            }
            Assert.That(actual, Is.EqualTo(coeffs).Within(1E-15));
        }

        public static void AssertFiltersEqual (AnalogFilter actual, AnalogFilter expected, double granularity) {
            Assert.That(actual.Left, Is.EqualTo(expected.Left));
            Assert.That(actual.Right, Is.EqualTo(expected.Right));
            Assert.That(actual.Radius, Is.EqualTo(expected.Radius));
            for( double x = actual.Left - granularity; x <= actual.Right + granularity; x += granularity ) {
                Assert.That(actual.Function(x), Is.EqualTo(expected.Function(x)).Within(1E-15));
            }
        }

    }

}
