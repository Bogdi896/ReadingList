using ReadingList.Application.Interfaces;
using ReadingList.Domain.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Application.Services
{
    public sealed class InMemoryRepository<T, TKey> : IRepository<T, TKey>
        where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, T> _store = new();
        private readonly Func<T, TKey> _keySelector;

        public InMemoryRepository(Func<T, TKey> keySelector)
        {
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        }

        public Result Add(T item)
        {
            var key = _keySelector(item);
            return _store.TryAdd(key, item)
                ? Result.Success()
                : Result.Fail($"Duplicate id {key}.");
        }

        // Upsert semantics: replace if exists, add if not
        public Result Update(T item)
        {
            var key = _keySelector(item);
            _store[key] = item;
            return Result.Success();
        }

        public Result Delete(TKey key)
            => _store.TryRemove(key, out _)
                ? Result.Success()
                : Result.Fail($"Not found id {key}.");

        public Result<T> TryGet(TKey key)
            => _store.TryGetValue(key, out var value)
                ? Result<T>.Success(value)
                : Result<T>.Fail($"Not found id {key}.");

        public IEnumerable<T> All() => _store.Values;
        // If you prefer a stable snapshot for enumeration, use: _store.Values.ToArray()
    }
}
