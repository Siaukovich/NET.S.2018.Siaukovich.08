namespace ShortNameFormatProvider
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Customer;

    /// <summary>
    /// Custom format provider to Customer class.
    /// Adds letter "S" to format specifiers, which stands for
    /// "short name".
    /// </summary>
    public class ShortNameFormatProvider : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// The parent format provider.
        /// </summary>
        private IFormatProvider parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortNameFormatProvider"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent format provider.
        /// </param>
        public ShortNameFormatProvider(IFormatProvider parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortNameFormatProvider"/> class.
        /// </summary>
        public ShortNameFormatProvider() : this(CultureInfo.InvariantCulture) { }

        /// <summary>
        /// Get's format.
        /// </summary>
        /// <param name="formatType">
        /// The format type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Formats string using provided specifiers.
        /// </summary>
        /// <param name="format">
        /// Format string.
        /// </param>
        /// <param name="arg">
        /// Argument that will be represented as string.
        /// </param>
        /// <param name="formatProvider">
        /// Format provider.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// String representation of given object.
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown if argument type is not Customer,
        /// format string contains N and S at the same time,
        /// format string contain more than one letter S.
        /// </exception>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }

            format = format.ToUpperInvariant();

            if (arg == null || !format.Contains("S"))
            {
                return string.Format(this.parent, "{0:" + format + "}", arg);
            }

            ThrowForInvalidParameters();

            const string SEPARATOR = ", ";
            var customer = (Customer)arg;
            List<string> parts = this.GetStringRepresentationParts(format, customer, SEPARATOR);

            var shortName = GetShortName(customer);

            int index = format.IndexOf("S", StringComparison.Ordinal);
            parts.Insert(index, shortName);

            return string.Join(SEPARATOR, parts);

            void ThrowForInvalidParameters()
            {
                if (arg.GetType() != typeof(Customer))
                {
                    throw new FormatException($"{nameof(arg)} must have type Customer.");
                }

                if (format.Contains("S") && format.Contains("N"))
                {
                    throw new FormatException("Format string cannot contain S and N at the same time.");
                }

                if (format.Count(x => x == 'S') > 1)
                {
                    throw new FormatException("Format string cannot contain more than a single S at the same time.");
                }
            }
        }

        /// <summary>
        /// Get's given customer's short name.
        /// </summary>
        /// <param name="customer">
        /// Customer.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// Customer's short name.
        /// </returns>
        private static string GetShortName(Customer customer)
        {
            string[] nameParts = customer.Name.Split();
            for (int i = 0; i < nameParts.Length - 1; i++)
            {
                nameParts[i] = nameParts[i][0] + ".";
            }

            return string.Join(" ", nameParts);
        }

        /// <summary>
        /// Returns passed customer's string representation's parts as List.
        /// </summary>
        /// <param name="format">
        /// Format string.
        /// </param>
        /// <param name="customer">
        /// Customer object.
        /// </param>
        /// <param name="separator">
        /// Separator between parts in customer's string representation.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// List of customer's string representation parts.
        /// </returns>
        private List<string> GetStringRepresentationParts(string format, Customer customer, string separator)
        {
            var reprParts = new List<string>();
            string namelessFormat = format.Replace("S", string.Empty);
            if (namelessFormat == string.Empty)
            {
                return reprParts;
            }

            string[] parts = customer.ToString(namelessFormat, this.parent).Split(new[] { separator }, StringSplitOptions.None);
            reprParts.AddRange(parts);

            return reprParts;
        }
    }
}
