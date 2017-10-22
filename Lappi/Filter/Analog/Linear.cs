using System;

namespace Lappi.Filter.Analog {

    public class Linear : AnalogFilter, ResamplingFilter {

        public double Left => -1;
        public double Right => +1;
        public double Radius => 1;

        public Func<double, double> Kernel => x => {
            double t = Math.Abs(x);
            if( t >= 1 ) {
                return 0;
            }
            return 1 - t;
        };

    }

}
