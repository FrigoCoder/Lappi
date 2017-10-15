﻿using System;
using System.Diagnostics.CodeAnalysis;

using Lappi.Filter.Analog;

namespace Lappi.Filter.Digital {

    public class DigitalAdapter : DigitalFilter {

        public int Left { get; }
        public int Right { get; }
        public int Radius { get; }
        public Func<int, double> Function => x => coefficients[x - Left];
        private readonly double[] coefficients;

        public DigitalAdapter (AnalogFilter analog, double scale) {
            Left = NonZeroLeft(analog, scale);
            Right = NonZeroRight(analog, scale);
            Radius = Math.Max(Math.Abs(Left), Math.Abs(Right));
            coefficients = new double[Right - Left + 1];
            for( int i = 0; i < coefficients.Length; i++ ) {
                coefficients[i] = analog.Function((i + Left) / scale);
            }
        }

        public override string ToString () {
            return GetType().Name + "{Left = " + Left + ", Right = " + Right + ", Radius = " + Radius + ", Coefficients=[" +
                string.Join(", ", coefficients) + "]}";
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static int NonZeroLeft (AnalogFilter analog, double scale) {
            int left = Convert.ToInt32(Math.Ceiling(analog.Left * scale));
            while( analog.Function(left / scale) == 0 ) {
                left++;
            }
            return left;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static int NonZeroRight (AnalogFilter analog, double scale) {
            int right = Convert.ToInt32(Math.Floor(analog.Right * scale));
            while( analog.Function(right / scale) == 0 ) {
                right--;
            }
            return right;
        }

    }

}
