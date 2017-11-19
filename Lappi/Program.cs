using System;
using System.Windows.Forms;

using Lappi.Filter.Digital;
using Lappi.Image;

namespace Lappi {

    public static class Program {

        [STAThread]
        private static void Main () {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //            Application.Run(new MainForm());

            Image<YuvD> lenna = Image<YuvD>.Load("Resources\\Lenna.png");
            Laplacian2D<YuvD> transform = new Laplacian2D<YuvD>(CDF97.AnalysisLowpass, CDF97.SynthesisLowpass);

            Image<YuvD>[] transformed = transform.Forward(lenna, 5);
            Image<YuvD> reconstructed = transform.Inverse(transformed);

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
