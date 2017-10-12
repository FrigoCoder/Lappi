using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class DigitalAdapterTest {

        [TestCase]
        public void Linear_filter_with_scale_1 () {
            FilterTestUtil.AssertCoefficients(new DigitalAdapter(new Linear(), 1.0), new[] {1.0});
            FilterTestUtil.AssertCoefficients(new DigitalAdapter(new Linear(), 2.0), new[] {0.5, 1.0, 0.5});
            FilterTestUtil.AssertCoefficients(new DigitalAdapter(new Linear(), 3.0), new[] {1.0 / 3.0, 2.0 / 3.0, 1, 2.0 / 3.0, 1.0 / 3.0});
            FilterTestUtil.AssertCoefficients(new DigitalAdapter(new Linear(), 4.0), new[] {0.25, 0.5, 0.75, 1.0, 0.75, 0.5, 0.25});
        }

    }

}
