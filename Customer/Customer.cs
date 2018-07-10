namespace Customer
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class that describes a single customer by his name, contact phone and revenue.
    /// </summary>
    public class Customer
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

        #region Propereties

        /// <summary>
        /// Gets or sets current customer's name.
        /// </summary>
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
        /// <exception cref="ArgumentException">
        /// Thrown if number not in form "+X (XXX) XXX-XXXX".
        /// </exception>
        public string ContactPhone
        {
            get => this.contactPhone;

            set
            {
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

        #region Helpers

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
        private static bool HaveMoreThanTwoDecimalPlaces(decimal value) => value != (100 * value);

        #endregion
    }
}
