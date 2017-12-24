namespace Lappi.Filter.Digital2D {

    public interface DigitalFilter2D {

        int Left { get; }
        int Right { get; }
        int Top { get; }
        int Bottom { get; }
        double[,] Coefficients { get; }
        double this [int x, int y] { get; }

    }

}
