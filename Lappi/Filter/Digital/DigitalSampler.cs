﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Lappi.Filter.Digital {

    public class DigitalSampler : DigitalSampler<double> {

        public DigitalSampler (DigitalFilter filter) : base(filter) {
        }

    }

    public class DigitalSampler<T> where T : new() {

        private readonly DigitalFilter filter;
        private readonly double[] coefficients;

        public DigitalSampler (DigitalFilter filter) {
            this.filter = filter;
            coefficients = Normalize(filter.Coefficients);
        }

        public T Sample (T[] source, int center) {
            int left = Math.Max(center + filter.Left, 0);
            int right = Math.Min(center + filter.Right, source.Length - 1);
            T result = new T();
            for( int index = left; index <= right; index++ ) {
                double weight = coefficients[index - center - filter.Left];
                result += (dynamic) source[index] * weight;
            }
            result /= (dynamic) Normalize(center, left, right);
            return result;
        }

        public T SampleHighpass (T[] source, int center) {
            return (dynamic) source[center] - Sample(source, center);
        }

        public T[] Convolute (T[] source) {
            T[] result = new T[source.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = Sample(source, i);
            }
            return result;
        }

        public T[] ConvoluteHighpass (T[] source) {
            T[] result = new T[source.Length];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = SampleHighpass(source, i);
            }
            return result;
        }

        public T[] Downsample (T[] source, int factor, int shift) {
            T[] result = new T[source.Length / factor];
            for( int i = 0; i < result.Length; i++ ) {
                result[i] = Sample(source, i * factor + shift);
            }
            return result;
        }

        public T[] Upsample (T[] source, int factor, int shift) {
            T[] v = new T[source.Length * factor];
            if( default(T) == null ) {
                for( int i = 0; i < v.Length; i++ ) {
                    v[i] = new T();
                }
            }
            for( int i = 0; i < source.Length; i++ ) {
                v[i * factor + shift] = source[i];
            }
            return Convolute(v);
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private double Normalize (int center, int left, int right) {
            double sum = 0;
            for( int index = left; index <= right; index++ ) {
                sum += coefficients[index - center - filter.Left];
            }
            double altSum = 0;
            for( int index = left; index <= right; index += 2 ) {
                altSum += coefficients[index - center - filter.Left];
            }
            for( int index = left + 1; index <= right; index += 2 ) {
                altSum -= coefficients[index - center - filter.Left];
            }
            double result = Math.Max(Math.Abs(sum), Math.Abs(altSum));
            return result == 0 ? 1 : result;
        }

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        private static double[] Normalize (double[] coefficients) {
            double sum = coefficients.Sum();
            double altSum = coefficients.Select((x, i) => i % 2 == 0 ? x : -x).Sum();
            double factor = Math.Max(Math.Abs(sum), Math.Abs(altSum));
            return factor == 0 ? coefficients : coefficients.Select(x => x / factor).ToArray();
        }

    }

}
