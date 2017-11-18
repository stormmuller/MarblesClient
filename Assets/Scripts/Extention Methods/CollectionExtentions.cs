using System.Collections.Generic;

namespace Marbles.ExtentionMethods
{
    public static class HashSetExtentions
    {
        public static HashSet<T> AddRange<T>(this HashSet<T> thisCollection, IEnumerable<T> otherCollection)
        {
            foreach (var item in otherCollection)
            {
                thisCollection.Add(item);
            }

            return thisCollection;
        }
    }

    public static class DictionaryExtentions
    {
        public static bool AllValuesEqualTo<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue objectToCompareAgainst)
        {
            foreach (var value in dictionary.Values)
            {
                if (!value.Equals(objectToCompareAgainst))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
