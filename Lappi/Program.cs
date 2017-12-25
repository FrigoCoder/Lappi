using System;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;
using Lappi.Filter.Digital2D;
using Lappi.Image;

namespace Lappi {

    public static class Program {

        [STAThread]
        private static void Main () {
            Image<YuvD> lenna = Image<YuvD>.Load("Resources\\Lenna.png");

            // DigitalFilter analysis = new CoefficientAdapter(4, new[] {0.015625, 0, -0.125, 0.25, 0.71875, 0.25, -0.125, 0, 0.015625});
            // DigitalFilter synthesis = new CoefficientAdapter(3, new[] {-0.03125, 0, 0.28125, 0.5, 0.28125, 0, -0.03125});

            // DigitalFilter2D analysis = new SeparableAdapter(new DigitalAdapter(new Dirichlet(3), 2.0));
            // DigitalFilter2D synthesis = new SeparableAdapter(new DigitalAdapter(new Dirichlet(2), 2.0));

            DigitalFilter analysis = new DigitalAdapter(new Dirichlet(3), 2.0);
            DigitalFilter synthesis = new DigitalAdapter(new Dirichlet(2), 2.0);

            Laplacian2D<YuvD> transform = new Laplacian2D<YuvD>(analysis, synthesis);
            Image<YuvD>[] transformed = transform.Forward(lenna, 5);

            for( int i = 1; i < transformed.Length; i++ ) {
                transformed[i] = new Image<YuvD>(transformed[i].Xs, transformed[i].Ys, (x, y) => new YuvD());
            }

            transformed[0].Save("c:\\temp\\lenna-0.png");
            for( int i = 1; i < transformed.Length; i++ ) {
                transformed[i].Normalize();
                transformed[i].Save($"c:\\temp\\lenna-{i}.png");
            }

            Image<YuvD> reconstructed = transform.Inverse(transformed);
            reconstructed.Save("c:\\temp\\lenna-r.png");
        }

        private static void Normalize (this Image<YuvD> image) {
            image.ForEach((x, y) => image[x, y] += new YuvD(0.5, 0, 0));
        }

    }

}
