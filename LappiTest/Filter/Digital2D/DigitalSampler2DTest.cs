using Lappi.Filter.Analog;
using Lappi.Filter.Digital2D;
using Lappi.Image;

using NUnit.Framework;

namespace LappiTest.Filter.Digital2D {

    using Image = Image<double>;
    using DigitalSampler2D = DigitalSampler2D<double>;

    public class DigitalSampler2DTest {

        private readonly Image source = new Image(new double[,]
            {{1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}, {1, 4, 9, 16, 25, 36}});

        private readonly DigitalSampler2D linear1 = new DigitalSampler2D(new RadialAdapter(new Linear(), 1.0));
        private readonly DigitalSampler2D linear2 = new DigitalSampler2D(new RadialAdapter(new Linear(), 2.0));

        [Test]
        public void Convolute_with_scale_1_results_in_the_original_image () {
            AssertEquals(linear1.Convolute(source), source);
        }

        [Test]
        public void Convolute_with_scale_2_results_in_blurred_image () {
            Image expected = new Image(new[,] {
                {2.0374140570188866, 4.5139002551474166, 9.5139002551474157, 16.513900255147416, 25.513900255147412, 32.196148457597417},
                {2.0556010205896675, 4.5205645305001854, 9.5205645305001845, 16.520564530500184, 25.520564530500181, 32.129462924504551},
                {2.0556010205896675, 4.5205645305001854, 9.5205645305001845, 16.520564530500184, 25.520564530500181, 32.129462924504551},
                {2.0374140570188866, 4.5139002551474166, 9.5139002551474157, 16.513900255147416, 25.513900255147412, 32.196148457597417}
            });
            AssertEquals(linear2.Convolute(source), expected);
        }

        private void AssertEquals (Image actual, Image expected) {
            Assert.That(actual.Xs, Is.EqualTo(expected.Xs));
            Assert.That(actual.Ys, Is.EqualTo(expected.Ys));
            for( int x = 0; x < actual.Xs; x++ ) {
                for( int y = 0; y < actual.Ys; y++ ) {
                    Assert.That(actual[x, y], Is.EqualTo(expected[x, y]).Within(1E-14), $"At {x}, {y}");
                }
            }
        }

    }

}
