using System.Collections.Generic;

namespace TaskManager.Core.Abstractions
{
    public interface ITaskManagerBehavior
    {
        IEnumerable<Process> Processes { get; }

        void TryToAdd(Process process);
    }
}