using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrap.Blazor.Extensions;
internal static class DictionaryExtensions
{
    internal static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        where TKey : notnull
    {
        if (dic.ContainsKey(key))
            dic[key] = value;
        else
            dic.Add(key, value);
    }

    internal static Dictionary<string, object> RemoveAttribute(this Dictionary<string, object> dic, string attribute)
    {
        if (dic.ContainsKey(attribute))
            dic.Remove(attribute);

        return dic;
    }
}
