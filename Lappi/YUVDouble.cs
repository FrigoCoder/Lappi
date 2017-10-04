using System;
using System.Drawing;

namespace Lappi {

    public class YUVDouble : Colorspace {

        public readonly double Y;
        public readonly double U;
        public readonly double V;

        public YUVDouble (double Y, double U, double V) {
            this.Y = Y;
            this.U = U;
            this.V = V;
        }

        public YUVDouble (Color color) {
            Y = (F[0, 0] * color.R + F[0, 1] * color.G + F[0, 2] * color.B) / 255;
            U = (F[1, 0] * color.R + F[1, 1] * color.G + F[1, 2] * color.B) / 255;
            V = (F[2, 0] * color.R + F[2, 1] * color.G + F[2, 2] * color.B) / 255;
        }

        public Color ToColor () {
            double r = I[0, 0] * Y + I[0, 1] * U + I[0, 2] * V;
            double g = I[1, 0] * Y + I[1, 1] * U + I[1, 2] * V;
            double b = I[2, 0] * Y + I[2, 1] * U + I[2, 2] * V;
            return Color.FromArgb(ToByte(r), ToByte(g), ToByte(b));
        }

        public override string ToString () {
            return "YUVDouble(" + Y + "," + U + "," + V + ")";
        }

        private const double WR = 0.299;
        private const double WG = 1.000 - WR - WB;
        private const double WB = 0.114;

        private const double Umax = 0.436;
        private const double Vmax = 0.615;

        private static readonly double[,] F = {
            {WR, WG, WB},
            {-Umax * WR / (WR + WG), -Umax * WG / (WR + WG), Umax},
            {Vmax, -Vmax * WG / (WG + WB), -Vmax * WB / (WG + WB)}
        };

        private static readonly double[,] I = {
            {1, 0, (WG + WB) / Vmax},
            {1, WB * (WB - 1) / WG / Umax, WR * (WR - 1) / WG / Vmax},
            {1, (WR + WG) / Umax, 0}
        };

        private static byte ToByte (double x) {
            int rounded = (int) Math.Round(x * 255.0);
            int clamped = Math.Max(0, Math.Min(rounded, 255));
            return (byte) clamped;
        }

    }

}
