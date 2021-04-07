using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Core.Abstractions;

namespace TaskManager.Core
{
    public class TaskManager
    {
        private readonly int _maxCapacity;
        private readonly ITaskManagerBehavior _taskManagerBehavior;

        internal TaskManager(
            int maxCapacity,
            ITaskManagerBehavior taskManagerBehavior)
        {
            if (maxCapacity <= 0)
                throw new ArgumentException("Max capacity cannot be 0 or less", nameof(maxCapacity));

            _maxCapacity = maxCapacity;
            _taskManagerBehavior = taskManagerBehavior ?? throw new ArgumentNullException(nameof(taskManagerBehavior));
        }

        public IEnumerable<Process> List(SortBy sortOption = SortBy.Default) => sortOption switch
        {
            SortBy.CreationTime => _taskManagerBehavior.Processes.OrderBy(p => p.StartedAtUtc),
            SortBy.Id => _taskManagerBehavior.Processes.OrderBy(p => p.Id.PID),
            SortBy.Priority => _taskManagerBehavior.Processes.OrderBy(p => p.Id.Priority),
            SortBy.Default => _taskManagerBehavior.Processes
        };

        public void Add(Process process)
        {
            if (process == null) throw new ArgumentNullException(nameof(process));

            _taskManagerBehavior.TryToAdd(process);
        }

        public void Kill(int pid)
        {
            _taskManagerBehavior
                .Processes
                .SingleOrDefault(p => p.Id.PID == pid)?
                .Kill();
        }

        public void KillGroup(int priority)
        {
            _taskManagerBehavior
                .Processes
                .Where(p => p.Id.Priority == priority)
                .ToList()
                .ForEach(p => p.Kill());
        }

        public void KillAll()
        {
            _taskManagerBehavior
                .Processes
                .ToList()
                .ForEach(p => p.Kill());
        }
    }

    public enum SortBy
    {
        Default,
        CreationTime,
        Priority,
        Id
    }
}