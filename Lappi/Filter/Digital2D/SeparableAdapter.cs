using Lappi.Filter.Digital;

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
            Coefficients = new double[Right - Left + 1, Bottom - Top + 1];
            for( int x = Left; x <= Right; x++ ) {
                for( int y = Top; y <= Bottom; y++ ) {
                    this[x, y] = filter[x] * filter[y];
                }
            }
        }

        public double this [int x, int y] {
            get => Coefficients[x - Left, y - Top];
            private set => Coefficients[x - Left, y - Top] = value;
        }

    }

}
