using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingList.Application.Interfaces;

namespace ReadingList.Application.Commands
{
    public sealed class ExitCommand : ICommand
    {
        private Action? _stop;

        public string Name => "exit";
        public string Description => "Exit the application";

        public void SetStopAction(Action stop) => _stop = stop;

        public async Task ExecuteAsync(string[] args)
        {
            _stop?.Invoke();  // flips _running=false in the loop
        }
    }
}
