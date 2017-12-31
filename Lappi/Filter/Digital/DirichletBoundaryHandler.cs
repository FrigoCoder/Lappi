using System.Collections.Generic;
using System.Linq;

using Lappi.Filter.Analog;
using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class DirichletBoundaryHandler : BoundaryHandler {

        private readonly int max;
        private readonly DigitalFilter maxFilter;
        private readonly Dictionary<int, DigitalFilter> leftFilters = new Dictionary<int, DigitalFilter>();
        private readonly Dictionary<int, DigitalFilter> rightFilters = new Dictionary<int, DigitalFilter>();

        public DirichletBoundaryHandler (int max) {
            this.max = max;
            maxFilter = NormalizedDirichlet(max);
            leftFilters[0] = new CoefficientAdapter(0, new[] {0.5, 0.5});
            rightFilters[0] = new CoefficientAdapter(1, new[] {0.5, 0.5});
            for( int i = 1; i < max; i++ ) {
                DigitalFilter filter = NormalizedDirichlet(i);
                leftFilters[2 * i] = leftFilters[2 * i - 1] = rightFilters[2 * i] = rightFilters[2 * i - 1] = filter;
            }
        }

        public DigitalFilter GetFilter (int index, int length) {
            Preconditions.Require(0 <= index && index < length);
            if( leftFilters.ContainsKey(index) ) {
                return leftFilters[index];
            }
            if( rightFilters.ContainsKey(length - 1 - index) ) {
                return rightFilters[length - 1 - index];
            }
            return maxFilter;
        }

        private static DigitalFilter NormalizedDirichlet (double radius) {
            DigitalFilter filter = new DigitalAdapter(new Dirichlet(radius), 2.0);
            double sum = filter.Coefficients.Sum();
            return new CoefficientAdapter(-filter.Left, filter.Coefficients.Select(x => x / sum).ToArray());
        }

    }

}
