using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Core.Extensions;

namespace TaskManager.Core.Behaviors
{
    public class FifoBehavior : Behavior
    {
        // a queue would be suitable but as multiple threads can add processes,
        // by the time we dequeue the process, another process might be in the tail of the queue
        // so a key-based collection offers a deterministic behavior when removing a process
        private readonly ConcurrentDictionary<int, Process> _processes;

        private FifoBehavior(int maxCapacity)
            : base(maxCapacity)
        {
            _processes = new ConcurrentDictionary<int, Process>();
        }

        public static FifoBehavior Create(int maxCapacity)
        {
            return new(maxCapacity);
        }

        internal override IEnumerable<Process> GetProcesses() => _processes.Values;

        internal override Process GetProcessByPid(int pid)
        {
            return _processes.TryGetValue(pid, out var process)
                ? process
                : default;
        }

        internal override bool TryToAdd(Process process)
        {
            if (_processes.ContainsKey(process.Id.PID))
                return false;
            
            if (MaxCapacityReached)
            {
                _processes.First().Value.Kill();

                // recursively try to add a process, so that eventually we are able to enqueue the process
                return TryToAdd(process);
            }

            _processes.AddOrIgnoreWhenExisting(process.Id.PID,
                addValueFactory: _ =>
                {
                    SubscribeToProcessKilledOn(process);
                    return process;
                });

            return true;
        }

        protected override void HandleProcessKilled(Process process)
        {
            if (_processes.TryRemove(process.Id.PID, out var removed))
            {
                UnsubscribeToProcessKilledOn(removed);
            }
        }
    }
}