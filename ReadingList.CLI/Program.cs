using ReadingList.Domain.Entities;
using ReadingList.Application.Interfaces;
using ReadingList.Application.Services;
using ReadingList.Domain.Queries;
using ReadingList.Infrastructure;
using ReadingList.CLI;
using ReadingList.Infrastructure.Export;

IRepository<Book, int> repository = new InMemoryRepository<Book, int>(book => book.Id);
IBookParser parser = new CsvBookParser();
JsonExporter jsonExporter = new JsonExporter();
CsvExporter csvExporter = new CsvExporter();

var importer = new Importer(repository, parser);
var cli = new CommandLoop(repository, importer, jsonExporter, csvExporter);
await cli.Run();