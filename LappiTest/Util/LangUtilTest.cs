﻿using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    public class LangUtilTest {

        [Test]
        public void Ref_swaps_two_elements () {
            int x = 1;
            int y = 2;
            LangUtil.Swap(ref x, ref y);
            Assert.That(x, Is.EqualTo(2));
            Assert.That(y, Is.EqualTo(1));
        }

    }

}
