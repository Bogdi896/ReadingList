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
    public sealed class RateCommand : ICommand
    {
        public string Name => "rate";
        public string Description => "Rate a book in the reading list.";
        private readonly IRepository<Book, int> _bookRepository;

        public RateCommand(IRepository<Book, int> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length != 2 || !int.TryParse(args[0], out var idToRate))
            {
                Console.WriteLine("Usage: rate <id> <0-5>");
                return;
            }
            if (!double.TryParse(args[1], System.Globalization.NumberStyles.Float,
                                 System.Globalization.CultureInfo.InvariantCulture, out var rating)
                || rating < 0 || rating > 5)
            {
                Console.WriteLine("Rating must be a number between 0 and 5.");
                return;
            }

            {
                var got = _bookRepository.TryGet(idToRate);
                if (!got.Ok) { Console.WriteLine(got.Error); return; }

                var book = got.Value!;
                try
                {
                    book.SetRating(rating);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"400 Bad Request: {ex.Message}");
                    return;
                }
                var upd = _bookRepository.Update(book);
                Console.WriteLine(upd.Ok ? "200 OK (rating updated)" : upd.Error);
            }
        }
    }
}
