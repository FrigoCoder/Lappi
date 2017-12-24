using Lappi.Util;

using NUnit.Framework;

namespace LappiTest.Util {

    public class IntegersTest {

        [Test]
        public void ToUnsignedInt_test () {
            Assert.That(0x00000000.ToUnsigned(), Is.EqualTo(0x00000000u));
            Assert.That(0x00000001.ToUnsigned(), Is.EqualTo(0x00000001u));
            Assert.That(0x12345678.ToUnsigned(), Is.EqualTo(0x12345678u));
            Assert.That(int.MinValue.ToUnsigned(), Is.EqualTo(0x80000000u));
            Assert.That((-1).ToUnsigned(), Is.EqualTo(0xffffffffu));
        }

        [Test]
        public void ToSignedInt_test () {
            Assert.That(0x00000000u.ToSigned(), Is.EqualTo(0x00000000));
            Assert.That(0x00000001u.ToSigned(), Is.EqualTo(0x00000001));
            Assert.That(0x12345678u.ToSigned(), Is.EqualTo(0x12345678));
            Assert.That(0x80000000u.ToSigned(), Is.EqualTo(int.MinValue));
            Assert.That(0xffffffffu.ToSigned(), Is.EqualTo(-1));
        }

        [Test]
        public void Reverse_int_test () {
            Assert.That(0x00000000.Reverse(), Is.EqualTo(0x00000000));
            Assert.That(0x00000001.Reverse(), Is.EqualTo(int.MinValue));
            Assert.That(0x12345678.Reverse(), Is.EqualTo(0x1E6A2C48));
            Assert.That(0x1E6A2C48.Reverse(), Is.EqualTo(0x12345678));
            Assert.That(int.MinValue.Reverse(), Is.EqualTo(0x00000001));
            Assert.That((-1).Reverse(), Is.EqualTo(-1));
        }

        [Test]
        public void Reverse_uint_test () {
            Assert.That(0x00000000u.Reverse(), Is.EqualTo(0x00000000u));
            Assert.That(0x00000001u.Reverse(), Is.EqualTo(0x80000000u));
            Assert.That(0x12345678u.Reverse(), Is.EqualTo(0x1E6A2C48u));
            Assert.That(0x1E6A2C48u.Reverse(), Is.EqualTo(0x12345678u));
            Assert.That(0x80000000u.Reverse(), Is.EqualTo(0x00000001u));
            Assert.That(0xffffffffu.Reverse(), Is.EqualTo(0xffffffffu));
        }

    }

}
