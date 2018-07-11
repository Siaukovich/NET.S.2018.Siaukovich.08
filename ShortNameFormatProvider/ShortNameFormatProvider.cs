namespace ShortNameFormatProvider
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Customer;

    public class ShortNameFormatProvider : IFormatProvider, ICustomFormatter
    {
        private IFormatProvider parent;

        public ShortNameFormatProvider(IFormatProvider parent)
        {
            this.parent = parent;
        }

        public ShortNameFormatProvider() : this(CultureInfo.InvariantCulture) { }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // НИЖЕ ОЧЕНЬ ПЛОХОЙ КОД
            //
            // НО ОН РАБОТАЕТ
            //
            // ДНЁМ Я ЕГО ОТРЕФАКТОРЮ
            //
            // ЧЕСТНО-ЧЕСТНО
            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }

            format = format.ToUpperInvariant();

            if (arg == null || !format.Contains("S"))
            {
                return string.Format(this.parent, "{0:" + format + "}", arg);
            }

            if (arg.GetType() != typeof(Customer))
            {
                throw new FormatException($"{nameof(arg)} must have type Customer.");
            }

            if ((format.Contains("S") && format.Contains("N")) || format.Count(x => x == 'S') > 1)
            {
                throw new FormatException("Format string cannot contain two S or S and N at the same time.");
            }

            string namelessFormat = format.Replace("S", string.Empty);

            var p = new List<string>();
            var customer = (Customer)arg;
            if (namelessFormat != string.Empty)
            {
                const string SEPARATOR = ", ";
                string[] parts = customer.ToString(namelessFormat, this.parent).Split(new[] { SEPARATOR }, StringSplitOptions.None);
                p.AddRange(parts);
            }

            int index = format.IndexOf("S", StringComparison.Ordinal);
            string name = customer.Name;
            string[] nameParts = name.Split();
            for (int i = 0; i < nameParts.Length - 1; i++)
            {
                nameParts[i] = nameParts[i][0] + ".";
            }

            name = string.Join(" ", nameParts);
            p.Insert(index, name);
            return string.Join(", ", p);
        }
    }
}
