using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager.Core.Behaviors
{
    public abstract class Behavior
    {
        private Behavior()
        {
        }
        
        protected Behavior(int maxCapacity)
        {
            MaxCapacity = maxCapacity <= 0
                ? throw new ArgumentException("Max capacity cannot be 0 or less", nameof(maxCapacity))
                : maxCapacity;
        }

        protected int MaxCapacity { get; }
        protected bool MaxCapacityReached => GetProcesses().Count() >= MaxCapacity;
        
        internal abstract IEnumerable<Process> GetProcesses();
        internal abstract Process GetProcessByPid(int pid);
        internal abstract bool TryToAdd(Process process);
        protected abstract void HandleProcessKilled(Process process);

        protected void SubscribeToProcessKilledOn(Process process)
        {
            process.ProcessKilled += HandleProcessKilled;
        }
        
        protected void UnsubscribeToProcessKilledOn(Process process)
        {
            process.ProcessKilled -= HandleProcessKilled;
        }
    }
}