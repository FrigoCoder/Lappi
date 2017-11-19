using System;

namespace Lappi.Filter.Digital {

    public interface Filter2D {

        int Left { get; }
        int Right { get; }
        int Top { get; }
        int Bottom { get; }
        double[,] Coefficients { get; }
        Func<int, int, double> Kernel { get; }

    }

}
