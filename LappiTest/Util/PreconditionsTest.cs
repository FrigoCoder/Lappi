using System;

using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    public class PreconditionsTest {

        [Test]
        public void Require_passes () {
            Preconditions.Require(true);
        }

        [Test]
        public void Require_fails () {
            Assert.That(() => Preconditions.Require(false), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Require_generic_passes () {
            Preconditions.Require<ArgumentException>(true);
        }

        [Test]
        public void Require_generic_fails () {
            Assert.That(() => Preconditions.Require<ArgumentException>(false), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Require_parameterized_passes () {
            Preconditions.Require(true, typeof(ArgumentException));
        }

        [Test]
        public void Require_parameterized_fails () {
            Assert.That(() => Preconditions.Require(false, typeof(ArgumentException)), Throws.TypeOf<ArgumentException>());
        }

    }

}
