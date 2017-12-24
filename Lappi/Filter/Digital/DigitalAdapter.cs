﻿using System;
using System.Diagnostics.CodeAnalysis;

using Lappi.Filter.Analog;

namespace Lappi.Filter.Digital {

    public class DigitalAdapter : DigitalFilter {

        public int Left { get; }
        public int Right { get; }
        public int Radius { get; }
        public double[] Coefficients { get; }

        public DigitalAdapter (AnalogFilter analog, double scale) {
            Left = NonZeroLeft(analog, scale);
            Right = NonZeroRight(analog, scale);
            Radius = Math.Max(Math.Abs(Left), Math.Abs(Right));
            Coefficients = new double[Right - Left + 1];
            for( int i = 0; i < Coefficients.Length; i++ ) {
                Coefficients[i] = analog[(i + Left) / scale];
            }
        }

        public override string ToString () => GetType().Name + "{Left = " + Left + ", Right = " + Right + ", Radius = " + Radius +
            ", Coefficients=[" + string.Join(", ", Coefficients) + "]}";

        public double this [int x] => Coefficients[x - Left];

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static int NonZeroLeft (AnalogFilter analog, double scale) {
            int left = Convert.ToInt32(Math.Ceiling(analog.Left * scale));
            while( analog[left / scale] == 0 ) {
                left++;
            }
            return left;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static int NonZeroRight (AnalogFilter analog, double scale) {
            int right = Convert.ToInt32(Math.Floor(analog.Right * scale));
            while( analog[right / scale] == 0 ) {
                right--;
            }
            return right;
        }

    }

}
