namespace ShortNameFormatProvider.Tests
{
    using System;

    using Customer;
    using NUnit.Framework;

    /// <summary>
    /// Class for testing ShortNameFormatProvider class.
    /// </summary>
    [TestFixture]
    public class ShortNameFormatProviderTests
    {
        [TestCase("John Doe", "SP", ExpectedResult = "J. Doe, +1 (425) 555-0100")]
        [TestCase("John Doe", "sPR", ExpectedResult = "J. Doe, +1 (425) 555-0100, 1000.52")]
        [TestCase("John Doe", "rs", ExpectedResult = "1000.52, J. Doe")]
        [TestCase("John Clark Robin Doe", "S", ExpectedResult = "J. C. R. Doe")]
        [TestCase("Кирилл Александрович Севкович", "S", ExpectedResult = "К. А. Севкович")]
        public string CustomersToString_ValidInputInvariantCulture_ValidString(string name, string format)
        {
            var c = new Customer(name, "+1 (425) 555-0100", 1000.52m);
            var customFormatProvider = new ShortNameFormatProvider();
            return string.Format(customFormatProvider, $"{{0:{format}}}", c);
        }

        [TestCase("SS")]
        [TestCase("NS")]
        [TestCase("NSR")]
        public void CustomersToString_NSInFormat_ThrowsFormatException(string format)
        {
            var c = new Customer("John Doe", "+1 (425) 555-0100", 1000.52m);
            var customFormatProvider = new ShortNameFormatProvider();
            Assert.Throws<FormatException>(() => string.Format(customFormatProvider, $"{{0:{format}}}", c));
        }
    }
}
