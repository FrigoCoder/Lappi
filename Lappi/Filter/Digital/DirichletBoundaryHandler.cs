using System.Collections.Generic;

using Lappi.Filter.Analog;
using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class DirichletBoundaryHandler : BoundaryHandler {

        private readonly DigitalFilter fullFilter;
        private readonly Dictionary<int, DigitalFilter> leftFilters = new Dictionary<int, DigitalFilter>();
        private readonly Dictionary<int, DigitalFilter> rightFilters = new Dictionary<int, DigitalFilter>();

        public DirichletBoundaryHandler (int max) {
            fullFilter = NormalizedDirichlet(max);
            leftFilters[0] = new DigitalFilter(0, new[] {0.5, 0.5});
            rightFilters[0] = new DigitalFilter(1, new[] {0.5, 0.5});
            for( int i = 1; i < max; i++ ) {
                DigitalFilter filter = NormalizedDirichlet(i);
                leftFilters[2 * i] = leftFilters[2 * i - 1] = rightFilters[2 * i] = rightFilters[2 * i - 1] = filter;
            }
        }

        public DigitalFilter GetFilter (int center, int length) {
            Preconditions.Require(0 <= center && center < length);
            if( leftFilters.TryGetValue(center, out DigitalFilter leftFilter) ) {
                return leftFilter;
            }
            if( rightFilters.TryGetValue(length - 1 - center, out DigitalFilter rightFilter) ) {
                return rightFilter;
            }
            return fullFilter;
        }

        private static DigitalFilter NormalizedDirichlet (double radius) => new DigitalFilter(new Dirichlet(radius), 2.0).Normalize();

    }

}
