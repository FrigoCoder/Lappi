namespace Lappi.Filter.Digital {

    public interface Filter2D {

        int Left { get; }
        int Right { get; }
        int Top { get; }
        int Bottom { get; }
        double[,] Coefficients { get; }
        double this [int x, int y] { get; }

    }

}
