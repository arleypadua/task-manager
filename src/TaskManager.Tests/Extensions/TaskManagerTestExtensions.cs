using System;
using TaskManager.Core;

namespace TaskManager.Tests.Extensions
{
    internal static class TaskManagerTestExtensions
    {
        private static readonly Random Random = new();

        public static Core.TaskManager WithExistingProcess(this Core.TaskManager taskManager, Process process = null)
        {
            taskManager.Add(
                process ?? new Process(
                    Random.Next(1, int.MaxValue),
                    Random.Next(1, 5)));

            return taskManager;
        }
    }
}