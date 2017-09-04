using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GW2MH.Core.Util
{
    internal static class Ext
    {

        public static string RemoveWhiteSpaces(this string s)
        {
            return Regex.Replace(s, @"\s+", string.Empty);
        }

        public static byte[] ToArray(this string s)
        {
            return Enumerable.Range(0, s.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(s.Substring(x, 2), 16))
                     .ToArray();
        }

    }
}