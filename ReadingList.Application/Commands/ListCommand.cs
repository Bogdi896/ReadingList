using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Application.Interfaces;
using ReadingList.Application.Services;
using ReadingList.Domain.Entities;
using ReadingList.Domain.Queries;

namespace ReadingList.Application.Commands
{
    public sealed class ListCommand : ICommand
    {
        public string Name => "list";
        public string Description => "List all books in the reading list.";
        private readonly IRepository<Book, int> _bookRepository;
        public ListCommand(IRepository<Book, int> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task ExecuteAsync(string[] args)
        {
            var books = BookQueries.ListAll(_bookRepository.All());
            if (!books.Any())
            {
                Console.WriteLine("No books found in the reading list.");
                return;
            }
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
            await Task.CompletedTask;
        }
    }
}
