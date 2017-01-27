using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bits;

namespace Test
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void ConvertDecimalToBinary()
        {
            int dec;
            string expected, binary;

            dec = 0;
            expected = "0";
            binary = Program.ConvertDecimalToBinary(dec);
            Assert.IsTrue(binary.Equals(expected));

            dec = 12;
            expected = "1100";
            binary = Program.ConvertDecimalToBinary(dec);
            Assert.IsTrue(binary.Equals(expected));

            dec = 249;
            expected = "11111001";
            binary = Program.ConvertDecimalToBinary(dec);
            Assert.IsTrue(binary.Equals(expected));
        }

        [TestMethod]
        public void ConvertToBinary()
        {
            string value, expected, binary;

            value = "0";
            expected = "0";
            binary = Program.ConvertToBinary(value);
            Assert.IsTrue(binary.Equals(expected));

            value = "G";
            expected = "1000111";
            binary = Program.ConvertToBinary(value);
            Assert.IsTrue(binary.Equals(expected));

            value = "513";
            expected = "1000000001";
            binary = Program.ConvertToBinary(value);
            Assert.IsTrue(binary.Equals(expected));
        }

        [TestMethod]
        public void ConvertDecimalToHex()
        {
            int dec;
            string expected, hex;

            dec = 0;
            expected = "0x0";
            hex = Program.ConvertDecimalToHexadecimal(dec);
            Assert.IsTrue(hex.Equals(expected));

            dec = 1128;
            expected = "0x468";
            hex = Program.ConvertDecimalToHexadecimal(dec);
            Assert.IsTrue(hex.Equals(expected));

            dec = 12;
            expected = "0xC";
            hex = Program.ConvertDecimalToHexadecimal(dec);
            Assert.IsTrue(hex.Equals(expected));

            dec = 24;
            expected = "0x18";
            hex = Program.ConvertDecimalToHexadecimal(dec);
            Assert.IsTrue(hex.Equals(expected));

            dec = 255;
            expected = "0xFF";
            hex = Program.ConvertDecimalToHexadecimal(dec);
            Assert.IsTrue(hex.Equals(expected));
        }

        [TestMethod]
        public void ConvertToHex()
        {
            string value, expected, hex;

            value = "0";
            expected = "0x0";
            hex = Program.ConvertToHexadecimal(value);
            Assert.IsTrue(hex.Equals(expected));

            value = "A";
            expected = "0x41";
            hex = Program.ConvertToHexadecimal(value);
            Assert.IsTrue(hex.Equals(expected));

            value = "123";
            expected = "0x7B";
            hex = Program.ConvertToHexadecimal(value);
            Assert.IsTrue(hex.Equals(expected));
        }
    }
}
