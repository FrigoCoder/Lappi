using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    [TestFixture]
    public class IntegralArrayTest {

        [TestCase]
        public void Sum_works_perfectly () {
            double[] array = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            IntegralArray<double> integral = new IntegralArray<double>(array);
            Assert.That(integral.Sum(1, 5), Is.EqualTo(15));
            Assert.That(integral.Sum(0, 9), Is.EqualTo(45));
        }

        [TestCase]
        public void Sum_returns_0_for_empty_intervals () {
            double[] array = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            IntegralArray<double> integral = new IntegralArray<double>(array);
            for( int i = 0; i < array.Length; i++ ) {
                for( int j = 0; j < i; j++ ) {
                    Assert.That(integral.Sum(i, j), Is.EqualTo(0));
                }
            }
        }

    }

}
