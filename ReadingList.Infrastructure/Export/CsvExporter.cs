using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Domain.Common;
using ReadingList.Application.Interfaces;

namespace ReadingList.Infrastructure.Export
{
    public sealed class CsvExporter : IExporter
    {
        public string Name => "csv";
        public Result Save(string filePath, IEnumerable<Domain.Entities.Book> books)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("Id,Title,Author,Year,Pages,Genre,Finished,Rating");

                foreach (var b in books)
                {
                    string Q(string s) => "\"" + (s?.Replace("\"", "\"\"") ?? string.Empty) + "\"";

                    sb.Append(b.Id).Append(',')
                      .Append(Q(b.Title)).Append(',')
                      .Append(Q(b.Author)).Append(',')
                      .Append(b.Year).Append(',')
                      .Append(b.Pages).Append(',')
                      .Append(Q(b.Genre)).Append(',')
                      .Append(b.Finished ? "yes" : "no").Append(',')
                      .Append(b.Rating.ToString(CultureInfo.InvariantCulture))
                      .AppendLine();
                }

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to save CSV file: {ex.Message}");
            }
        }
    }
}
