using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TaskManager.Core;

namespace TaskManager.Api.Infrastructure
{
    internal class InMemoryTaskManagerStateStore : ITaskManagerStateStore
    {
        private ConcurrentDictionary<string, Core.TaskManager> _state;

        public InMemoryTaskManagerStateStore()
        {
            _state = new ConcurrentDictionary<string, Core.TaskManager>();
        }

        public void CreateOrReplace(string behavior, int maxCapacity)
        {
            _state.TryRemove(behavior, out _);
            _state.TryAdd(behavior, new TaskManagerBuilder(maxCapacity)
                .With(behavior)
                .Build());
        }

        public Core.TaskManager GetFor(string behavior)
        {
            return _state.GetValueOrDefault(behavior)
                   ?? throw new InvalidOperationException(
                       $"Task Manager with {behavior} has not been initialized yet.");
        }
    }
}