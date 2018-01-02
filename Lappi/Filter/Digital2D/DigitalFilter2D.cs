using Lappi.Filter.Digital;
using Lappi.Util;

namespace Lappi.Filter.Digital2D {

    public class DigitalFilter2D {

        public readonly int Left;
        public readonly int Right;
        public readonly int Top;
        public readonly int Bottom;
        public readonly double[,] Coefficients;

        public DigitalFilter2D (int centerX, int centerY, double[,] coefficients) {
            Left = 0 - centerX;
            Right = coefficients.GetLength(0) - 1 - centerX;
            Top = 0 - centerY;
            Bottom = coefficients.GetLength(1) - 1 - centerY;
            Coefficients = coefficients;
        }

        public DigitalFilter2D (DigitalFilter filter) {
            Left = filter.Left;
            Right = filter.Right;
            Top = filter.Left;
            Bottom = filter.Right;
            Coefficients = Arrays.New(Right - Left + 1, Bottom - Top + 1, (x, y) => filter[x + Left] * filter[y + Top]);
        }

        public double this [int x, int y] => Coefficients[x - Left, y - Top];

    }

}
