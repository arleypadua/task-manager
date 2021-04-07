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

        public int MaxCapacity { get; }
        
        internal abstract IEnumerable<Process> GetProcesses();
        internal abstract bool TryToAdd(Process process);

        protected bool MaxCapacityReached => GetProcesses().Count() >= MaxCapacity;
    }
}