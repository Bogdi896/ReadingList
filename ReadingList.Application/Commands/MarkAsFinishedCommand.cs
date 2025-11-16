using ReadingList.Application.Interfaces;
using ReadingList.Application.Services;
using ReadingList.Domain.Entities;
using ReadingList.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Application.Commands
{
    public sealed class MarkAsFinishedCommand : ICommand
    {
        public string Name => "mark";
        public string Description => "Mark a book as finished in the reading list.";
        private readonly IRepository<Book, int> _bookRepository;
        public MarkAsFinishedCommand(IRepository<Book, int> bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length != 2 || args[0].ToLowerInvariant() != "finished")
            {
                Console.WriteLine("Usage: mark finished <id>");
                return;
            }
            if (!int.TryParse(args[1], out var idToMark))
            {
                Console.WriteLine("Invalid id.");
                return;
            }

            {
                var got = _bookRepository.TryGet(idToMark);
                if (!got.Ok) { Console.WriteLine(got.Error); return; }

                var book = got.Value!;
                book.MarkFinished();
                var upd = _bookRepository.Update(book);
                Console.WriteLine(upd.Ok ? "200 OK (marked finished)" : upd.Error);
            }
        }
    }
}
