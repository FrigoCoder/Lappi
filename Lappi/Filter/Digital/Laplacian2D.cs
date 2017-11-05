﻿using System;

namespace Lappi.Filter.Digital {

    public class Laplacian2D<T> where T : new() {

        private readonly DigitalSampler<T> analysis;
        private readonly DigitalSampler<T> synthesis;

        public Laplacian2D (DigitalFilter analysis, DigitalFilter synthesis) {
            this.analysis = new DigitalSampler<T>(analysis);
            this.synthesis = new DigitalSampler<T>(synthesis);
        }

        public Tuple<Image<T>, Image<T>> Forward (Image<T> image) {
            Image<T> half = new Image<T>(image.Xs / 2, image.Ys);
            for( int y = 0; y < image.Ys; y++ ) {
                half.Rows[y] = analysis.Downsample(image.Rows[y], 2, 0);
            }

            Image<T> quarter = new Image<T>(image.Xs / 2, image.Ys / 2);
            for( int x = 0; x < image.Xs / 2; x++ ) {
                quarter.Columns[x] = analysis.Downsample(half.Columns[x], 2, 0);
            }

            return Tuple.Create(quarter, new Image<T>(image.Xs, image.Ys));
        }

    }

}
