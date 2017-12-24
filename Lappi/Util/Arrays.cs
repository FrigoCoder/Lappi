using System;

namespace Lappi.Util {

    public static class Arrays {

        public static T[] New<T> (int length) where T : new() => default(T) == null ? New(length, new T()) : new T[length];

        public static T[] New<T> (int length, T defaultValue) {
            T[] v = new T[length];
            for( int i = 0; i < v.Length; i++ ) {
                v[i] = defaultValue;
            }
            return v;
        }

        public static T[] New<T> (int length, Func<int, T> f) {
            T[] result = new T[length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = f(i);
            }
            return result;
        }

        public static T[] New<T> (int length, Func<T[], int, T> f) {
            T[] v = new T[length];
            for( int i = 0; i < v.Length; i++ ) {
                v[i] = f(v, i);
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

    }

}
