using System;
using System.Windows.Forms;

using Lappi.Filter.Analog;
using Lappi.Filter.Digital;
using Lappi.Image;

namespace Lappi {

    public static class Program {

        [STAThread]
        private static void Main () {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Image<YuvD> lenna = Image<YuvD>.Load("Resources\\Lenna.png");

            Filter2D analysis = new RadialAdapter(new Dirichlet(4), 2.0);
            Filter2D synthesis = new RadialAdapter(new Dirichlet(3), 2.0);
            Laplacian2D<YuvD> transform = new Laplacian2D<YuvD>(analysis, synthesis);

/*            DigitalFilter analysis = new DigitalAdapter(new Dirichlet(4), 2.0);
            DigitalFilter synthesis = new DigitalAdapter(new Dirichlet(3), 2.0);
            LaplacianSeparable<YuvD> transform = new LaplacianSeparable<YuvD>(analysis, synthesis);*/

            Image<YuvD>[] transformed = transform.Forward(lenna, 5);
            Image<YuvD> reconstructed = transform.Inverse(transformed);

            transformed[0].Save($"c:\\temp\\lenna-0.png");
            for( int i = 1; i < transformed.Length; i++ ) {
                transformed[i].Normalize();
                transformed[i].Save($"c:\\temp\\lenna-{i}.png");
            }
            reconstructed.Save("c:\\temp\\lenna-r.png");
        }

        private static void Normalize (this Image<YuvD> image) {
            for( int x = 0; x < image.Xs; x++ ) {
                for( int y = 0; y < image.Ys; y++ ) {
                    image[x, y] += new YuvD(0.5, 0, 0);
                }
            }
        }

    }

}
