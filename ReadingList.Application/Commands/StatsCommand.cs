using ReadingList.Application.Interfaces;
using ReadingList.Domain.Common;
using ReadingList.Domain.Entities;
using ReadingList.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Application.Commands
{
    public sealed class StatsCommand : ICommand
    {
        public string Name => "stats";
        public string Description => "Show statistics about the reading list.";
        private readonly IRepository<Book, int> _bookRepository;
        public StatsCommand(IRepository<Book, int> bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public Task ExecuteAsync(string[] args)
        {
            var books = _bookRepository.All();                 
            var stats = books.Stats();                

            Console.WriteLine("=== Stats ===");

            Console.WriteLine($"Total books     : {stats.TotalBooks}");
            Console.WriteLine($"Finished books  : {stats.FinishedBooks}");

            Console.WriteLine($"Average rating  : {stats.AverageRating.ToString("0.00", CultureInfo.InvariantCulture)}");

            Console.WriteLine();
            Console.WriteLine("Pages by genre:");
            if (stats.PagesByGenre is { Count: > 0 })
            {
                foreach (var gp in stats.PagesByGenre)
                    Console.WriteLine($"  {gp.Genre,-20} {gp.TotalPages,8} pages");
            }
            else
            {
                Console.WriteLine("  (none)");
            }

            Console.WriteLine();
            Console.WriteLine("Top authors (by book count):");
            var top3 = stats.BooksByAuthor
                .OrderByDescending(a => a.BookCount)
                .ThenBy(a => a.Author)        
                .Take(3)
                .ToList();

            if (top3.Count > 0)
            {
                int rank = 1;
                foreach (var a in top3)
                    Console.WriteLine($"  {rank++}. {a.Author,-25} {a.BookCount,3} book(s)");
            }
            else
            {
                Console.WriteLine("  (none)");
            }

            return Task.CompletedTask;
        }
    }
}
