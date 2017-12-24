namespace Lappi.Util {

    public static class Lang {

        public static T Sqr<T> (T x) => (dynamic) x * x;

        public static void Swap<T> (ref T x, ref T y) {
            T t = x;
            x = y;
            y = t;
        }

    }

}
