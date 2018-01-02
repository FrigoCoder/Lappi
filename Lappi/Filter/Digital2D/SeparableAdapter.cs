using Lappi.Filter.Digital;
using Lappi.Util;

namespace Lappi.Filter.Digital2D {

    public class SeparableAdapter : DigitalFilter2D {

        public int Left { get; }
        public int Right { get; }
        public int Top { get; }
        public int Bottom { get; }
        public double[,] Coefficients { get; }

        public SeparableAdapter (DigitalFilter filter) {
            Left = filter.Left;
            Right = filter.Right;
            Top = filter.Left;
            Bottom = filter.Right;
            Coefficients = Arrays.New(Right - Left + 1, Bottom - Top + 1, (x, y) => filter[x + Left] * filter[y + Top]);
        }

        public double this [int x, int y] => Coefficients[x - Left, y - Top];

    }

}
