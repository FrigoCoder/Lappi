using System;

namespace Lappi.Filter.Analog {

    public class Dirichlet : AnalogFilter, ResamplingFilter {

        public double Left => -Radius;
        public double Right => Radius;
        public double Radius { get; }

        public Func<double, double> Function => x => {
            double t = Math.Abs(x);
            if( t > Radius ) {
                return 0;
            }
            if( t > 0 ) {
                double z = t * Math.PI / Radius;
                return 0.5 / Radius * (1.0 + Math.Cos(z)) * Math.Sin(z * Radius) / Math.Sin(z);
            }
            return 1;
        };

        public Dirichlet (double radius) {
            Radius = radius;
        }

    }

}
