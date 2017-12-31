using System;
using System.Linq;

using Lappi.Filter.Analog;

namespace Lappi.Filter.Digital {

    public class Dirichlet3BoundaryHandler : BoundaryHandler {

        public DigitalFilter GetFilter (int index, int length) {
            if( index == length - 1 ) {
                return Dirichlet0B;
            }
            switch( Math.Min(index, length - 1 - index) ) {
                case 0:
                    return Dirichlet0A;
                case 1:
                case 2:
                    return Dirichlet1;
                case 3:
                case 4:
                    return Dirichlet2;
                default:
                    return Dirichlet3;
            }
        }

        private static readonly DigitalFilter Dirichlet0A = new CoefficientAdapter(0, new[] {0.5, 0.5});
        private static readonly DigitalFilter Dirichlet0B = new CoefficientAdapter(1, new[] {0.5, 0.5});
        private static readonly DigitalFilter Dirichlet1 = NormalizedDirichlet(1);
        private static readonly DigitalFilter Dirichlet2 = NormalizedDirichlet(2);
        private static readonly DigitalFilter Dirichlet3 = NormalizedDirichlet(3);

        private static DigitalFilter NormalizedDirichlet (double radius) {
            DigitalFilter filter = new DigitalAdapter(new Dirichlet(radius), 2.0);
            double sum = filter.Coefficients.Sum();
            return new CoefficientAdapter(-filter.Left, filter.Coefficients.Select(x => x / sum).ToArray());
        }

    }

}
