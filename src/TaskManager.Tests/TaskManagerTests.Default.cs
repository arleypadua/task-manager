using System;
using System.Linq;
using FluentAssertions;
using TaskManager.Core;
using TaskManager.Core.Behaviors;
using TaskManager.Core.Exceptions;
using TaskManager.Tests.Extensions;
using Xunit;

namespace TaskManager.Tests
{
    public partial class TaskManagerTests
    {
        [Fact]
        public void DefaultBehavior_WhenAddingProcess_ProcessShouldBeAdded()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<DefaultBehavior>()
                .Build();

            var process = new Process(1, 2);

            taskManager.Add(process);

            taskManager
                .List().Should().Contain(process);
        }

        [Fact]
        public void DefaultBehaviorAndCapacityIsFull_WhenAddingProcess_ErrorShouldBeThrown()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<DefaultBehavior>()
                .Build()
                .WithExistingProcess();

            var existing = taskManager.List().Single();
            var processToAdd = new Process(1, 2);

            taskManager.Invoking(t => t.Add(processToAdd))
                .Should()
                .Throw<MaxCapacityOfProcessesReachedException>();

            taskManager
                .List().Should().Contain(existing)
                .And
                .NotContain(processToAdd);
        }

        [Fact]
        public void DefaultBehaviorWithExistingProcess_WhenKillingExistingProcess_ProcessShouldBeKilled()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<DefaultBehavior>()
                .Build()
                .WithExistingProcess();

            taskManager.Kill(
                taskManager.List().Single().Id.PID);

            taskManager
                .List().Should().BeEmpty();
        }
    }
}