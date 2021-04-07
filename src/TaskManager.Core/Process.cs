using System;

namespace TaskManager.Core
{
    public record Process(ProcessIdentifier Id, DateTime StartedAtUtc)
    {
        public Process(int pid, int priority, DateTime? startedAtUtc = null)
            : this(new ProcessIdentifier(pid, priority), startedAtUtc ?? DateTime.UtcNow)
        {
        }

        internal void Kill()
        {
            // release resources
            
            ProcessKilled?.Invoke(this);
        }

        internal event Action<Process> ProcessKilled;
    }
    
    public record ProcessIdentifier(int PID, int Priority);
}