using System;

namespace Lappi.Util {

    public static class Arrays {

        public static T[] New<T> (int length) where T : new() {
            T[] v = new T[length];
            if( default(T) == null ) {
                T x = new T();
                for( int i = 0; i < v.Length; i++ ) {
                    v[i] = x;
                }
            }
            return v;
        }

        public static T[] New<T> (int length, Func<int, T> f) {
            T[] v = new T[length];
            for( int i = 0; i < v.Length; i++ ) {
                v[i] = f(i);
            }
            return v;
        }

        public static T[] New<T> (int length, Func<int, T[], T> f) {
            T[] v = new T[length];
            for( int i = 0; i < v.Length; i++ ) {
                v[i] = f(i, v);
            }
            return v;
        }

        public static T[,] New<T> (int length1, int length2) where T : new() {
            T[,] v = new T[length1, length2];
            if( default(T) == null ) {
                T x = new T();
                for( int i = 0; i < v.GetLength(0); i++ ) {
                    for( int j = 0; j < v.GetLength(1); j++ ) {
                        v[i, j] = x;
                    }
                }
            }
            return v;
        }

        public static T[,] New<T> (int length1, int length2, Func<int, int, T> f) {
            T[,] v = new T[length1, length2];
            for( int i = 0; i < v.GetLength(0); i++ ) {
                for( int j = 0; j < v.GetLength(1); j++ ) {
                    v[i, j] = f(i, j);
                }
            }
            return v;
        }

        public static T[,] New<T> (int length1, int length2, Func<int, int, T[,], T> f) {
            T[,] v = new T[length1, length2];
            for( int i = 0; i < v.GetLength(0); i++ ) {
                for( int j = 0; j < v.GetLength(1); j++ ) {
                    v[i, j] = f(i, j, v);
                }
            }
            return v;
        }

        public static T[] Add<T> (this T[] u, T[] v) {
            if( u.Length != v.Length ) {
                throw new ArgumentException();
            }
            return New<T>(u.Length, i => (dynamic) u[i] + v[i]);
        }

        public static T[] Sub<T> (this T[] u, T[] v) {
            if( u.Length != v.Length ) {
                throw new ArgumentException();
            }
            return New<T>(u.Length, i => (dynamic) u[i] - v[i]);
        }

        public static void Foreach<T> (this T[] v, Action<T> action) {
            foreach( T item in v ) {
                action(item);
            }
        }

        public static void Foreach<T> (this T[] v, Action<T, int> action) {
            for( int i = 0; i < v.Length; i++ ) {
                action(v[i], i);
            }
        }

        public static void Foreach<T> (this T[] v, Action<T, int, T[]> action) {
            for( int i = 0; i < v.Length; i++ ) {
                action(v[i], i, v);
            }
        }

    }

}
