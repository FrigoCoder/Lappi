using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    public class ArraysTest {

        [Test]
        public void New_value_type () {
            double[] v = Arrays.New(10, new double());
            Assert.That(v.Length, Is.EqualTo(10));
            foreach( double x in v ) {
                Assert.That(x, Is.EqualTo(0.0));
            }
        }

        [Test]
        public void New_reference_type () {
            object[] v = Arrays.New(10, new object());
            Assert.That(v.Length, Is.EqualTo(10));
            Assert.That(v[0], Is.TypeOf<object>());
            foreach( object t in v ) {
                Assert.That(t, Is.SameAs(v[0]));
            }
        }

        [Test]
        public void New_lambda () {
            double[] v = Arrays.New<double>(10, i => i);
            Assert.That(v.Length, Is.EqualTo(10));
            for( int i = 0; i < v.Length; i++ ) {
                Assert.That(v[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void New_lambda_with_array () {
            double[] v = Arrays.New<double>(10, (i, u) => i == 0 ? 0 : u[i - 1] + 1);
            Assert.That(v.Length, Is.EqualTo(10));
            for( int i = 0; i < v.Length; i++ ) {
                Assert.That(v[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void New_2d_value_type () {
            double[,] v = Arrays.New(10, 11, new double());
            Assert.That(v.GetLength(0), Is.EqualTo(10));
            Assert.That(v.GetLength(1), Is.EqualTo(11));
            for( int i = 0; i < v.GetLength(0); i++ ) {
                for( int j = 0; j < v.GetLength(1); j++ ) {
                    Assert.That(v[i, j], Is.EqualTo(0.0));
                }
            }
        }

        [Test]
        public void New_2d_reference_type () {
            object[,] v = Arrays.New(10, 11, new object());
            Assert.That(v.GetLength(0), Is.EqualTo(10));
            Assert.That(v.GetLength(1), Is.EqualTo(11));
            Assert.That(v[0, 0], Is.TypeOf<object>());
            for( int i = 0; i < v.GetLength(0); i++ ) {
                for( int j = 0; j < v.GetLength(1); j++ ) {
                    Assert.That(v[i, j], Is.SameAs(v[0, 0]));
                }
            }
        }

        [Test]
        public void New_2d_lambda () {
            double[,] v = Arrays.New<double>(10, 11, (i, j) => i + 10 * j);
            Assert.That(v.GetLength(0), Is.EqualTo(10));
            Assert.That(v.GetLength(1), Is.EqualTo(11));
            for( int i = 0; i < v.GetLength(0); i++ ) {
                for( int j = 0; j < v.GetLength(1); j++ ) {
                    Assert.That(v[i, j], Is.EqualTo(i + 10 * j));
                }
            }
        }

        [Test]
        public void New_2d_lambda_with_array () {
            double[,] v = Arrays.New<double>(10, 11, (i, j, u) => i == 0 ? j : u[i - 1, j] + 1);
            Assert.That(v.GetLength(0), Is.EqualTo(10));
            Assert.That(v.GetLength(1), Is.EqualTo(11));
            for( int i = 0; i < v.GetLength(0); i++ ) {
                for( int j = 0; j < v.GetLength(1); j++ ) {
                    Assert.That(v[i, j], Is.EqualTo(i + j));
                }
            }
        }

        [Test]
        public void Add () {
            double[] u = {1, 2, 3, 4, 5};
            double[] v = {3, 3, 3, 3, 3};
            double[] expected = {4, 5, 6, 7, 8};
            Assert.That(u.Add(v), Is.EqualTo(expected));
        }

        [Test]
        public void Sub () {
            double[] u = {1, 2, 3, 4, 5};
            double[] v = {3, 3, 3, 3, 3};
            double[] expected = {-2, -1, 0, 1, 2};
            Assert.That(u.Sub(v), Is.EqualTo(expected));
        }

        [Test]
        public void Foreach () {
            double[] v = {1, 2, 3, 4, 5};
            double sum = 0;
            v.Foreach(x => sum += x);
            Assert.That(sum, Is.EqualTo(15));
        }

        [Test]
        public void Foreach_with_index () {
            double[] v = {1, 2, 3, 4, 5};
            double sum = 0;
            v.Foreach((x, i) => sum += x * v[i]);
            Assert.That(sum, Is.EqualTo(55));
        }

        [Test]
        public void Foreach_with_index_and_array () {
            double[] v = {1, 2, 3, 4, 5};
            double sum = 0;
            v.Foreach((x, i, u) => sum += x * u[i]);
            Assert.That(sum, Is.EqualTo(55));
        }

        [Test]
        public void Foreach_2d () {
            double[,] v = {{1, 2, 3}, {4, 5, 6}};
            double sum = 0;
            v.Foreach(x => sum += x);
            Assert.That(sum, Is.EqualTo(21));
        }

        [Test]
        public void Foreach_2d_with_index () {
            double[,] v = {{1, 2, 3}, {4, 5, 6}};
            double sum = 0;
            v.Foreach((x, i, j) => sum += x * v[i, j]);
            Assert.That(sum, Is.EqualTo(91));
        }

        [Test]
        public void Foreach_2d_with_index_and_array () {
            double[,] v = {{1, 2, 3}, {4, 5, 6}};
            double sum = 0;
            v.Foreach((x, i, j, u) => sum += x * u[i, j]);
            Assert.That(sum, Is.EqualTo(91));
        }

    }

}
