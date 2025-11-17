using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using ReadingList.Domain.Entities;
using ReadingList.Domain.Common;
using ReadingList.Application.Interfaces;

namespace ReadingList.Infrastructure.Export
{
    public sealed class JsonExporter : IExporter
    {
        public string Name => "json";
        public Result Save(string filePath, IEnumerable<Book> books)
        {
            try
            {
                var json = JsonSerializer.Serialize(books, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(filePath, json);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to save JSON file: {ex.Message}");
            }
        }
    }
}
