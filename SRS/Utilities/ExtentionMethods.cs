using System;
using System.Configuration;
using System.Linq;

namespace SRS
{
    public static class ExtentionMethods
    {

        public static string GetEmailSetting(this string setting)
        {
            return ConfigurationManager.AppSettings[setting];
        }
        public static bool In(this string source, string csv)
        {
            var list = csv.Split(',');
            return list.Contains(source, StringComparer.OrdinalIgnoreCase);
        }
        public static string Prepend(this string x, string pre)
        {
            return pre + x;

        }
        public static DateTime GetDateTime(this DateTime date, string arg)
        {
            if (string.IsNullOrWhiteSpace(arg)) return DateTime.Now;
            DateTime dt;
            var isValidDate = DateTime.TryParse(arg, out dt);
            if (isValidDate)
                return dt;
            Console.WriteLine("Invalid DATE");
            throw new ArgumentException("Invalid date argument exception");
        }
    }
}
