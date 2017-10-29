namespace Lappi.Filter.Digital {

    public class HighpassSampler<T> : DigitalSampler<T> where T : new() {

        public HighpassSampler (DigitalFilter filter) : base(filter) {
        }

        public override T Sample (T[] source, int center) {
            return (dynamic) source[center] - base.Sample(source, center);
        }

    }

}
