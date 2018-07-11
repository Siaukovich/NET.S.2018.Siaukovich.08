namespace Customer.Tests
{
    using System;
    using System.Globalization;

    using NUnit.Framework;

    /// <summary>
    /// Test class for Customer class.
    /// </summary>
    [TestFixture]
    public class CustomerTests
    {
        #region ToString tests

        [TestCase("G", ExpectedResult = "John Doe, +1 (425) 555-0100, 1000.52")]
        [TestCase("NPR", ExpectedResult = "John Doe, +1 (425) 555-0100, 1000.52")]
        [TestCase("NP", ExpectedResult = "John Doe, +1 (425) 555-0100")]
        [TestCase("NR", ExpectedResult = "John Doe, 1000.52")]
        [TestCase("RN", ExpectedResult = "1000.52, John Doe")]
        [TestCase("PN", ExpectedResult = "+1 (425) 555-0100, John Doe")]
        [TestCase("PR", ExpectedResult = "+1 (425) 555-0100, 1000.52")]
        [TestCase("RP", ExpectedResult = "1000.52, +1 (425) 555-0100")]
        public string CustomersToString_ValidInputInvariantCulture_ValidString(string format)
        {
            var c = new Customer("John Doe", "+1 (425) 555-0100", 1000.52m);
            return c.ToString(format, CultureInfo.InvariantCulture);
        }

        [TestCase("G", ExpectedResult = "John Doe, +1 (425) 555-0100, 1000,52")]
        [TestCase("NPR", ExpectedResult = "John Doe, +1 (425) 555-0100, 1000,52")]
        [TestCase("RN", ExpectedResult = "1000,52, John Doe")]
        public string CustomersToString_ValidInputRussianCulture_ValidString(string format)
        {
            var c = new Customer("John Doe", "+1 (425) 555-0100", 1000.52m);
            return c.ToString(format, new CultureInfo("ru-ru"));
        }

        #endregion

        #region Propereties tests

        [TestCase("jonh doe")]
        [TestCase("John")]
        [TestCase("John G123")]
        [TestCase("")]
        [TestCase("Севкович 1")]
        [TestCase("Севкович 1Кирилл1")]
        public void CustomersCtor_InvalidName_ThrowsArgumentException(string invalidName) =>
            Assert.Throws<ArgumentException>(() => new Customer(invalidName, "+1 (425) 555-0100", 1000));

        [Test]
        public void CustomersCtor_NullName_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => new Customer(null, "+1 (425) 555-0100", 1000));

        [TestCase("John Doe")]
        [TestCase("John Alex Doe")]
        [TestCase("John J K Doe")]
        [TestCase("Севкович Кирилл")]
        [TestCase("Севкович Кирилл Александрович")]
        public void CustomersCtor_ValidName_NoException(string validName)
        {
            new Customer(validName, "+1 (425) 555-0100", 1000);
        }

        [TestCase("123321123")]
        [TestCase("+2134123423")]
        [TestCase("+1 123 123 1234")]
        [TestCase("+1 123 1231234")]
        [TestCase("+1 123 123-1234")]
        public void CustomersCtor_InvalidNumber_ThrowsArgumentException(string invalidNumber) =>
            Assert.Throws<ArgumentException>(() => new Customer("John Doe", invalidNumber, 1000));

        [Test]
        public void CustomersCtor_NullNumber_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => new Customer("John Doe", null, 1000));

        [Test]
        public void CustomersCtor_ValidNumber_NoException()
        {
            new Customer("John Doe", "+1 (425) 555-0100", 1000);
        }

        [TestCase(123.456)]
        [TestCase(-123)]
        [TestCase(0.123)]
        public void CustomersCtor_InvalidRevenue_ThrowsArgumentException(double invalidRevenue)
        {
            Assert.Throws<ArgumentException>(() => new Customer("John Doe", "+1 (425) 555-0100", new decimal(invalidRevenue)));
        }

        [TestCase(123.12)]
        [TestCase(0)]
        [TestCase(10000)]
        [TestCase(100.1)]
        public void CustomersCtor_ValidRevenue_NoException(double validRevenue)
        {
            new Customer("John Doe", "+1 (425) 555-0100", 1000);
        }

        #endregion
    }
}
