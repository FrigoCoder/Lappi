using Lappi.Filter.Analog;
using Lappi.Filter.Digital;
using Lappi.Filter.Digital2D;
using Lappi.Image;

using LappiTest.Image;

using NUnit.Framework;

namespace LappiTest.Filter.Digital2D {

    using Image = Image<double>;
    using DigitalSampler2D = NonSeparableSampler<double>;

    public class DigitalSampler2DTest {

        private readonly Image source = new Image(new double[,]
            {{1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {0, 0, 0, 0, 0, 0}});

        private readonly DigitalSampler2D linear1 = new DigitalSampler2D(new DigitalFilter2D(new DigitalFilter(new Linear(), 1.0)));
        private readonly DigitalSampler2D linear2 = new DigitalSampler2D(new DigitalFilter2D(new DigitalFilter(new Linear(), 2.0)));

        [Test]
        public void Convolute_with_scale_1_results_in_the_original_image () {
            ImageTest.AssertEquals(linear1.Convolute(source), source);
        }

        [Test]
        public void Convolute_with_scale_2_results_in_blurred_image () {
            Image expected = new Image(new[,] {
                {2.0, 4.5, 9.5, 16.5, 25.5, 32.33333333333333}, {2.0, 4.5, 9.5, 16.5, 25.5, 32.33333333333333},
                {1.5, 3.375, 7.125, 12.375, 19.125, 24.25}, {0.6666666666666666, 1.5, 3.166666666666666, 5.5, 8.5, 10.77777777777777}
            });
            ImageTest.AssertEquals(linear2.Convolute(source), expected);
        }

    }

}
