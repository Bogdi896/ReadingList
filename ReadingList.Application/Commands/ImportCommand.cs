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
    public sealed class ImportCommand : ICommand
    {
        public string Name => "import";
        public string Description => "Import books from CSV files.";
        private readonly Importer _importer;

        public ImportCommand(Importer importer)
        {
            _importer = importer;
        }
        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: import <filepath>");
                return;
            }
            var filePaths = args.Skip(0);
            await _importer.ImportAsync(filePaths);
        }
    }
}
