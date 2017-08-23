// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Microsoft.Dynamics365.UIAutomation.Browser
{
    public sealed class DynamicJsonConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Type> SupportedTypes => new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) }));

        #region Nested type: DynamicJsonObject

        private sealed class DynamicJsonObject : DynamicObject
        {
            private readonly IDictionary<string, object> _dictionary;

            public DynamicJsonObject(IDictionary<string, object> dictionary)
            {
                if (dictionary == null)
                    throw new ArgumentNullException(nameof(dictionary));

                _dictionary = dictionary;
            }

            public override string ToString()
            {
                var sb = new StringBuilder("{");

                ToString(sb);

                return sb.ToString();
            }

            private void ToString(StringBuilder sb)
            {
                var firstInDictionary = true;
                foreach (var pair in _dictionary)
                {
                    if (!firstInDictionary)
                    {
                        sb.Append(",");
                    }

                    firstInDictionary = false;

                    var value = pair.Value;
                    var name = pair.Key;

                    if (value is string)
                    {
                        sb.AppendFormat("{0}:\"{1}\"", name, value);
                    }
                    else if (value is IDictionary<string, object>)
                    {
                        new DynamicJsonObject((IDictionary<string, object>)value).ToString(sb);
                    }
                    else if (value is ArrayList)
                    {
                        sb.Append(name + ":[");

                        var firstInArray = true;

                        foreach (var arrayValue in (ArrayList)value)
                        {
                            if (!firstInArray)
                                sb.Append(",");

                            firstInArray = false;

                            var objects = arrayValue as IDictionary<string, object>;

                            if (objects != null)
                                new DynamicJsonObject(objects).ToString(sb);
                            else if (arrayValue is string)
                                sb.AppendFormat("\"{0}\"", arrayValue);
                            else
                                sb.AppendFormat("{0}", arrayValue);
                        }

                        sb.Append("]");
                    }
                    else
                    {
                        sb.AppendFormat("{0}:{1}", name, value);
                    }
                }
                sb.Append("}");
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (!_dictionary.TryGetValue(binder.Name, out result))
                {
                    return true;
                }

                result = WrapResultObject(result);

                return true;
            }

            public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
            {
                if (indexes.Length != 1 || indexes[0] == null)
                {
                    return base.TryGetIndex(binder, indexes, out result);
                }

                if (!_dictionary.TryGetValue(indexes[0].ToString(), out result))
                {
                    return true;
                }

                result = WrapResultObject(result);

                return true;
            }

            private static object WrapResultObject(object result)
            {
                var dictionary = result as IDictionary<string, object>;

                if (dictionary != null)
                    return new DynamicJsonObject(dictionary);

                var arrayList = result as ArrayList;

                if (arrayList != null && arrayList.Count > 0)
                {
                    return arrayList[0] is IDictionary<string, object>
                        ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x)))
                        : new List<object>(arrayList.Cast<object>());
                }

                return result.ToString();
            }
        }

        #endregion Nested type: DynamicJsonObject
    }
}