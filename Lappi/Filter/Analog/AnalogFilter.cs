namespace Lappi.Filter.Analog {

    public interface AnalogFilter {

        double Left { get; }
        double Right { get; }
        double Radius { get; }
        double this [double x] { get; }

    }

}
