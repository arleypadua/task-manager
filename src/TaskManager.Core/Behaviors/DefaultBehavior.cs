using System.Collections.Concurrent;
using System.Collections.Generic;
using TaskManager.Core.Exceptions;

namespace TaskManager.Core.Behaviors
{
    public class DefaultBehavior : Behavior
    {
        private readonly ConcurrentDictionary<ProcessIdentifier, Process> _processes;

        public DefaultBehavior()
        {
            _processes = new ConcurrentDictionary<ProcessIdentifier, Process>();
        }

        internal override IEnumerable<Process> GetProcesses() => _processes.Values;

        internal override void TryToAdd(Process process)
        {
            if (_processes.Count >= MaxCapacity)
                throw new MaxCapacityOfProcessesReachedException(MaxCapacity, process.Id);

            var added = _processes.TryAdd(process.Id, process);
            if (!added) return;

            process.ProcessKilled += HandleProcessKilled;
        }

        private void HandleProcessKilled(Process process)
        {
            if (_processes.TryRemove(process.Id, out var removed))
            {
                removed.ProcessKilled -= HandleProcessKilled;
            }
        }
    }
}