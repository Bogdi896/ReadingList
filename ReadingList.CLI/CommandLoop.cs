using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Application.Interfaces;
using ReadingList.Application.Services;
using ReadingList.Application.Commands;
using ReadingList.Domain.Entities;
using ReadingList.Domain.Queries;
using ReadingList.Infrastructure.Export;
using System.Security.Cryptography;

namespace ReadingList.CLI
{
    public sealed class CommandLoop
    {
        private readonly Dictionary<string, ICommand> _commands;
        private bool _running;

        public CommandLoop(IEnumerable<ICommand> commands)
        {
            _commands = commands.ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);
            _running = true;

            foreach (var cmd in _commands.Values)
            {
                if (cmd is ExitCommand exit) exit.SetStopAction(() => _running = false);
            }
            // Provide commands list to any HelpCommand instance
            foreach (var cmd in _commands.Values)
            {
                if (cmd is HelpCommand help) help.SetCommands(_commands.Values);
            }
        }

        public void Run()
        {
            Console.WriteLine("Reading List CLI");
            Console.WriteLine("Type 'help' to see available commands.");
            Console.WriteLine();
            while (_running)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var name = parts[0];
                var args = parts.Skip(1).ToArray();

                if (_commands.TryGetValue(name, out var cmd))
                {
                    try { cmd.ExecuteAsync(args); }
                    catch (Exception ex) { Console.WriteLine($"Command error: {ex.Message}"); }
                }
                else
                {
                    Console.WriteLine($"Unknown command '{name}'. Type 'help'.");
                }
            }
        }
    }
}
