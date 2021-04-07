using System.Collections.Generic;

namespace TaskManager.Core.Behaviors
{
    public class FifoBehavior : Behavior
    {
        public FifoBehavior()
        {
        }

        internal override IEnumerable<Process> GetProcesses()
        {
            throw new System.NotImplementedException();
        }

        internal override void TryToAdd(Process process)
        {
            throw new System.NotImplementedException();
        }
    }
}