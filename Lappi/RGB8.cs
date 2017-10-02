using System.Drawing;

namespace Lappi {

    public struct RGB8 : ColorConvertible {

        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public RGB8 (byte R, byte G, byte B) {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public RGB8 (Color color) {
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public Color ToColor () {
            return Color.FromArgb(R, G, B);
        }

    }

}
