using System;

namespace Lappi.Filter.Analog {

    public class Dirichlet : AnalogFilter {

        public double Left => -Radius;
        public double Right => Radius;
        public double Radius { get; }

        public Dirichlet (double radius) {
            Radius = radius;
        }

        public double this [double x] {
            get {
                double t = Math.Abs(x);
                if( t > Radius ) {
                    return 0;
                }
                if( t > 0 ) {
                    double z = t * Math.PI / Radius;
                    return 0.5 / Radius * (1.0 + Math.Cos(z)) * Math.Sin(z * Radius) / Math.Sin(z);
                }
                return 1;
            }
        }

    }

}
