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
    public sealed class InMemoryRepository<T> : IRepository<T>
    {
        private readonly ConcurrentDictionary<int, T> _store = new();
        private readonly Func<T, int> _id;

        public InMemoryRepository(Func<T, int> idSelector)
            => _id = idSelector ?? throw new ArgumentNullException(nameof(idSelector));

        public Result Add(T item)
        {
            var key = _id(item);
            return _store.TryAdd(key, item)
                ? Result.Success()
                : Result.Fail($"Duplicate id {key}.");
        }

        public Result Update(T item)
        {
            var key = _id(item);
            _store[key] = item;                 // upsert semantics for simplicity
            return Result.Success();
        }

        public Result Delete(int id)
            => _store.TryRemove(id, out _)
                ? Result.Success()
                : Result.Fail($"Not found id {id}.");

        public Result<T> TryGet(int id)
            => _store.TryGetValue(id, out var value)
                ? Result<T>.Success(value)
                : Result<T>.Fail($"Not found id {id}.");

        public IEnumerable<T> All() => _store.Values;
    }
}
