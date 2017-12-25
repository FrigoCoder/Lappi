using System;

using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    public class PreconditionsTest {

        [Test]
        public void Require_passes () {
            Preconditions.Require<ArgumentException>(true);
        }

        [Test]
        public void Require_fails () {
            Assert.That(() => Preconditions.Require<ArgumentException>(false), Throws.TypeOf<ArgumentException>());
        }

    }

}
