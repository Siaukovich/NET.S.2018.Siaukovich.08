namespace Customer.Tests
{
    using System;

    using NUnit.Framework;

    /// <summary>
    /// Test class for Customer class.
    /// </summary>
    [TestFixture]
    public class CustomerTests
    {
        [TestCase("jonh doe")]
        [TestCase("John")]
        [TestCase("John G123")]
        [TestCase("")]
        public void CustomersCtor_InvalidName_ThrowsArgumentException(string invalidName) =>
            Assert.Throws<ArgumentException>(() => new Customer(invalidName, "+1 (425) 555-0100", 1000));

        [Test]
        public void CustomersCtor_NullName_ThrowsArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => new Customer(null, "+1 (425) 555-0100", 1000));

        [TestCase("John Doe")]
        [TestCase("John Alex Doe")]
        [TestCase("John J K Doe")]
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
    }
}
