using System;
using FluentAssertions;
using TaskManager.Core;
using TaskManager.Core.Behaviors;
using Xunit;

namespace TaskManager.Tests
{
    public partial class TaskManagerTests
    {
        [Fact]
        public void WhenListingSortedByTimeOfCreation_OrderShouldMatch()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 2)
                .With<DefaultBehavior>()
                .Build();

            var processA = new Process(1, 2, DateTime.UtcNow.AddMinutes(-15));
            var processB = new Process(2, 1, DateTime.UtcNow.AddMinutes(-10));

            taskManager.Add(processB);
            taskManager.Add(processA);

            taskManager.List(SortBy.CreationTime)
                .Should().BeEquivalentTo(
                    processA,
                    processB);
        }

        [Fact]
        public void WhenListingSortedByPID_OrderShouldMatch()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 2)
                .With<DefaultBehavior>()
                .Build();

            var processA = new Process(1, 2, DateTime.UtcNow.AddMinutes(-15));
            var processB = new Process(2, 1, DateTime.UtcNow.AddMinutes(-10));

            taskManager.Add(processB);
            taskManager.Add(processA);

            taskManager.List(SortBy.Id)
                .Should().BeEquivalentTo(
                    processA,
                    processB);
        }

        [Fact]
        public void WhenListingSortedByPriority_OrderShouldMatch()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 2)
                .With<DefaultBehavior>()
                .Build();

            var processA = new Process(1, 2, DateTime.UtcNow.AddMinutes(-15));
            var processB = new Process(2, 1, DateTime.UtcNow.AddMinutes(-10));

            taskManager.Add(processB);
            taskManager.Add(processA);

            taskManager.List(SortBy.Priority)
                .Should().BeEquivalentTo(
                    processB,
                    processA);
        }
    }
}