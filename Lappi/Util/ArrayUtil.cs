using System;

namespace Lappi.Util {

    public static class ArrayUtil {

        public static void Fill<T> (this T[] v, T defaultValue) {
            for( int i = 0; i < v.Length; i++ ) {
                v[i] = defaultValue;
            }
        }

        public static void Fill<T> (this T[] v, Func<int, T> f) {
            for( int i = 0; i < v.Length; i++ ) {
                v[i] = f(i);
            }
        }

        public static T[] Add<T> (this T[] u, T[] v) {
            if( u.Length != v.Length ) {
                throw new ArgumentException();
            }
            T[] result = new T[u.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = (dynamic) u[i] + v[i];
            }
            return result;
        }

        public static T[] Sub<T> (this T[] u, T[] v) {
            if( u.Length != v.Length ) {
                throw new ArgumentException();
            }
            T[] result = new T[u.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = (dynamic) u[i] - v[i];
            }
            return result;
        }

    }

}
