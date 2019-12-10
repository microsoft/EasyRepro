using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics365.UIAutomation.Browser.Extensions
{
    public static class DictionaryExtensions
    {
        
        public static Dictionary<T1, T2> Merge<T1, T2>(this Dictionary<T1, T2> dict, params IDictionary<T1, T2>[] others)
        {
            var result = new Dictionary<T1, T2>(dict, dict.Comparer);
            foreach (IDictionary<T1, T2> src in (new List<IDictionary<T1, T2>> { dict }).Concat(others))
            {
                foreach (KeyValuePair<T1, T2> p in src)
                {
                    if (result.ContainsKey(p.Key))
                        result[p.Key] = p.Value;
                    else
                        result.Add(p.Key, p.Value);
                }
            }
            return result;
        }

    }
}
