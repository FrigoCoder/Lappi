namespace Lappi.Filter.Digital {

    public interface BoundaryHandler {

        Filter1D GetFilter (int center, int length);

    }

}
