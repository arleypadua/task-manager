using System;
using FluentAssertions;
using TaskManager.Core;
using TaskManager.Core.Behaviors;
using TaskManager.Tests.Extensions;
using Xunit;

namespace TaskManager.Tests
{
    public partial class TaskManagerTests
    {
        [Fact]
        public void WhenListingSortedByTimeOfCreation_OrderShouldMatch()
        {
            var processA = new Process(1, 2, DateTime.UtcNow.AddMinutes(-15));
            var processB = new Process(2, 1, DateTime.UtcNow.AddMinutes(-10));
            
            var taskManager = new TaskManagerBuilder(maxCapacity: 2)
                .With<DefaultBehavior>()
                .Build()
                .WithExistingProcess(processB)
                .WithExistingProcess(processA);

            taskManager.List(SortBy.CreationTime)
                .Should().BeEquivalentTo(
                    processA,
                    processB);
        }

        [Fact]
        public void WhenListingSortedByPID_OrderShouldMatch()
        {
            var processA = new Process(1, 2, DateTime.UtcNow.AddMinutes(-15));
            var processB = new Process(2, 1, DateTime.UtcNow.AddMinutes(-10));
            
            var taskManager = new TaskManagerBuilder(maxCapacity: 2)
                .With<DefaultBehavior>()
                .Build()
                .WithExistingProcess(processB)
                .WithExistingProcess(processA);

            taskManager.List(SortBy.Id)
                .Should().BeEquivalentTo(
                    processA,
                    processB);
        }

        [Fact]
        public void WhenListingSortedByPriority_OrderShouldMatch()
        {
            var processA = new Process(1, 2, DateTime.UtcNow.AddMinutes(-15));
            var processB = new Process(2, 1, DateTime.UtcNow.AddMinutes(-10));
            
            var taskManager = new TaskManagerBuilder(maxCapacity: 2)
                .With<DefaultBehavior>()
                .Build()
                .WithExistingProcess(processB)
                .WithExistingProcess(processA);

            taskManager.List(SortBy.Priority)
                .Should().BeEquivalentTo(
                    processB,
                    processA);
        }

        [Fact]
        public void WhenKillingByGroup_GroupShouldBeKilled()
        {
            var processA = new Process(1, 2, DateTime.UtcNow.AddMinutes(-15));
            var processB = new Process(2, 1, DateTime.UtcNow.AddMinutes(-10));
            
            var taskManager = new TaskManagerBuilder(maxCapacity: 2)
                .With<DefaultBehavior>()
                .Build()
                .WithExistingProcess(processB)
                .WithExistingProcess(processA);

            taskManager.KillGroup(2);

            taskManager.List()
                .Should().NotContain(processA);
        }

        [Fact]
        public void WhenKillingAll_AllShouldBeKilled()
        {
            var processA = new Process(1, 2, DateTime.UtcNow.AddMinutes(-15));
            var processB = new Process(2, 1, DateTime.UtcNow.AddMinutes(-10));
            
            var taskManager = new TaskManagerBuilder(maxCapacity: 2)
                .With<DefaultBehavior>()
                .Build()
                .WithExistingProcess(processB)
                .WithExistingProcess(processA);

            taskManager.KillAll();

            taskManager.List()
                .Should().BeEmpty();
        }
    }
}