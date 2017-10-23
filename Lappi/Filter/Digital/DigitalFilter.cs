using System;

namespace Lappi.Filter.Digital {

    public interface DigitalFilter {

        int Left { get; }
        int Right { get; }
        int Radius { get; }
        double[] Coefficients { get; }
        Func<int, double> Kernel { get; }

    }

}
