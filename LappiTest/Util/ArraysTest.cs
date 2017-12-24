using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    public class ArraysTest {

        [Test]
        public void New_value_type_test () {
            double[] v = Arrays.New<double>(10);
            Assert.That(v.Length, Is.EqualTo(10));
            foreach( double x in v ) {
                Assert.That(x, Is.EqualTo(0.0));
            }
        }

        [Test]
        public void New_reference_type_test () {
            object[] v = Arrays.New<object>(10);
            Assert.That(v.Length, Is.EqualTo(10));
            Assert.That(v[0], Is.TypeOf<object>());
            foreach( object t in v ) {
                Assert.That(t, Is.EqualTo(t));
            }
        }

        [Test]
        public void New_constant_test () {
            double[] v = Arrays.New(10, 1.23);
            Assert.That(v.Length, Is.EqualTo(10));
            foreach( double x in v ) {
                Assert.That(x, Is.EqualTo(1.23));
            }
        }

        [Test]
        public void New_lambda_test () {
            double[] v = Arrays.New<double>(10, i => i);
            Assert.That(v.Length, Is.EqualTo(10));
            for( int i = 0; i < v.Length; i++ ) {
                Assert.That(v[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void Fill_constant_test () {
            double[] v = new double[10];
            v.Fill(1.23);
            foreach( double x in v ) {
                Assert.That(x, Is.EqualTo(1.23));
            }
        }

        [Test]
        public void Fill_lambda_test () {
            double[] v = new double[10];
            v.Fill(i => i);
            for( int i = 0; i < v.Length; i++ ) {
                Assert.That(v[i], Is.EqualTo(i));
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
