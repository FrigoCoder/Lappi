using System;
using System.Linq;

namespace Lappi.Filter.Digital {

    public class Dirichlet2BoundaryHandler : BoundaryHandler {

        public DigitalFilter GetFilter (int index, int length) {
            if( index == 0 ) {
                return Dirichlet0A;
            }
            if( index == 1 || index == 2 ) {
                return Dirichlet1;
            }
            if( 3 <= index && index <= length - 4 ) {
                return Dirichlet2;
            }
            if( index == length - 3 || index == length - 2 ) {
                return Dirichlet1;
            }
            if( index == length - 1 ) {
                return Dirichlet0B;
            }
            throw new ArgumentException();
        }

        private static readonly DigitalFilter Dirichlet0A = Normalize(new CoefficientAdapter(0, new[] {1.0, 1.0}));
        private static readonly DigitalFilter Dirichlet0B = Normalize(new CoefficientAdapter(1, new[] {1.0, 1.0}));
        private static readonly DigitalFilter Dirichlet1 = Normalize(new CoefficientAdapter(1, new[] {0.5, 1.0, 0.5}));

        private static readonly DigitalFilter Dirichlet2 = Normalize(new CoefficientAdapter(3,
            new[] {-0.10355339059327377, 0, 0.60355339059327384, 1.0, 0.60355339059327384, 0, -0.10355339059327377}));

        private static DigitalFilter Normalize (DigitalFilter filter) {
            double sum = filter.Coefficients.Sum();
            return new CoefficientAdapter(-filter.Left, filter.Coefficients.Select(x => x / sum).ToArray());
        }

    }

}
