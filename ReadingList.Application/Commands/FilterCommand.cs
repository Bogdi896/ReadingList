using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Application.Interfaces;
using ReadingList.Domain.Entities;
using ReadingList.Domain.Common;
using ReadingList.Domain.Queries;

namespace ReadingList.Application.Commands
{
    public sealed class FilterCommand : ICommand
    {
        public string Name => "filter";
        public string Description => "Filter books by author or title keyword.";
        private readonly IRepository<Book, int> _bookRepository;
        public FilterCommand(IRepository<Book, int> bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: filter finished / filter top rated <n> / filter by author <author name>");
                return;
            }
            var criteria = args[0].ToLowerInvariant();
            IEnumerable<Book> filteredBooks = Enumerable.Empty<Book>();
            switch (criteria)
            {
                case "finished":
                    filteredBooks = BookQueries.ListFinished(_bookRepository.All());
                    break;
                case "by":
                    if (args.Length < 3 || args[1].ToLowerInvariant() != "author")
                    {
                        Console.WriteLine("Usage: filter by author <author name>");
                        break;
                    }
                    var authorName = string.Join(' ', args.Skip(2));
                    filteredBooks = BookQueries.ByAuthor(_bookRepository.All(), authorName);
                    break;
                case "top":
                    if (args.Length < 3 || args[1].ToLowerInvariant() != "rated" || !int.TryParse(args[2], out int n))
                    {
                        Console.WriteLine("Usage: filter top rated <n>");
                        break;
                    }
                    filteredBooks = BookQueries.TopRated(_bookRepository.All(), n);
                    break;
                default:
                    Console.WriteLine("Unknown filter criteria.");
                    break;
            }
            foreach (var book in filteredBooks)
            {
                Console.WriteLine(book);
            }
        }
    }
}
