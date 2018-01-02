using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class SumBoundaryHandler : BoundaryHandler {

        private readonly Filter1D fullFilter;
        private readonly Dictionary<int, Filter1D> leftFilters = new Dictionary<int, Filter1D>();
        private readonly Dictionary<int, Filter1D> rightFilters = new Dictionary<int, Filter1D>();

        public SumBoundaryHandler (Filter1D filter) {
            fullFilter = CreateFilter(filter, filter.Left, filter.Right);
            for( int left = filter.Left + 1; left <= 0; left++ ) {
                leftFilters[-left] = CreateFilter(filter, left, filter.Right);
            }
            for( int right = 0; right <= filter.Right - 1; right++ ) {
                rightFilters[right] = CreateFilter(filter, filter.Left, right);
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

        private Filter1D CreateFilter (Filter1D source, int left, int right) {
            double[] coeffs = Arrays.New(right - left + 1, i => source[i + left]);
            double factor = GetFactor(coeffs);
            return new Filter1D(-left, coeffs.Select(x => x / factor).ToArray());
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private double GetFactor (double[] coeffs) {
            double sum = coeffs.Sum();
            double altSum = coeffs.Select((x, i) => i % 2 == 0 ? x : -x).Sum();
            double factor = Math.Max(Math.Abs(sum), Math.Abs(altSum));
            return factor == 0 ? 1 : factor;
        }

    }

}
