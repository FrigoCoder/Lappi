using System;

using Lappi.Filter.Digital;
using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Filter.Digital {

    using Sampler1D = Sampler1D<double>;
    using DigitalSampler = DigitalSampler<double>;
    using Dirichlet3Sampler = Dirichlet3Sampler<double>;

    public class Dirichlet3SamplerTest {

        private readonly double[] array = Arrays.New(1024, i => random.NextDouble());
        private readonly Sampler1D dirichletSampler = new Dirichlet3Sampler();
        private readonly Sampler1D digitalSampler = new DigitalSampler(new DirichletBoundaryHandler(3));

        [Test]
        public void Convolute_test () {
            Assert.That(dirichletSampler.Convolute(array), Is.EqualTo(digitalSampler.Convolute(array)).Within(1E-15));
        }

        [Test]
        public void Downsample_test () {
            Assert.That(dirichletSampler.Downsample(array, 2, 0), Is.EqualTo(digitalSampler.Downsample(array, 2, 0)).Within(1E-15));
        }

        [Test]
        public void Upsample_test () {
            Assert.That(dirichletSampler.Upsample(array, 2, 0), Is.EqualTo(digitalSampler.Upsample(array, 2, 0)).Within(1E-15));
        }

        private static readonly Random random = new Random();

    }

}
