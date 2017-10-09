using System;

namespace Lappi.Filter.Analog {

    public interface AnalogFilter {

        double Left { get; }
        double Right { get; }
        double Radius { get; }
        Func<double, double> Function { get; }

    }

}
