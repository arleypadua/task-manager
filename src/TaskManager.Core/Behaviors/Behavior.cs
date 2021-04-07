using System;
using System.Collections.Generic;

namespace TaskManager.Core.Behaviors
{
    public abstract class Behavior
    {
        private readonly int _maxCapacity;

        public int MaxCapacity
        {
            get => _maxCapacity;
            internal init =>
                _maxCapacity = value <= 0
                    ? throw new ArgumentException("Max capacity cannot be 0 or less", nameof(value))
                    : value;
        }

        internal abstract IEnumerable<Process> GetProcesses();
        internal abstract void TryToAdd(Process process);
    }
}