using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Lappi.Filter.Analog;

namespace Lappi.Filter.Digital {

    public static class DigitalFilters {

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public static int NonZeroLeft (AnalogFilter analog, double scale) {
            int left = Convert.ToInt32(Math.Ceiling(analog.Left * scale));
            while( analog[left / scale] == 0 ) {
                left++;
            }
            return left;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public static int NonZeroRight (AnalogFilter analog, double scale) {
            int right = Convert.ToInt32(Math.Floor(analog.Right * scale));
            while( analog[right / scale] == 0 ) {
                right--;
            }
            return right;
        }

        public static DigitalFilter Normalize (this DigitalFilter filter) {
            double sum = filter.Coefficients.Sum();
            return new CoefficientAdapter(-filter.Left, filter.Coefficients.Select(x => x / sum).ToArray());
        }

        public static string ToString (this DigitalFilter filter) => filter.GetType().Name + "{Left = " + filter.Left + ", Right = " + filter.Right +
            ", Radius = " + filter.Radius + ", Coefficients=[" + string.Join(", ", filter.Coefficients) + "]}";

    }

}
