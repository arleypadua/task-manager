using System.Collections.Generic;
using TaskManager.Core.Abstractions;

namespace TaskManager.Core.Behaviors
{
    public class FifoBehavior : ITaskManagerBehavior
    {
        public FifoBehavior()
        {
        }
        
        public IEnumerable<Process> Processes { get; }
        public void TryToAdd(Process process)
        {
            throw new System.NotImplementedException();
        }
    }
}