using System;

namespace TaskManager.Core.Exceptions
{
    public class MaxCapacityOfProcessesReachedException : InvalidOperationException
    {
        public MaxCapacityOfProcessesReachedException(int maxCapacity, ProcessIdentifier id)
            : base($"Cannot add process {id} as the max capacity of {maxCapacity} processes is reached.")
        {
        }
    }
}