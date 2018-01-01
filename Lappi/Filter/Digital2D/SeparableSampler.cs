using System;

using Lappi.Filter.Digital;
using Lappi.Image;

namespace Lappi.Filter.Digital2D {

    public class SeparableSampler<T> : Sampler2D<T> where T : new() {

        private readonly Sampler1D<T> sampler;

        public SeparableSampler (Sampler1D<T> sampler) {
            this.sampler = sampler;
        }

        public T Sample (Image<T> source, int cx, int cy) => throw new NotImplementedException();

        public Image<T> Convolute (Image<T> source) => throw new NotImplementedException();

        public Image<T> Downsample (Image<T> source) {
            Image<T> half = new Image<T>(source.Xs / 2, source.Ys);
            for( int y = 0; y < source.Ys; y++ ) {
                half.Rows[y] = sampler.Downsample(source.Rows[y]);
            }
            Image<T> quarter = new Image<T>(source.Xs / 2, source.Ys / 2);
            for( int x = 0; x < source.Xs / 2; x++ ) {
                quarter.Columns[x] = sampler.Downsample(half.Columns[x]);
            }
            return quarter;
        }

        public Image<T> Upsample (Image<T> quarter, int xs, int ys) {
            Image<T> half = new Image<T>(quarter.Xs, ys);
            for( int x = 0; x < half.Xs; x++ ) {
                half.Columns[x] = sampler.Upsample(quarter.Columns[x], ys);
            }
            Image<T> full = new Image<T>(xs, ys);
            for( int y = 0; y < full.Ys; y++ ) {
                full.Rows[y] = sampler.Upsample(half.Rows[y], xs);
            }
            return full;
        }

    }

}
