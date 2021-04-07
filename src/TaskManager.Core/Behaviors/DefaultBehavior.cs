using System.Collections.Concurrent;
using System.Collections.Generic;
using TaskManager.Core.Exceptions;

namespace TaskManager.Core.Behaviors
{
    public class DefaultBehavior : Behavior
    {
        private readonly ConcurrentDictionary<ProcessIdentifier, Process> _processes;

        private DefaultBehavior(int maxCapacity)
            : base(maxCapacity)
        {
            _processes = new ConcurrentDictionary<ProcessIdentifier, Process>();
        }

        public static DefaultBehavior Create(int maxCapacity)
        {
            return new(maxCapacity);
        }

        internal override IEnumerable<Process> GetProcesses() => _processes.Values;

        internal override bool TryToAdd(Process process)
        {
            if (MaxCapacityReached)
                throw new MaxCapacityOfProcessesReachedException(MaxCapacity, process.Id);

            var added = _processes.TryAdd(process.Id, process);
            if (added) SubscribeToProcessKilledOn(process);

            return added;
        }

        protected override void HandleProcessKilled(Process process)
        {
            if (_processes.TryRemove(process.Id, out var removed))
            {
                UnsubscribeToProcessKilledOn(process);
            }
        }
    }
}