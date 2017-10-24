using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class IntegralArrayTest {


        [TestCase]
        public void Sum_works_perfectly () {
            double[] array = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            IntegralArray<double> integral = new IntegralArray<double>(array);
            Assert.That(integral.Sum(1, 5), Is.EqualTo(15));
            Assert.That(integral.Sum(0, 9), Is.EqualTo(45));
        }

    }

}
