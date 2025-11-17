using ReadingList.Domain.Common;
using ReadingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Application.Interfaces
{
    public interface IBookParser
    {
        Result<Book> ParseCsvLine(string header, string line);
    }
}
