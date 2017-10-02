using System;
using System.Drawing;

namespace Lappi {

    public class Image<T> where T : ColorConvertible {

        public static Image<T> Load (string filename) {
            using( Bitmap bitmap = new Bitmap(filename) ) {
                Image<T> image = new Image<T>(bitmap.Width, bitmap.Height);
                for( int x = 0; x < image.xs; x++ ) {
                    for( int y = 0; y < image.ys; y++ ) {
                        Color color = bitmap.GetPixel(x, y);
                        image[x, y] = (T) Activator.CreateInstance(typeof(T), color);
                    }
                }
                return image;
            }
        }

        public readonly int xs;
        public readonly int ys;
        private readonly T[,] pixels;

        public Image (int xs, int ys) {
            this.xs = xs;
            this.ys = ys;
            pixels = new T[xs, ys];
        }

        public void Save (string filename) {
            using( Bitmap bitmap = new Bitmap(xs, ys) ) {
                for( int x = 0; x < xs; x++ ) {
                    for( int y = 0; y < ys; y++ ) {
                        bitmap.SetPixel(x, y, this[x, y].ToColor());
                    }
                }
                bitmap.Save(filename);
            }
        }

        public T this [int x, int y] {
            get => pixels[x, y];
            set => pixels[x, y] = value;
        }

    }

}
