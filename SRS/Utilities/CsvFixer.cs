using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SRS.Utilities
{
    internal static class CsvFixer
    {
        public static string FixRecord(string record)
        {
            foreach (Match o in new Regex(@"[ ]"".+?""[ ]").Matches(record))
            {
                record = record.Replace(o.Value, o.Value.Replace('"', '\''));
            }
            return record;
        }
    }
}
