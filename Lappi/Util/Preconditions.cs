using System;

namespace Lappi.Util {

    public static class Preconditions {

        public static void Require<E> (bool condition) where E : Exception, new() {
            if( !condition ) {
                throw new E();
            }
        }

    }

}
