using System;

namespace Lappi.Filter.Analog {

    public class Linear : AnalogFilter {

        public double Left => -1;
        public double Right => +1;
        public double Radius => 1;

        public double this [double x] {
            get {
                double t = Math.Abs(x);
                if( t >= 1 ) {
                    return 0;
                }
                return 1 - t;
            }
        }

    }

}
