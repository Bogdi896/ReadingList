using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Domain.Common;

namespace ReadingList.Application.Interfaces
{
    public interface IRepository<T>
    {
        Result Add(T item);             
        Result Update(T item);           
        Result Delete(int id);          
        Result<T> TryGet(int id);        
        IEnumerable<T> All();          
    }
}
