using System;
using System.Windows.Forms;

using Lappi.Filter.Digital;
using Lappi.Filter.Digital2D;
using Lappi.Image;

namespace Lappi {

    public static class Program {

        [STAThread]
        private static void Main () {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Image<YuvD> lenna = Image<YuvD>.Load("Resources\\Lenna.png");

            /*            for( int a = 1; a <= 8; a++ ) {
                            for( int s = 1; s <= 8; s++ ) {
                                DigitalFilter analysis = new DigitalAdapter(new Dirichlet(a), 2.0);
                                DigitalFilter synthesis = new DigitalAdapter(new Dirichlet(s), 2.0);
                                LaplacianSeparable<YuvD> transform = new LaplacianSeparable<YuvD>(analysis, synthesis);

                                Image<YuvD>[] transformed = transform.Forward(lenna, 5);
                                Image<YuvD> reconstructed = transform.Inverse(transformed);

                                transformed[5].Normalize();
                                transformed[5].Save($"c:\\temp\\lenna-{a}_{s}.png");
                                Console.WriteLine($"c:\\temp\\lenna-{a}_{s}.png");
                            }
                        }
            */

            //                        DigitalFilter analysis = new DigitalAdapter(new Dirichlet(3), 2.0);
            //                        DigitalFilter synthesis = new DigitalAdapter(new Dirichlet(3), 2.0);

            DigitalFilter analysis = new CoefficientAdapter(4, new[] {0.015625, 0, -0.125, 0.25, 0.71875, 0.25, -0.125, 0, 0.015625});
            DigitalFilter synthesis = new CoefficientAdapter(3, new[] {-0.03125, 0, 0.28125, 0.5, 0.28125, 0, -0.03125});
            Laplacian2D<YuvD> transform = new Laplacian2D<YuvD>(analysis, synthesis);

            Image<YuvD>[] transformed = transform.Forward(lenna, 5);
            Image<YuvD> reconstructed = transform.Inverse(transformed);

            transformed[0].Save("c:\\temp\\lenna-0.png");
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
