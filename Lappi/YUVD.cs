using System;
using System.Drawing;

namespace Lappi {

    /// <summary>
    ///     Based on https://en.wikipedia.org/wiki/YUV#SDTV_with_BT.601
    /// </summary>
    public class YUVD : Colorspace {

        public readonly double Y;
        public readonly double U;
        public readonly double V;

        public YUVD (double y, double u, double v) {
            Y = y;
            U = u;
            V = v;
        }

        public YUVD (Color color) {
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

        private const double Wr = 0.299;
        private const double Wg = 1.000 - Wr - Wb;
        private const double Wb = 0.114;

        private const double Umax = 0.436;
        private const double Vmax = 0.615;

        private static readonly double[,] F ={{Wr, Wg, Wb}, {-Umax * Wr / (Wr + Wg), -Umax * Wg / (Wr + Wg), Umax}, {Vmax, -Vmax * Wg / (Wg + Wb), -Vmax * Wb / (Wg + Wb)}};

        private static readonly double[,] I =
            {{1, 0, (Wg + Wb) / Vmax}, {1, Wb * (Wb - 1) / Wg / Umax, Wr * (Wr - 1) / Wg / Vmax}, {1, (Wr + Wg) / Umax, 0}};

        private static byte ToByte (double x) {
            int rounded = (int) Math.Round(x * 255.0);
            int clamped = Math.Max(0, Math.Min(rounded, 255));
            return (byte) clamped;
        }

    }

}
