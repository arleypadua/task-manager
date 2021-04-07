using System.Collections.Generic;

namespace TaskManager.Core.Behaviors
{
    public class PriorityBehavior : Behavior
    {
        private PriorityBehavior(int maxCapacity)
            : base(maxCapacity)
        {
        }

        public static PriorityBehavior Create(int maxCapacity)
        {
            return new(maxCapacity);
        }

        internal override IEnumerable<Process> GetProcesses()
        {
            throw new System.NotImplementedException();
        }

        internal override bool TryToAdd(Process process)
        {
            throw new System.NotImplementedException();
        }
    }
}