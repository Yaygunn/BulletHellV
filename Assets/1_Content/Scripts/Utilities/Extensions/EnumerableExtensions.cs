using System;
using System.Collections.Generic;

namespace BH.Utilities.Extensions
{
    public static class EnumerableExtensions 
    {
        /// <summary>
        /// Performs an action on each element in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to iterate over.</param>
        /// <param name="action">The action to perform on each element.</param>    
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action) 
        {
            foreach (T item in sequence) 
            {
                action(item);
            }
        }
    }
}