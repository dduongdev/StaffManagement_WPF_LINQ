using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SM_WPF_LINQ.Utils
{
    public class Validator
    {
        public static bool IsPhone(string input)
        {
            string pattern = @"^0\d{9,10}$";
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsNumeric(string input)
        {
            return double.TryParse(input, out _);
        }

        public static bool IsNumericInteger(string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
