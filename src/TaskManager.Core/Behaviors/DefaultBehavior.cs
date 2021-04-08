using System.Collections.Concurrent;
using System.Collections.Generic;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Extensions;

namespace TaskManager.Core.Behaviors
{
    public class DefaultBehavior : Behavior
    {
        private readonly ConcurrentDictionary<int, Process> _processes;

        private DefaultBehavior(int maxCapacity)
            : base(maxCapacity)
        {
            _processes = new ConcurrentDictionary<int, Process>();
        }

        public static DefaultBehavior Create(int maxCapacity)
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
                throw new MaxCapacityOfProcessesReachedException(MaxCapacity, process.Id);

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
                UnsubscribeToProcessKilledOn(process);
            }
        }
    }
}