namespace Customer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class that describes a single customer by his name, contact phone and revenue.
    /// </summary>
    public class Customer : IFormattable
    {
        #region Private fields

        /// <summary>
        /// Customer's name.
        /// </summary>
        private string name;

        /// <summary>
        /// Customer's contact phone.
        /// </summary>
        private string contactPhone;

        /// <summary>
        /// Customer's revenue.
        /// </summary>
        private decimal revenue;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        /// <param name="name">
        /// Customer's name.
        /// </param>
        /// <param name="contactPhone">
        /// Customer's contact phone.
        /// </param>
        /// <param name="revenue">
        /// Customer's revenue.
        /// </param>
        public Customer(string name, string contactPhone, decimal revenue)
        {
            this.Name = name;
            this.ContactPhone = contactPhone;
            this.Revenue = revenue;
        }

        #endregion

        #region Propereties

        /// <summary>
        /// Gets or sets current customer's name.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if passed name is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if passed name doesn't satisfy given rules:
        /// 1. Name can be only in Russian or English.
        /// 2. Name must contain at least two words. 
        /// 3. All words separated by one whitespace.
        /// 4. Only first letter of each word must be uppercase.
        /// </exception>
        public string Name
        {
            get => this.name;

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                const string PATTERN_ENG = @"^([A-Z][a-z]* )+[A-Z][a-z]*$";
                var regex = new Regex(PATTERN_ENG);
                if (regex.IsMatch(value))
                {
                    this.name = value;
                    return;
                }

                const string PATTERN_RUS = @"^([А-ЯЁ][а-яё]* )+[А-ЯЁ][а-яё]*$";
                regex = new Regex(PATTERN_RUS);
                if (regex.IsMatch(value))
                {
                    this.name = value;
                    return;
                }

                throw new ArgumentException("Name must be in English or in Russian, contain at least two words separated by "
                                            + "single whitespace and only first letter of each name must be uppercase.");
            }
        }

        /// <summary>
        /// Gets or sets current customer's contact phone.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown if passed number is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if passed number not in form "+X (XXX) XXX-XXXX".
        /// </exception>
        public string ContactPhone
        {
            get => this.contactPhone;

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                var pattern = @"^\+\d \(\d{3}\) \d{3}-\d{4}";
                var regex = new Regex(pattern);
                if (!regex.IsMatch(value))
                {
                    throw new ArgumentException("Number must be in form \"+X (XXX) XXX-XXXX\".");
                }

                this.contactPhone = value;
            }
        }

        /// <summary>
        /// Gets or sets current customer's revenue.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if passed value is negative or contain more than 2 decimal places.
        /// </exception>
        public decimal Revenue
        {
            get => this.revenue;

            set
            {
                if (HaveMoreThanTwoDecimalPlaces(value) || value < 0)
                {
                    throw new ArgumentException($"{nameof(Revenue)} must be a non-negative number and have at most two decimal places!");
                }

                this.revenue = value;
            } 
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns string representation of current customer in format "Name, phone, revenue".
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// Current customer string representation.
        /// </returns>
        public override string ToString()
        {
            return this.ToString("G", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Creates string representation of current customer 
        /// that corresponds to passed format.
        /// Class supports four format specifiers:
        /// N - name.
        /// P - contact phone.
        /// R - revenue.
        /// G - general.
        /// Specifiers N, P and R may be combined in any order.
        /// Format string can not contain duplicate specifiers.
        /// G can not be combined with anything.
        /// </summary>
        /// <param name="format">
        /// Format string.
        /// </param>
        /// <param name="formatProvider">
        /// Format provider.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// String representation of current customer 
        /// that corresponds to passed format string.
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown if given format string doesn't fit format rules.
        /// </exception>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CheckInput();

            if (format == "G" || format == "NPR")
            {
                return string.Join(", ", this.Name, this.ContactPhone, this.Revenue.ToString(formatProvider));
            }

            var specifiers = new Dictionary<string, string>
            {
                { "N", this.Name },
                { "P", this.ContactPhone },
                { "R", this.Revenue.ToString(formatProvider) }
            };

            return BuildStringRepresentation(format, specifiers); 

            void CheckInput()
            {
                if (string.IsNullOrEmpty(format))
                {
                    format = "G";
                }

                if (formatProvider == null)
                {
                    formatProvider = CultureInfo.CurrentCulture;
                }
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Builds current customer's string representation 
        /// based on provided format string and specifiers mapping.
        /// </summary>
        /// <param name="format">
        /// Format string.
        /// </param>
        /// <param name="specifiers">
        /// Specifiers mapping "specifier -> string".
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// String representation of current customer 
        /// that corresponds to passed format string.
        /// </returns>
        /// <exception cref="FormatException">
        /// Thrown if given format string doesn't fit format rules.
        /// </exception>
        private static string BuildStringRepresentation(string format, Dictionary<string, string> specifiers)
        {
            const string SEPARATOR = ", ";
            var resultStr = string.Empty;
            for (var index = 0; index < format.Length; index++)
            {
                var specifier = format[index].ToString();
                if (!specifiers.ContainsKey(specifier))
                {
                    throw new FormatException($"The \"{format}\" format string is not supported.");
                }

                resultStr += specifiers[specifier];
                specifiers.Remove(specifier);

                if (index != format.Length - 1)
                {
                    resultStr += SEPARATOR;
                }
            }

            return resultStr;
        }

        /// <summary>
        /// Checks if passed decimal value have more than two decimal places.
        /// </summary>
        /// <param name="value">
        /// Value that needs to be checked.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True of number have more than two decimal places, false otherwise.
        /// </returns>
        private static bool HaveMoreThanTwoDecimalPlaces(decimal value)
        {
            decimal mustBeInt = value * 100;
            return decimal.Floor(mustBeInt) != mustBeInt;
        } 

        #endregion
    }
}
