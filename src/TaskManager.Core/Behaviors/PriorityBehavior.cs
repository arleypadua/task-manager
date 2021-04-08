using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Core.Extensions;

namespace TaskManager.Core.Behaviors
{
    public class PriorityBehavior : Behavior
    {
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, Process>> _processes;

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

        internal override Process GetProcessByPid(int pid)
        {
            return GetProcesses()
                .FirstOrDefault(p => p.Id.PID == pid);
        }

        internal override bool TryToAdd(Process process)
        {
            var priorityBucket = GetOrCreatePriorityBucket(process);
            if (priorityBucket.ContainsKey(process.Id.PID))
                return false;

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
                    .First()
                    .Kill();

                return TryToAdd(process);
            }

            priorityBucket.AddOrIgnoreWhenExisting(process.Id.PID,
                addValueFactory: _ =>
                {
                    SubscribeToProcessKilledOn(process);
                    return process;
                });

            return true;
        }

        private ConcurrentDictionary<int, Process> GetOrCreatePriorityBucket(Process process)
        {
            return _processes.GetOrAdd(process.Id.Priority, new ConcurrentDictionary<int, Process>());
        }

        protected override void HandleProcessKilled(Process process)
        {
            var priorityBucket = GetOrCreatePriorityBucket(process);
            if (priorityBucket.TryRemove(process.Id.PID, out var removed))
            {
                UnsubscribeToProcessKilledOn(removed);
            }
        }
    }
}