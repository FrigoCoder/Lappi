using System.Drawing;

namespace Lappi.Image {

    public struct Rgb8 {

        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public Rgb8 (byte r, byte g, byte b) {
            R = r;
            G = g;
            B = b;
        }

        public Rgb8 (Color color) {
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public Color ToColor () => Color.FromArgb(R, G, B);

    }

}
