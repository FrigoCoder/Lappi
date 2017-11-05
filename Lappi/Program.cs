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

            Image<YuvD> image = Image<YuvD>.Load("Resources\\Lenna.png");

            Laplacian2D<YuvD> laplacian = new Laplacian2D<YuvD>(CDF97.AnalysisLowpass, CDF97.SynthesisLowpass);
            Tuple<Image<YuvD>, Image<YuvD>> tuple = laplacian.Forward(image);
            Image<YuvD> low = tuple.Item1;
            Image<YuvD> high = tuple.Item2;
            for( int x = 0; x < high.Xs; x++ ) {
                for( int y = 0; y < high.Ys; y++ ) {
                    high[x, y] += new YuvD(0.5, 0, 0);
                }
            }

            low.Save("c:\\temp\\low.png");
            high.Save("c:\\temp\\high.png");
        }

    }

}
