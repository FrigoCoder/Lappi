using System.Collections.Generic;

using Lappi.Filter.Analog;
using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class DirichletBoundaryHandler : BoundaryHandler {

        private readonly Filter1D fullFilter;
        private readonly Dictionary<int, Filter1D> leftFilters = new Dictionary<int, Filter1D>();
        private readonly Dictionary<int, Filter1D> rightFilters = new Dictionary<int, Filter1D>();

        public DirichletBoundaryHandler (int max) {
            fullFilter = NormalizedDirichlet(max);
            leftFilters[0] = new Filter1D(0, new[] {0.5, 0.5});
            rightFilters[0] = new Filter1D(1, new[] {0.5, 0.5});
            for( int i = 1; i < max; i++ ) {
                Filter1D filter = NormalizedDirichlet(i);
                leftFilters[2 * i] = leftFilters[2 * i - 1] = rightFilters[2 * i] = rightFilters[2 * i - 1] = filter;
            }
        }

        public Filter1D GetFilter (int center, int length) {
            Preconditions.Require(0 <= center && center < length);
            if( leftFilters.TryGetValue(center, out Filter1D leftFilter) ) {
                return leftFilter;
            }
            if( rightFilters.TryGetValue(length - 1 - center, out Filter1D rightFilter) ) {
                return rightFilter;
            }
            return fullFilter;
        }

        private static Filter1D NormalizedDirichlet (double radius) => new Filter1D(new Dirichlet(radius), 2.0).Normalize();

    }

}
