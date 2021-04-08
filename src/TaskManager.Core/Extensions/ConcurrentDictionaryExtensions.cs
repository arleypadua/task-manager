using System;
using System.Collections.Concurrent;

namespace TaskManager.Core.Extensions
{
    public static class ConcurrentDictionaryExtensions
    {
        public static void AddOrIgnoreWhenExisting<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TKey, TValue> addValueFactory)
        {
            dictionary.AddOrUpdate(
                key,
                addValueFactory,
                updateValueFactory: (_, existing) => existing);
        }
    }
}