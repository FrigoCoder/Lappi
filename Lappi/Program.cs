using Lappi.Filter.Digital;
using Lappi.Filter.Digital2D;
using Lappi.Image;

namespace Lappi {

    public static class Program {

        public static void Main () {
            Image<YuvD> lenna = Image<YuvD>.Load("Resources\\Lenna.png");

            Sampler2D<YuvD> analysis = new SeparableSampler<YuvD>(new DigitalSampler<YuvD>(new Dirichlet3BoundaryHandler()));
            Sampler2D<YuvD> synthesis = new SeparableSampler<YuvD>(new DigitalSampler<YuvD>(new Dirichlet2BoundaryHandler()));

            Laplacian2D<YuvD> transform = new Laplacian2D<YuvD>(analysis, synthesis);
            Image<YuvD>[] transformed = transform.Forward(lenna, 5);

            transformed[0].Save("c:\\temp\\lenna-0.png");
            for( int i = 1; i < transformed.Length; i++ ) {
                transformed[i].Normalize();
                transformed[i].Save($"c:\\temp\\lenna-{i}.png");
                transformed[i].Clear();
            }

            Image<YuvD> reconstructed = transform.Inverse(transformed);
            reconstructed.Save("c:\\temp\\lenna-r.png");
        }

        private static void Normalize (this Image<YuvD> image) {
            image.ForEach((x, y) => image[x, y] += new YuvD(0.5, 0, 0));
        }

        private static void Clear (this Image<YuvD> image) {
            image.ForEach((x, y) => image[x, y] = new YuvD());
        }

    }

}
