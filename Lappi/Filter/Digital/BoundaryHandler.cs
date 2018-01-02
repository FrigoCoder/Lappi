namespace Lappi.Filter.Digital {

    public interface BoundaryHandler {

        DigitalFilter GetFilter (int center, int length);

    }

}
