using System;

namespace Lappi.Util {

    public static class Preconditions {

        public static void Require (bool condition) {
            if( !condition ) {
                throw new ArgumentException();
            }
        }

        public static void Require<E> (bool condition) where E : Exception, new() {
            if( !condition ) {
                throw new E();
            }
        }

        public static void Require (bool condition, Type exception) {
            if( !condition ) {
                throw (Exception) Activator.CreateInstance(exception);
            }
        }

    }

}
