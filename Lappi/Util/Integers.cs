namespace Lappi.Util {

    public static class Integers {

        public static uint ToUnsigned (this int i) => unchecked((uint) i);

        public static int ToSigned (this uint i) => unchecked((int) i);

        public static int Reverse (this int i) => i.ToUnsigned().Reverse().ToSigned();

        public static uint Reverse (this uint i) {
            i = ((i & 0x55555555) << 1) | ((i >> 1) & 0x55555555);
            i = ((i & 0x33333333) << 2) | ((i >> 2) & 0x33333333);
            i = ((i & 0x0f0f0f0f) << 4) | ((i >> 4) & 0x0f0f0f0f);
            i = ((i & 0x00ff00ff) << 8) | ((i >> 8) & 0x00ff00ff);
            i = ((i & 0x0000ffff) << 16) | ((i >> 16) & 0x0000ffff);
            return i;
        }

    }

}
