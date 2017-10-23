using Lappi.Filter.Analog;
using Lappi.Filter.Digital;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    [TestFixture]
    public class HighpassAdapterTest {

        [TestCase]
        public void Highpass_of_linear_filter () {
            DigitalFilter highpass = new HighpassAdapter(new DigitalAdapter(new Linear(), 2.0));
            double[] expected = {-0.25, 0.5, -0.25};
            FilterTestUtil.AssertCoefficients(highpass, expected);
        }

        [TestCase]
        public void Highpass_of_dirichlet_filter () {
            DigitalFilter highpass = new HighpassAdapter(new DigitalAdapter(new Dirichlet(4), 2.0));
            double[] expected = {
                0.0124320229612286, 0.0000000000000000, -0.0417611648699562, 0.0000000000000000, 0.0935378601665931, 0.0000000000000000,
                -0.3142087182578655, 0.5000000000000000, -0.3142087182578655, 0.0000000000000000, 0.0935378601665930, 0.0000000000000000,
                -0.0417611648699562, 0.0000000000000000, 0.0124320229612286
            };
            FilterTestUtil.AssertCoefficients(highpass, expected);
        }

    }

}
