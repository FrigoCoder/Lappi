using System;

using Lappi;

using NUnit.Framework;

namespace LappiTest {

    public class ComplexTest {

        [Test]
        public void Constants () {
            Assert.That(Complex.Zero, Is.EqualTo(complex(0, 0)));
            Assert.That(Complex.One, Is.EqualTo(complex(1, 0)));
            Assert.That(Complex.I, Is.EqualTo(complex(0, 1)));
        }

        [Test]
        public void Unary_operators () {
            Assert.That(+complex(2, 3), Is.EqualTo(complex(2, 3)));
            Assert.That(-complex(2, 3), Is.EqualTo(complex(-2, -3)));

            Complex c = complex(2, 3);
            Assert.That(c++, Is.EqualTo(complex(2, 3)));
            Assert.That(++c, Is.EqualTo(complex(4, 3)));
            Assert.That(c--, Is.EqualTo(complex(4, 3)));
            Assert.That(--c, Is.EqualTo(complex(2, 3)));
        }

        [Test]
        public void Add () {
            Assert.That(complex(1, -1) + complex(-1, 1), Is.EqualTo(complex(0, 0)));
            Assert.That(complex(1, 2) + complex(3, 4), Is.EqualTo(complex(4, 6)));

            Assert.That(complex(1, -1) + 1, Is.EqualTo(complex(2, -1)));
            Assert.That(complex(-1, 0) + 1, Is.EqualTo(complex(0, 0)));

            Assert.That(1 + complex(2, 3), Is.EqualTo(complex(3, 3)));
            Assert.That(-1 + complex(0, 1), Is.EqualTo(complex(-1, 1)));
        }

        [Test]
        public void Sub () {
            Assert.That(complex(1, 0) - complex(0, 1), Is.EqualTo(complex(1, -1)));
            Assert.That(complex(1, 1) - complex(0, 1), Is.EqualTo(complex(1, 0)));
            Assert.That(complex(0, 1) - complex(0, 1), Is.EqualTo(complex(0, 0)));

            Assert.That(complex(1, 1) - 1, Is.EqualTo(complex(0, 1)));
            Assert.That(complex(16, 1) - 8.5, Is.EqualTo(complex(7.5, 1.0)));

            Assert.That(2 - complex(3, 4), Is.EqualTo(complex(-1, -4)));
            Assert.That(3 - complex(4, 5), Is.EqualTo(complex(-1, -5)));
        }

        [Test]
        public void Mul () {
            Assert.That(complex(1, 2) * complex(3, 4), Is.EqualTo(complex(-5, 10)));
            Assert.That(complex(2, 3) * complex(4, 5), Is.EqualTo(complex(-7, 22)));

            Assert.That(complex(1, 2) * 2, Is.EqualTo(complex(2, 4)));
            Assert.That(complex(2, 3) * 4, Is.EqualTo(complex(8, 12)));

            Assert.That(2 * complex(3, 4), Is.EqualTo(complex(6, 8)));
            Assert.That(3 * complex(3, 4), Is.EqualTo(complex(9, 12)));
        }

        [Test]
        public void Div () {
            Assert.That(complex(1, 2) / complex(1, 2), Is.EqualTo(complex(1, 0)));
            Assert.That(complex(2, 4) / complex(1, 2), Is.EqualTo(complex(2, 0)));
            Assert.That(complex(1, 2) / complex(3, 4), Is.EqualTo(complex(0.44, 0.08)));

            Assert.That(complex(1, 2) / 0.5, Is.EqualTo(complex(2, 4)));
            Assert.That(complex(3, 1) / 2, Is.EqualTo(complex(1.5, 0.5)));

            Assert.That(2 / complex(3, 4), Is.EqualTo(complex(0.24, -0.32)));
            Assert.That(2 / complex(4, 3), Is.EqualTo(complex(0.32, -0.24)));
        }

        [Test]
        public void Equality_operators () {
            Assert.That(Complex.One == complex(1, 0), Is.EqualTo(true));
            Assert.That(Complex.One == complex(0, 1), Is.EqualTo(false));
            Assert.That(Complex.I != complex(1, 0), Is.EqualTo(true));
            Assert.That(Complex.I != complex(0, 1), Is.EqualTo(false));
        }

        [Test]
        public void Cis () {
            Assert.That(Complex.Cis(0), Is.EqualTo(complex(1, 0)));
            Assert.That(Complex.Cis(Math.PI / 2), Is.EqualTo(complex(0, 1)));
            Assert.That(Complex.Cis(Math.PI), Is.EqualTo(complex(-1, 0)));
            Assert.That(Complex.Cis(3 * Math.PI / 2), Is.EqualTo(complex(0, -1)));
        }

        [Test]
        public void Abs () {
            Assert.That(complex(1, 0).Abs(), Is.EqualTo(1));
            Assert.That(complex(0, 1).Abs(), Is.EqualTo(1));
            Assert.That(complex(1, 1).Abs(), Is.EqualTo(Math.Sqrt(2)));
            Assert.That(complex(Math.Sqrt(2), -Math.Sqrt(2)).Abs(), Is.EqualTo(2));
        }

        [Test]
        public void AbsSqr () {
            Assert.That(complex(1, 1).AbsSqr(), Is.EqualTo(2));
            Assert.That(complex(3, 4).AbsSqr(), Is.EqualTo(25));
        }

        [Test]
        public void Arg () {
            Assert.That(Complex.Cis(0.1).Arg(), Is.EqualTo(0.1));
            Assert.That((Complex.Cis(0.1) * 2).Arg(), Is.EqualTo(0.1));
            Assert.That((Complex.Cis(0.1) * 01).Arg(), Is.EqualTo(0.1));
        }

        [Test]
        public void Conj () {
            Assert.That(complex(1, 2).Conj(), Is.EqualTo(complex(1, -2)));
            Assert.That(complex(3, 4).Conj(), Is.EqualTo(complex(3, -4)));
        }

        [Test]
        public void Equals () {
            Assert.That(Complex.One.Equals(complex(1, 0)), Is.EqualTo(true));
            Assert.That(Complex.One.Equals(complex(0, 1)), Is.EqualTo(false));
        }

        [Test]
        public void HashCode () {
            Assert.That(complex(1, 2).GetHashCode(), Is.EqualTo(31 * 1.0.GetHashCode() + 2.0.GetHashCode()));
            Assert.That(complex(3, 4).GetHashCode(), Is.EqualTo(31 * 3.0.GetHashCode() + 4.0.GetHashCode()));
        }

        [Test]
        public void Inv () {
            Assert.That(complex(2, 0).Inv(), Is.EqualTo(complex(0.5, 0)));
            Assert.That(complex(3, 4).Inv(), Is.EqualTo(complex(0.12, -0.16)));
        }

        [Test]
        public void Sqr () {
            Assert.That(complex(2, 1).Sqr(), Is.EqualTo(complex(3, 4)));
            Assert.That(complex(3, 4).Sqr(), Is.EqualTo(complex(-7, 24)));
        }

        private Complex complex (double re, double im) => new Complex(re, im);

    }

}
