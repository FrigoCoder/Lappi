namespace Lappi.Filter.Digital {

    public static class CDF53 {

        public static readonly DigitalFilter AnalysisLowpass = new CoefficientAdapter(2, new[] {-0.125, 0.25, 0.75, 0.25, -0.125});
        public static readonly DigitalFilter AnalysisHighpass = new CoefficientAdapter(1, new[] {-0.25, 0.5, -0.25});

        public static readonly DigitalFilter SynthesisLowpass = new CoefficientAdapter(1, new[] {0.25, 0.5, 0.25});
        public static readonly DigitalFilter SynthesisHighpass = new CoefficientAdapter(2, new[] {-0.125, -0.25, 0.75, -0.25, -0.125});

    }

}
