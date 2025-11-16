using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Domain.Entities;
using ReadingList.Application.Interfaces;

namespace ReadingList.Application.Services
{
    public sealed class Importer
    {
        private readonly IRepository<Book, int> _repository;
        private readonly IBookParser _parser;

        public Importer(IRepository<Book, int> repository, IBookParser parser)
        {
            _repository = repository;
            _parser = parser;
        }

        public async Task<(int imported, int duplicates, int malformed)> ImportAsync(
    IEnumerable<string> filePaths,
    int maxDegreeOfParallelism = 4,
    CancellationToken ct = default)
        {
            int imported = 0, duplicates = 0, malformed = 0;
            var files = filePaths.ToArray();
            foreach (var f in files.Where(f => !File.Exists(f)))
                Console.WriteLine($"[warn] File not found: {f}");

            var existing = files.Where(File.Exists);

            var po = new ParallelOptions
            {
                MaxDegreeOfParallelism = Math.Max(1, maxDegreeOfParallelism),
                CancellationToken = ct
            };

            await Parallel.ForEachAsync(existing, po, async (path, token) =>
            {
                string[] lines;
                try
                {
                    lines = await File.ReadAllLinesAsync(path, token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[error] Unable to read '{path}': {ex.Message}");
                    return;
                }

                if (lines.Length == 0) { Console.WriteLine($"[info] Empty file: {path}"); return; }

                var header = lines[0];
                int localImported = 0, localDuplicates = 0, localMalformed = 0;

                foreach (var line in lines.Skip(1))
                {
                    token.ThrowIfCancellationRequested();

                    var parsed = _parser.ParseCsvLine(header, line);
                    if (!parsed.Ok) { localMalformed++; continue; }

                    var add = _repository.Add(parsed.Value!);
                    if (add.Ok) localImported++;
                    else localDuplicates++;
                }

                Interlocked.Add(ref imported, localImported);
                Interlocked.Add(ref duplicates, localDuplicates);
                Interlocked.Add(ref malformed, localMalformed);

                Console.WriteLine($"[done] {path}: +{localImported}, dup {localDuplicates}, bad {localMalformed}");
            });

            Console.WriteLine($"[summary] Imported {imported}, duplicates {duplicates}, malformed {malformed}");
            return (imported, duplicates, malformed);
        }

    }
}

