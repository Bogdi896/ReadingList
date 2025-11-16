using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using ReadingList.Domain.Entities;

namespace ReadingList.Infrastructure.Export
{
    public sealed class JsonExporter
    {
        public void Save(string filePath, IEnumerable<Book> books)
        {
            var json = JsonSerializer.Serialize(books, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(filePath, json);
        }
    }
}
