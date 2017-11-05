using System;
using System.Drawing;

namespace Lappi {

    public class Image<T> {

        public static Image<T> Load (string filename) {
            using( Bitmap bitmap = new Bitmap(filename) ) {
                Image<T> image = new Image<T>(bitmap.Width, bitmap.Height);
                for( int x = 0; x < image.Xs; x++ ) {
                    for( int y = 0; y < image.Ys; y++ ) {
                        Color color = bitmap.GetPixel(x, y);
                        image[x, y] = (T) Activator.CreateInstance(typeof(T), color);
                    }
                }
                return image;
            }
        }

        public readonly int Xs;
        public readonly int Ys;
        private readonly T[,] pixels;

        public Image (int xs, int ys) {
            Xs = xs;
            Ys = ys;
            pixels = new T[ys, xs];
        }

        public Image (T[,] pixels) {
            Xs = pixels.GetLength(1);
            Ys = pixels.GetLength(0);
            this.pixels = pixels;
        }

        public void Save (string filename) {
            if( !typeof(Colorspace).IsAssignableFrom(typeof(T)) ) {
                throw new ArgumentException();
            }
            using( Bitmap bitmap = new Bitmap(Xs, Ys) ) {
                for( int x = 0; x < Xs; x++ ) {
                    for( int y = 0; y < Ys; y++ ) {
                        Colorspace color = (dynamic) this[x, y];
                        bitmap.SetPixel(x, y, color.ToColor());
                    }
                }
                bitmap.Save(filename);
            }
        }

        public T this [int x, int y] {
            get => pixels[y, x];
            set => pixels[y, x] = value;
        }

    }

}
