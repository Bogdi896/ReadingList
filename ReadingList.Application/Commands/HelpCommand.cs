using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Application.Interfaces;

namespace ReadingList.Application.Commands
{
    public sealed class HelpCommand : ICommand
    {
        private IEnumerable<ICommand>? _commands;

        public string Name => "help";
        public string Description => "List available commands and usage";

        public void SetCommands(IEnumerable<ICommand> commands) => _commands = commands;

        public async Task ExecuteAsync(string[] args)
        {
            var cmds = _commands?.OrderBy(c => c.Name) ?? Enumerable.Empty<ICommand>();
            Console.WriteLine("Commands:");
            foreach (var c in cmds)
                Console.WriteLine($"  {c.Name,-14} {c.Description}");
        }
    }
}
