using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Lappi.Util;

namespace Lappi.Filter.Digital {

    public class SumBoundaryHandler : BoundaryHandler {

        private readonly DigitalFilter normalized;
        private readonly Dictionary<int, DigitalFilter> leftFilters = new Dictionary<int, DigitalFilter>();
        private readonly Dictionary<int, DigitalFilter> rightFilters = new Dictionary<int, DigitalFilter>();

        public SumBoundaryHandler (DigitalFilter filter) {
            normalized = CreateFilter(filter, filter.Left, filter.Right);
            for( int left = filter.Left; left <= 0; left++ ) {
                leftFilters[left] = CreateFilter(filter, left, filter.Right);
            }
            for( int right = 0; right <= filter.Right; right++ ) {
                rightFilters[right] = CreateFilter(filter, filter.Left, right);
            }
        }

        public DigitalFilter GetFilter (int center, int length) {
            Preconditions.Require(0 <= center && center < length);
            int left = Math.Max(0, center + normalized.Left) - center;
            int right = Math.Min(length - 1, center + normalized.Right) - center;
            if( left == normalized.Left && right == normalized.Right ) {
                return normalized;
            }
            if( left != normalized.Left ) {
                return leftFilters[left];
            }
            if( right != normalized.Right ) {
                return rightFilters[right];
            }
            throw new ArgumentException();
        }

        private DigitalFilter CreateFilter (DigitalFilter source, int left, int right) {
            double[] coeffs = Arrays.New(right - left + 1, i => source[i + left]);
            double factor = GetFactor(coeffs);
            return new CoefficientAdapter(-left, coeffs.Select(x => x / factor).ToArray());
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
