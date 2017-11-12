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
}
