using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Dynamics365.UIAutomation.Browser.Logs
{
    public static class StringFormater
    {
        /// <summary>
        /// Return the object as string formatted to be printed in plugin trace or any other log
        /// </summary>
        public static string Format(this object value, string format = null)
        {
            if (value == null)
                return "null";

            switch (value)
            {
                case string s: return s;
                case bool b: return b ? "true" : "false";
                case double d: return d.ToString(format ?? "N");
                case decimal @decimal: return @decimal.ToString(format ?? "N");
                case DateTime time: return time.ToString(format ?? "yyyy-MM-dd HH:mm:ss");
                case Enum enm: return enm.ToString(format ?? "F");
                default:
                    return value.ToString();
            }
        }

        public static string Format<T>(this T[] array, string separator = ", ")
        {
            IEnumerable<string> formated = array.Select(item => Format(item));
            string joined = string.Join(separator, formated);

            int count = array.Length;
            string itemType = typeof(T).Name;

            var result = separator.Contains(Environment.NewLine)
                ? $"Collection: {{{separator}Count: {count}{separator}Items ({itemType}):{separator}{joined}{separator}}}"
                : $"Collection: {{ Count: {count}{separator}Items ({itemType}): {joined} }}";
            return result;
        }
        
        public static string Format(this Exception input)
        {
            if (input == null)
                return "null";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(input.ToString());
            input = input.InnerException;
            int deep = 1;
            while (input != null)
            {
                sb.AppendLine($"--- InnerException (deep:{deep++}) ---");
                sb.AppendLine(input.ToString());
                input = input.InnerException;
            }

            return sb.ToString();
        }
       
        public static string FormatCollection<T>(this IEnumerable<T> collection, string separator = ", ")
        {
            if (collection == null)
                return "null";

            var array = collection as T[] ?? collection.ToArray();

            var result = Format(array, separator);
            return result;
        }
    }
}