﻿using System;

using Lappi.Filter.Analog;

using NUnit.Framework;

namespace LappiTest.Filter.Analog {

    [TestFixture]
    public class DirichletTest {

        [TestCase]
        public void Dirichlet_downsampling_coefficients_are_appropriate () {
            FilterTestUtil.AssertCoefficients(new Dirichlet(1), 2.0, new[] {0.5, 1.0, 0.5});
            FilterTestUtil.AssertCoefficients(new Dirichlet(2), 2.0,
                new[] {-0.10355339059327377, 0, 0.60355339059327384, 1.0, 0.60355339059327384, 0, -0.10355339059327377});
            FilterTestUtil.AssertCoefficients(new Dirichlet(3), 2.0,
                new[] {
                    0.044658198738520435, 0, -0.16666666666666666, 0, 0.62200846792814635, 1.0, 0.62200846792814635, 0, -0.16666666666666666, 0,
                    0.044658198738520435
                });
        }

        [TestCase]
        public void Dirichlet_same_as_naive_implementation () {
            FilterTestUtil.AssertFiltersEqual(new Dirichlet(1), new DirichletNaive(1), 0.125);
            FilterTestUtil.AssertFiltersEqual(new Dirichlet(2), new DirichletNaive(2), 0.125);
            FilterTestUtil.AssertFiltersEqual(new Dirichlet(3), new DirichletNaive(3), 0.125);
            FilterTestUtil.AssertFiltersEqual(new Dirichlet(4), new DirichletNaive(4), 0.125);
        }

        private class DirichletNaive : AnalogFilter, ResamplingFilter {

            private readonly int _n;
            public double Left => -_n;
            public double Right => _n;
            public double Radius => _n;

            public Func<double, double> Function => x => {
                if( Math.Abs(x) >= _n ) {
                    return 0;
                }

                double z = x / _n * Math.PI;

                double result = 0.5;
                for( int k = 1; k < _n; k++ ) {
                    result += Math.Cos(k * z);
                }
                result += 0.5 * Math.Cos(_n * z);
                return result / _n;
            };

            public DirichletNaive (int n) {
                _n = n;
            }

        }

    }

}
