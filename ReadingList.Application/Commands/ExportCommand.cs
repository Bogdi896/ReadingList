using ReadingList.Application.Interfaces;
using ReadingList.Application.Services;
using ReadingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Application.Commands
{
    public sealed class ExportCommand : ICommand
    {
        public string Name => "export";
        public string Description => "Export books to JSON or CSV files.";
        private readonly IRepository<Book, int> _repo;
        private readonly IReadOnlyDictionary<string, IExporter> _exporters;

        public ExportCommand(IRepository<Book, int> repo, IEnumerable<IExporter> exporters)
        {
            _repo = repo;
            _exporters = exporters.ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
        }

        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: export json <path> | export csv <path>");
                return;
            }
            var kind = args[0].ToLowerInvariant();
            var outPath = string.Join(' ', args.Skip(1));

            if (File.Exists(outPath))
            {
                Console.Write("File exists. Overwrite? (y/n): ");
                var ans = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (ans is not "y" and not "yes") { Console.WriteLine("Aborted."); return; }
            }

            var data = _repo.All();
            try
            {
                if (_exporters.ContainsKey(kind))
                {
                    _exporters[kind].Save(outPath, data);
                }
                else { Console.WriteLine("Use: export json <path> | export csv <path>"); return; }

                Console.WriteLine($"Saved to: {outPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }
    }
}
