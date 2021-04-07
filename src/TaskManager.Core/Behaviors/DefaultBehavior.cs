using System.Collections.Generic;
using TaskManager.Core.Abstractions;

namespace TaskManager.Core.Behaviors
{
    public class DefaultBehavior : ITaskManagerBehavior
    {
        public DefaultBehavior()
        {
        }
        
        public IEnumerable<Process> Processes { get; }
        public void TryToAdd(Process process)
        {
            throw new System.NotImplementedException();
        }
    }
}