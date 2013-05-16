using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonClasses.Helpers
{
    public static class TextHelper
    {
        public static string FirstToLower(this string str)
        {
            if (String.IsNullOrEmpty(str)) return str;
            if (str.Length == 1) return str.ToLower();
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        public static void AddHeaderWarning(this StringBuilder sb)
        {
            sb.AppendLine("//=========================================================================");
            sb.AppendLine("// THIS IS AUTOMATICALLY GENERATED FILE! PLEASE DON'T CHANGE IT MANUALLY!");
            sb.AppendLine("//=========================================================================");
        }

        public static bool InCommaList(this string str, string commaList)
        {
            return ("," + commaList + ",").Contains("," + str + ",");
        }
    }
}
