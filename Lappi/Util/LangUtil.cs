namespace Lappi.Util {

    public static class LangUtil {

        public static void Swap<T> (ref T x, ref T y) {
            T t = x;
            x = y;
            y = t;
        }

    }

}
