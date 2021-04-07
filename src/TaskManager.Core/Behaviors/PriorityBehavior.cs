using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Core.Behaviors
{
    public class PriorityBehavior : Behavior
    {
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<ProcessIdentifier, Process>> _processes;

        private PriorityBehavior(int maxCapacity)
            : base(maxCapacity)
        {
            _processes = new();
        }

        public static PriorityBehavior Create(int maxCapacity)
        {
            return new(maxCapacity);
        }

        internal override IEnumerable<Process> GetProcesses() => _processes
            .SelectMany(p => p.Value.Values);

        internal override bool TryToAdd(Process process)
        {
            var priorityBucket = GetOrCreatePriorityBucket(process);

            if (MaxCapacityReached)
            {
                var leastPriorityBuckets = _processes
                    .Where(k => k.Key > process.Id.Priority && k.Value.Any())
                    .OrderBy(k => k.Key)
                    .ToArray();

                // when no buckets with less priority are available, we don't add the process
                if (!leastPriorityBuckets.Any()) return false;

                var leastPriorityBucket = leastPriorityBuckets.Last();

                leastPriorityBucket
                    .Value
                    .OrderBy(d => d.Value.StartedAtUtc)
                    .Select(v => v.Value)
                    .Last()
                    .Kill();

                return TryToAdd(process);
            }

            var added = priorityBucket.TryAdd(process.Id, process);
            if (added) SubscribeToProcessKilledOn(process);

            return added;
        }

        private ConcurrentDictionary<ProcessIdentifier, Process> GetOrCreatePriorityBucket(Process process)
        {
            return _processes.GetOrAdd(process.Id.Priority, new ConcurrentDictionary<ProcessIdentifier, Process>());
        }

        protected override void HandleProcessKilled(Process process)
        {
            var priorityBucket = GetOrCreatePriorityBucket(process);
            if (priorityBucket.TryRemove(process.Id, out var removed))
            {
                UnsubscribeToProcessKilledOn(removed);
            }
        }
    }
}