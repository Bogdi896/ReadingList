using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Application.Interfaces;
using ReadingList.Application.Services;
using ReadingList.Domain.Entities;
using ReadingList.Domain.Queries;
using ReadingList.Infrastructure.Export;

namespace ReadingList.CLI
{
    public sealed class CommandLoop
    {
        private readonly IRepository<Book, int> _repo;
        private readonly Importer _importer;
        private readonly JsonExporter _jsonExporter;
        private readonly CsvExporter _csvExporter;

        public CommandLoop(IRepository<Book, int> repo, Importer importer, JsonExporter jsonExporter, CsvExporter csvExporter)
        {
            _repo = repo;
            _importer = importer;
            _jsonExporter = jsonExporter;
            _csvExporter = csvExporter;
        }

        public async Task Run()
        {
            Console.WriteLine("Welcome to the Reading List CLI! Type 'help' for a list of commands.");
            bool running = true;
            while (running)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var cmd = parts[0].ToLowerInvariant();

                try
                {
                    switch (cmd)
                    {
                        case "help":
                            Console.WriteLine("Available commands:");
                            Console.WriteLine("  import <filepath> - Import books from a file");
                            Console.WriteLine("  list - List all books");
                            Console.WriteLine("  filter finished / by author <author name> / top rated n / ");
                            Console.WriteLine("  mark finished <id> - Mark a book as finished");
                            Console.WriteLine("  rate <id> <0-5> - Rate a book");
                            Console.WriteLine("  stats - Show statistics");
                            Console.WriteLine("  export json <path> - Export books to a JSON file");
                            Console.WriteLine("  export csv <path> - Export books to a CSV file");
                            Console.WriteLine("  exit - Exit the application");
                            break;
                        case "import":
                            if (parts.Length < 2)
                            {
                                Console.WriteLine("Usage: import <filepath>");
                                break;
                            }
                            var filePaths = parts.Skip(1);
                            await _importer.ImportAsync(filePaths);
                            break;
                        case "list":
                            var books = _repo.All();
                            foreach (var book in books)
                            {
                                 Console.WriteLine(book);
                            }
                            break;
                        case "filter":
                            if (parts.Length < 2)
                            {
                                Console.WriteLine("Usage: filter <criteria>");
                                break;
                            }
                            var criteria = parts[1].ToLowerInvariant();
                            IEnumerable<Book> filteredBooks = Enumerable.Empty<Book>();
                            switch (criteria)
                            {
                                case "finished":
                                    filteredBooks = BookQueries.ListFinished(_repo.All());
                                    break;
                                case "by":
                                    if (parts.Length < 4 || parts[2].ToLowerInvariant() != "author")
                                    {
                                        Console.WriteLine("Usage: filter by author <author name>");
                                        break;
                                    }
                                    var authorName = string.Join(' ', parts.Skip(3));
                                    filteredBooks = BookQueries.ByAuthor(_repo.All(), authorName);
                                    break;
                                case "top":
                                    if (parts.Length < 4 || parts[2].ToLowerInvariant() != "rated" || !int.TryParse(parts[3], out int n))
                                    {
                                        Console.WriteLine("Usage: filter top rated <n>");
                                        break;
                                    }
                                    filteredBooks = BookQueries.TopRated(_repo.All(), n);
                                    break;
                                default:
                                    Console.WriteLine("Unknown filter criteria.");
                                    break;
                            }
                            foreach (var book in filteredBooks)
                            {
                                Console.WriteLine(book);
                            }
                            break;
                        case "mark":
                            if (parts.Length != 3 || parts[1].ToLowerInvariant() != "finished")
                            {
                                Console.WriteLine("Usage: mark finished <id>");
                                break;
                            }
                            if (!int.TryParse(parts[2], out var idToMark))
                            {
                                Console.WriteLine("Invalid id.");
                                break;
                            }

                            {
                                var got = _repo.TryGet(idToMark);
                                if (!got.Ok) { Console.WriteLine(got.Error); break; }

                                var book = got.Value!;
                                book.MarkFinished();
                                var upd = _repo.Update(book);
                                Console.WriteLine(upd.Ok ? "200 OK (marked finished)" : upd.Error);
                            }
                            break;
                        case "rate":
                            if (parts.Length != 3 || !int.TryParse(parts[1], out var idToRate))
                            {
                                Console.WriteLine("Usage: rate <id> <0-5>");
                                break;
                            }
                            if (!double.TryParse(parts[2], System.Globalization.NumberStyles.Float,
                                                 System.Globalization.CultureInfo.InvariantCulture, out var rating)
                                || rating < 0 || rating > 5)
                            {
                                Console.WriteLine("Rating must be a number between 0 and 5.");
                                break;
                            }

                            {
                                var got = _repo.TryGet(idToRate);
                                if (!got.Ok) { Console.WriteLine(got.Error); break; }

                                var book = got.Value!;
                                try
                                {
                                    book.SetRating(rating);
                                }
                                catch (ArgumentOutOfRangeException ex)
                                {
                                    Console.WriteLine($"400 Bad Request: {ex.Message}");
                                    break;
                                }
                                var upd = _repo.Update(book);
                                Console.WriteLine(upd.Ok ? "200 OK (rating updated)" : upd.Error);
                            }
                            break;
                        case "export":
                            if (parts.Length < 3)
                            {
                                Console.WriteLine("Usage: export json <path> | export csv <path>");
                                break;
                            }
                            var kind = parts[1].ToLowerInvariant();
                            var outPath = string.Join(' ', parts.Skip(2));

                            if (File.Exists(outPath))
                            {
                                Console.Write("File exists. Overwrite? (y/n): ");
                                var ans = Console.ReadLine()?.Trim().ToLowerInvariant();
                                if (ans is not "y" and not "yes") { Console.WriteLine("Aborted."); break; }
                            }

                            var data = _repo.All();
                            try
                            {
                                if (kind == "json") _jsonExporter.Save(outPath, data);
                                else if (kind == "csv") _csvExporter.Save(outPath, data);
                                else { Console.WriteLine("Use: export json <path> | export csv <path>"); break; }

                                Console.WriteLine($"Saved to: {outPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Export failed: {ex.Message}");
                            }
                            break;
                        case "stats":
                            var stats = BookQueries.Stats(_repo.All());
                            Console.WriteLine("Reading List Statistics:");
                            Console.WriteLine($"  Total books: {stats.TotalBooks}");
                            Console.WriteLine($"  Finished books: {stats.FinishedBooks}");
                            Console.WriteLine($"  Average rating: {stats.AverageRating:F2}");
                            break;
                        case "exit":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }
        }
    }
}
