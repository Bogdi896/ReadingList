using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Domain.Entities;
using ReadingList.Domain.Common;

namespace ReadingList.Application.Interfaces
{
    public interface IExporter
    {
        string Name { get; }
        Result Save(string filePath, IEnumerable<Book> books);
    }
}
