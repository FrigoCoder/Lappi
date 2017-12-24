namespace Lappi.Filter.Digital {

    public interface DigitalFilter {

        int Left { get; }
        int Right { get; }
        int Radius { get; }
        double[] Coefficients { get; }
        double this [int x] { get; }

    }

}
