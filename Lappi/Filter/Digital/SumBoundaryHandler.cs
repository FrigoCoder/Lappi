using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class SumBoundaryHandler : BoundaryHandler {

        private readonly DigitalFilter fullFilter;
        private readonly Dictionary<int, DigitalFilter> leftFilters = new Dictionary<int, DigitalFilter>();
        private readonly Dictionary<int, DigitalFilter> rightFilters = new Dictionary<int, DigitalFilter>();

        public SumBoundaryHandler (DigitalFilter filter) {
            fullFilter = CreateFilter(filter, filter.Left, filter.Right);
            for( int left = filter.Left + 1; left <= 0; left++ ) {
                leftFilters[-left] = CreateFilter(filter, left, filter.Right);
            }
            for( int right = 0; right <= filter.Right - 1; right++ ) {
                rightFilters[right] = CreateFilter(filter, filter.Left, right);
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

        private DigitalFilter CreateFilter (DigitalFilter source, int left, int right) {
            double[] coeffs = Arrays.New(right - left + 1, i => source[i + left]);
            double factor = GetFactor(coeffs);
            return new DigitalFilter(-left, coeffs.Select(x => x / factor).ToArray());
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
