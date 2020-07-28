using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMDesktopApp.Helpers
{
    public static class Extensions
    {
        public static void RemoveByValue<T, T1>(this Dictionary<T, T1> src, T1 Value)
        {
            foreach (var item in src.Where(kvp => kvp.Value.Equals(Value)).ToList())
            {
                src.Remove(item.Key);
            }
        }
    }
}
