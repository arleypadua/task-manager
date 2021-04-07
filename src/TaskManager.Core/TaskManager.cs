using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Core.Behaviors;

namespace TaskManager.Core
{
    public class TaskManager
    {
        private readonly Behavior _taskManagerBehavior;

        internal TaskManager(
            Behavior taskManagerBehavior)
        {
            _taskManagerBehavior = taskManagerBehavior ?? throw new ArgumentNullException(nameof(taskManagerBehavior));
        }

        public IEnumerable<Process> List(SortBy sortOption = SortBy.Default) => sortOption switch
        {
            SortBy.CreationTime => _taskManagerBehavior.GetProcesses().OrderBy(p => p.StartedAtUtc),
            SortBy.Id => _taskManagerBehavior.GetProcesses().OrderBy(p => p.Id.PID),
            SortBy.Priority => _taskManagerBehavior.GetProcesses().OrderBy(p => p.Id.Priority),
            SortBy.Default => _taskManagerBehavior.GetProcesses()
        };

        public void Add(Process process)
        {
            if (process == null) throw new ArgumentNullException(nameof(process));

            _taskManagerBehavior.TryToAdd(process);
        }

        public void Kill(int pid)
        {
            _taskManagerBehavior
                .GetProcesses()
                .SingleOrDefault(p => p.Id.PID == pid)?
                .Kill();
        }

        public void KillGroup(int priority)
        {
            _taskManagerBehavior
                .GetProcesses()
                .Where(p => p.Id.Priority == priority)
                .ToList()
                .ForEach(p => p.Kill());
        }

        public void KillAll()
        {
            _taskManagerBehavior
                .GetProcesses()
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