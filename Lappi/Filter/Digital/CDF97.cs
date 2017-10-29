namespace Lappi.Filter.Digital {

    public static class CDF97 {

        public static readonly DigitalFilter AnalysisLowpass =
            new CoefficientAdapter(4,
                new[] {
                    0.02674874108097600, -0.01686411844287495, -0.07822326652898785, 0.26686411844287230, 0.60294901823635790, 0.26686411844287230,
                    -0.07822326652898785, -0.01686411844287495, 0.02674874108097600
                });

        public static readonly DigitalFilter AnalysisHighpass =
            new CoefficientAdapter(3,
                new[] {
                    0.045635881557124740, -0.028771763114249785, -0.295635881557123500, 0.557543526228497000, -0.295635881557123500,
                    -0.028771763114249785, 0.045635881557124740
                });

        public static readonly DigitalFilter SynthesisLowpass =
            new CoefficientAdapter(3,
                new[] {
                    -0.045635881557124740, -0.028771763114249785, 0.295635881557123500, 0.557543526228497000, 0.295635881557123500,
                    -0.028771763114249785, -0.045635881557124740
                });

        public static readonly DigitalFilter SynthesisHighpass =
            new CoefficientAdapter(4,
                new[] {
                    0.02674874108097600, 0.01686411844287495, -0.07822326652898785, -0.26686411844287230, 0.60294901823635790, -0.26686411844287230,
                    -0.07822326652898785, 0.01686411844287495, 0.02674874108097600
                });

    }

}
