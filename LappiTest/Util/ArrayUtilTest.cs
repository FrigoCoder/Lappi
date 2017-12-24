using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    public class ArrayUtilTest {

        [Test]
        public void Fill_test () {
            double[] v = new double[10];
            v.Fill(1.23);
            foreach( double x in v ) {
                Assert.That(x, Is.EqualTo(1.23));
            }
        }

        [Test]
        public void Add_test () {
            double[] u = {1, 2, 3, 4, 5};
            double[] v = {3, 3, 3, 3, 3};
            double[] expected = {4, 5, 6, 7, 8};
            Assert.That(u.Add(v), Is.EqualTo(expected));
        }

        [Test]
        public void Sub_test () {
            double[] u = {1, 2, 3, 4, 5};
            double[] v = {3, 3, 3, 3, 3};
            double[] expected = {-2, -1, 0, 1, 2};
            Assert.That(u.Sub(v), Is.EqualTo(expected));
        }

    }

}
