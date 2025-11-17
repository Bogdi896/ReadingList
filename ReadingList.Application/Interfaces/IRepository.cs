using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Domain.Common;

namespace ReadingList.Application.Interfaces
{
    public interface IRepository<T, TKey>
    where TKey : notnull
    {
        Result Add(T item);
        Result Update(T item);
        Result Delete(TKey key);
        Result<T> TryGet(TKey key);
        IEnumerable<T> All();
    }
}
