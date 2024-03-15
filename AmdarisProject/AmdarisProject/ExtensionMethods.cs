using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmdarisProject
{
    public static class ExtensionMethods
    {
        public static void PrintEachElementOnANewLine<T>(this IEnumerable<T> enumerable)
        {
            foreach (T element in enumerable)
            {
                Print print = delegate (string? s)
                {
                    Console.WriteLine(s);
                };
                print(element?.ToString());
            }
        }
    }
}
