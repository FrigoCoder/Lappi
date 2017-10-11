using System;

namespace Lappi.Filter.Digital {

    public interface DigitalFilter {

        int Left { get; }
        int Right { get; }
        int Radius { get; }
        Func<int, double> Function { get; }

    }

}
