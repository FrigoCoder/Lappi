using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    public class IntegerUtilTest {

        [Test]
        public void Reverse_uint_test () {
            Assert.That(0x12345678u.Reverse(), Is.EqualTo(0x1E6A2C48u));
            Assert.That(0x1E6A2C48u.Reverse(), Is.EqualTo(0x12345678u));
            Assert.That(0x00000001u.Reverse(), Is.EqualTo(0x80000000u));
            Assert.That(0x80000000u.Reverse(), Is.EqualTo(0x00000001u));
        }

    }

}
