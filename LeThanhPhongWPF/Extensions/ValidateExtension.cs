using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeThanhPhongWPF.Extensions
{
    public static class ValidateExtension
    {
        public static void CheckYear(this string input, string fieldName)
        {
            // Define a regular expression pattern for a 4-digit year (e.g., "2023")
            string yearPattern = @"^\d{4}$";

            // Use Regex.IsMatch to check if the input string matches the year pattern
            if(! Regex.IsMatch(input, yearPattern))
            {
                throw new Exception($"'{fieldName}' is not a valid year");
            }
        }
        // Check if the string represents a float number
        public static void CheckFloat(this string input, string fieldName)
        {
            if (!float.TryParse(input, out _))
            {
                throw new Exception($"'{fieldName}' is not a valid float number.");
            }
        }

        // Check if the string represents an integer number (with optional negative allowance)
        public static void CheckInt(this string input, string fieldName, bool allowNegative = false)
        {
            if (allowNegative)
            {
                if (!int.TryParse(input, out _))
                {
                    throw new Exception($"'{fieldName}' is not a valid integer number.");
                }
            }
            else
            {
                if (!int.TryParse(input, out int result) || result < 0)
                {
                    throw new Exception($"'{fieldName}' is not a valid non-negative integer number.");
                }
            }
        }

        // Check if the string represents an integer within a specified range
        public static void CheckIntInRange(this string input, string fieldName, int minValue, int maxValue)
        {
            if (!int.TryParse(input, out int result) || result < minValue || result > maxValue)
            {
                throw new Exception($"'{fieldName}' is not a valid integer within the range {minValue} to {maxValue}.");
            }
        }

        // Check if the string represents a valid telephone number (simple example)
        public static void CheckTelephone(this string input, string fieldName)
        {
            // Customize the regular expression as needed for your telephone format
            string telephonePattern = @"^\d{10}$"; // Example: 10-digit number
            if (!Regex.IsMatch(input, telephonePattern))
            {
                throw new Exception($"'{fieldName}' is not a valid telephone number.");
            }
        }

        // Check if the string represents a valid email address
        public static void CheckEmail(this string input, string fieldName)
        {
            string emailPattern = @"^\S+@\S+\.\S+$"; // Example: Basic email pattern
            if (!Regex.IsMatch(input, emailPattern))
            {
                throw new Exception($"'{fieldName}' is not a valid email address.");
            }
        }

        // Check if the string represents a valid password (at least 8 characters)
        public static void CheckPassword(this string input, string fieldName)
        {
            if (string.IsNullOrEmpty(input) || input.Length < 8)
            {
                throw new Exception($"'{fieldName}' is not a valid password (must be at least 8 characters).");
            }
        }
        public static void CheckNotEmpty(this string input, string fieldName)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new Exception($"'{fieldName}' should not be empty.");
            }
        }
        public static void CheckObjectNotEmpty(this object input, string fieldName)
        {
            if (input is null)
            {
                throw new Exception($"'{fieldName}' should not be empty.");
            }
        }
        public static bool IsOver18YearsOld(this DateTime? date)
        {
            DateTime today = DateTime.Today;
            DateTime eighteenYearsAgo = today.AddYears(-18);
            return date <= eighteenYearsAgo;
        }
        public static bool IsNotExceedingCurrentTime(this DateTime? date)
        {
            DateTime currentDateTime = DateTime.Now;
            return date <= currentDateTime;
        }
    }
}
