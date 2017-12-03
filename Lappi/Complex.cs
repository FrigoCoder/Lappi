using System;
using System.Diagnostics.CodeAnalysis;

namespace Lappi {

    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public struct Complex {

        public static Complex Zero = new Complex(0);
        public static Complex One = new Complex(1);
        public static Complex I = new Complex(0, 1);

        public static Complex operator + (Complex x) => x;
        public static Complex operator - (Complex x) => new Complex(-x.Re, -x.Im);
        public static Complex operator ++ (Complex x) => new Complex(x.Re + 1, x.Im);
        public static Complex operator -- (Complex x) => new Complex(x.Re - 1, x.Im);

        public static Complex operator + (Complex x, Complex y) => new Complex(x.Re + y.Re, x.Im + y.Im);
        public static Complex operator + (Complex x, double y) => new Complex(x.Re + y, x.Im);
        public static Complex operator + (double x, Complex y) => new Complex(x + y.Re, y.Im);

        public static Complex operator - (Complex x, Complex y) => new Complex(x.Re - y.Re, x.Im - y.Im);
        public static Complex operator - (Complex x, double y) => new Complex(x.Re - y, x.Im);
        public static Complex operator - (double x, Complex y) => new Complex(x - y.Re, -y.Im);

        public static Complex operator * (Complex x, Complex y) {
            double t = (x.Re - x.Im) * y.Im;
            return new Complex(x.Re * (y.Re - y.Im) + t, x.Im * (y.Re + y.Im) + t);
        }

        public static Complex operator * (Complex x, double y) => new Complex(x.Re * y, x.Im * y);
        public static Complex operator * (double x, Complex y) => new Complex(x * y.Re, x * y.Im);

        public static Complex operator / (Complex x, Complex y) => x * y.Inv();
        public static Complex operator / (Complex x, double y) => new Complex(x.Re / y, x.Im / y);
        public static Complex operator / (double x, Complex y) => x * y.Inv();

        public static bool operator == (Complex x, Complex y) => x.Re == y.Re && x.Im == y.Im;
        public static bool operator != (Complex x, Complex y) => x.Re != y.Re || x.Im != y.Im;

        public static Complex Cis (double x) => new Complex(Math.Cos(x), Math.Sin(x));

        public readonly double Re;
        public readonly double Im;

        public Complex (double re = 0, double im = 0) {
            Re = re;
            Im = im;
        }

        public double Abs () => Math.Sqrt(AbsSqr());
        public double AbsSqr () => Re * Re + Im * Im;
        public double Arg () => Math.Atan2(Im, Re);
        public Complex Conj () => new Complex(Re, -Im);
        public override bool Equals (object obj) => obj is Complex && Re == ((Complex) obj).Re && Im == ((Complex) obj).Im;
        public override int GetHashCode () => 31 * Re.GetHashCode() + Im.GetHashCode();
        public Complex Inv () => Conj() / AbsSqr();
        public Complex Sqr () => new Complex(Re * Re - Im * Im, 2 * Re * Im);
        public override string ToString () => Re.ToString("R") + "+" + Im.ToString("R") + "i";

    }

}
