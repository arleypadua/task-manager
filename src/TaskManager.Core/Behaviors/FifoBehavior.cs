using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Core.Behaviors
{
    public class FifoBehavior : Behavior
    {
        // a queue would be suitable but as multiple threads can add processes,
        // by the time we dequeue the process, another process might be in the tail of the queue
        private readonly ConcurrentDictionary<ProcessIdentifier, Process> _processes;

        private FifoBehavior(int maxCapacity)
            : base(maxCapacity)
        {
            _processes = new ConcurrentDictionary<ProcessIdentifier, Process>();
        }

        public static FifoBehavior Create(int maxCapacity)
        {
            return new(maxCapacity);
        }

        internal override IEnumerable<Process> GetProcesses() => _processes.Values;

        internal override bool TryToAdd(Process process)
        {
            if (MaxCapacityReached)
            {
                _processes.Last().Value.Kill();

                // recursively try to add a process, so that eventually we are able to enqueue the process
                return TryToAdd(process);
            }

            var added = _processes.TryAdd(process.Id, process);
            if (added) SubscribeToProcessKilledOn(process);

            return added;
        }

        protected override void HandleProcessKilled(Process process)
        {
            if (_processes.TryRemove(process.Id, out var removed))
            {
                UnsubscribeToProcessKilledOn(removed);
            }
        }
    }
}