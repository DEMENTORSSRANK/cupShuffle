using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtensions
    {
        public static void MixList<TT>(ICollection<TT> list)
        {
            var r = new System.Random();

            var mixedList = new SortedList<int, TT>();

            foreach (var item in list)
                mixedList.Add(r.Next(), item);

            list.Clear();

            for (var i = 0; i < mixedList.Count; i++)
            {
                list.Add(mixedList.Values[i]);
            }
        }
    }
}