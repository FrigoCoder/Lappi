using System;
using System.Drawing;
using System.Linq;

namespace Lappi.Image {

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

        public static bool operator == (Image<T> image1, Image<T> image2) =>
            ReferenceEquals(image1, null) ? ReferenceEquals(image2, null) : image1.Equals(image2);

        public static bool operator != (Image<T> image1, Image<T> image2) =>
            ReferenceEquals(image1, null) ? !ReferenceEquals(image2, null) : !image1.Equals(image2);

        public static Image<T> operator + (Image<T> image1, Image<T> image2) {
            if( image1.Xs != image2.Xs || image1.Ys != image2.Ys ) {
                throw new ArgumentException();
            }
            Image<T> result = new Image<T>(image1.Xs, image2.Ys);
            for( int x = 0; x < result.Xs; x++ ) {
                for( int y = 0; y < result.Ys; y++ ) {
                    result[x, y] = (dynamic) image1[x, y] + image2[x, y];
                }
            }
            return result;
        }

        public static Image<T> operator - (Image<T> image1, Image<T> image2) {
            if( image1.Xs != image2.Xs || image1.Ys != image2.Ys ) {
                throw new ArgumentException();
            }
            Image<T> result = new Image<T>(image1.Xs, image2.Ys);
            for( int x = 0; x < result.Xs; x++ ) {
                for( int y = 0; y < result.Ys; y++ ) {
                    result[x, y] = (dynamic) image1[x, y] - image2[x, y];
                }
            }
            return result;
        }

        public readonly int Xs;
        public readonly int Ys;
        public readonly RowIndexer<T> Rows;
        public readonly ColumnIndexer<T> Columns;
        private readonly T[,] pixels;

        public Image (int xs, int ys) : this(new T[ys, xs]) {
        }

        public Image (int xs, int ys, T defaultValue) : this(new T[ys, xs]) {
            for( int x = 0; x < xs; x++ ) {
                for( int y = 0; y < ys; y++ ) {
                    this[x, y] = defaultValue;
                }
            }
        }

        public Image (T[,] pixels) {
            Xs = pixels.GetLength(1);
            Ys = pixels.GetLength(0);
            Rows = new RowIndexer<T>(this);
            Columns = new ColumnIndexer<T>(this);
            this.pixels = pixels;
        }

        public void Save (string filename) {
            using( Bitmap bitmap = new Bitmap(Xs, Ys) ) {
                for( int x = 0; x < Xs; x++ ) {
                    for( int y = 0; y < Ys; y++ ) {
                        bitmap.SetPixel(x, y, ((dynamic) this[x, y]).ToColor());
                    }
                }
                bitmap.Save(filename);
            }
        }

        public override string ToString () {
            return "{" + string.Join(", ", Enumerable.Range(0, Ys).Select(y => "{" + string.Join(", ", Rows[y]) + "}")) + "}";
        }

        public override bool Equals (object obj) {
            Image<T> that = obj as Image<T>;
            if( ReferenceEquals(that, null) || that.Xs != Xs || that.Ys != Ys ) {
                return false;
            }
            for( int x = 0; x < Xs; x++ ) {
                for( int y = 0; y < Ys; y++ ) {
                    if( !Equals(that[x, y], this[x, y]) ) {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode () => throw new NotSupportedException();

        public T this [int x, int y] {
            get => pixels[y, x];
            set => pixels[y, x] = value;
        }

    }

    public class RowIndexer<T> {

        private readonly Image<T> image;

        public RowIndexer (Image<T> image) {
            this.image = image;
        }

        public T[] this [int y] {
            get {
                T[] result = new T[image.Xs];
                for( int x = 0; x < result.Length; x++ ) {
                    result[x] = image[x, y];
                }
                return result;
            }
            set {
                if( value.Length != image.Xs ) {
                    throw new ArgumentException();
                }
                for( int x = 0; x < image.Xs; x++ ) {
                    image[x, y] = value[x];
                }
            }
        }

    }

    public class ColumnIndexer<T> {

        private readonly Image<T> image;

        public ColumnIndexer (Image<T> image) {
            this.image = image;
        }

        public T[] this [int x] {
            get {
                T[] result = new T[image.Ys];
                for( int y = 0; y < result.Length; y++ ) {
                    result[y] = image[x, y];
                }
                return result;
            }
            set {
                if( value.Length != image.Ys ) {
                    throw new ArgumentException();
                }
                for( int y = 0; y < image.Ys; y++ ) {
                    image[x, y] = value[y];
                }
            }
        }

    }

}
