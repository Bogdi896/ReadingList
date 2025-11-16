using ReadingList.Domain.Entities;
using ReadingList.Application.Interfaces;
using ReadingList.Application.Services;
using ReadingList.Domain.Queries;
using ReadingList.Infrastructure;
using ReadingList.CLI;
using ReadingList.Infrastructure.Export;
using ReadingList.Application.Commands;

IRepository<Book, int> repository = new InMemoryRepository<Book, int>(book => book.Id);
IBookParser parser = new CsvBookParser();
//JsonExporter jsonExporter = new JsonExporter();
//CsvExporter csvExporter = new CsvExporter();
var exporters = new IExporter[]
{
    new JsonExporter(),
    new CsvExporter()
};

var importer = new Importer(repository, parser);
var commands = new ICommand[]
{
    new ImportCommand(importer),
    new ListCommand(repository),
    new FilterCommand(repository),
    new MarkAsFinishedCommand(repository),
    new RateCommand(repository),
    new StatsCommand(repository),
    new ExportCommand(repository, exporters),
    new HelpCommand(),
    new ExitCommand()
};
var commandLoop = new CommandLoop(commands);
commandLoop.Run();