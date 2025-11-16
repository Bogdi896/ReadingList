using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Application.Interfaces
{
    public interface ICommand
    {
        string Name { get; }
        public string Description { get;}
        Task ExecuteAsync(string[] args);
    }
}
