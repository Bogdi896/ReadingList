using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Domain.Common;
using ReadingList.Domain.Entities;
using ReadingList.Application.Interfaces;

namespace ReadingList.Infrastructure
{
    public sealed class CsvBookParser : IBookParser
    {
        public Result<Book> ParseCsvLine(string header, string line)
        {
            try
            {
                var parts = line.Split(',', StringSplitOptions.TrimEntries);
                if (parts.Length < 8)
                    return Result<Book>.Fail("Malformed line: missing columns.");

                int id = int.Parse(parts[0]);
                string title = parts[1].Trim('"');
                string author = parts[2].Trim('"');
                int year = int.Parse(parts[3]);
                int pages = int.Parse(parts[4]);
                string genre = parts[5].Trim('"');
                bool finished = parts[6].Trim().ToLower() is "y" or "yes" or "true";
                double rating = double.Parse(parts[7]);

                var book = new Book(id, title, author, year, pages, genre, finished, rating);
                return Result<Book>.Success(book);
            }
            catch (Exception ex)
            {
                return Result<Book>.Fail($"Malformed line: {ex.Message}");
            }
        }
    }
}
