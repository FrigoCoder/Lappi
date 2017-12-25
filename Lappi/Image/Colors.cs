using System;
using System.Drawing;

namespace Lappi.Image {

    public static class Colors {

        public static T To<T> (Color color) => (T) Activator.CreateInstance(typeof(T), color);

        public static T ToT<T> (this Color color) => (T) Activator.CreateInstance(typeof(T), color);

    }

}
