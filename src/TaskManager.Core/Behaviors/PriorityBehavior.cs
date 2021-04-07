using System.Collections.Generic;
using TaskManager.Core.Abstractions;

namespace TaskManager.Core.Behaviors
{
    public class PriorityBehavior : ITaskManagerBehavior
    {
        public PriorityBehavior()
        {
        }
        
        public IEnumerable<Process> Processes { get; }
        public void TryToAdd(Process process)
        {
            throw new System.NotImplementedException();
        }
    }
}