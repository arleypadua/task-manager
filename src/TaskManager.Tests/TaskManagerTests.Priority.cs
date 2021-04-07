using System.Linq;
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
        public void PriorityBehavior_WhenAddingProcess_ProcessShouldBeAdded()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<PriorityBehavior>()
                .Build();

            var process = new Process(1, 2);

            taskManager.Add(process);

            taskManager
                .List().Should().Contain(process);
        }

        [Fact]
        public void
            PriorityBehaviorAndCapacityIsFull_WhenAddingProcessWithHigherPriority_OldestShouldBeKilledNewShouldTakePlace()
        {
            var processWithHigherPriority = new Process(1, 1);
            var processWithLowerPriority = new Process(2, 5);

            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<PriorityBehavior>()
                .Build()
                .WithExistingProcess(processWithLowerPriority);

            taskManager.Add(processWithHigherPriority);

            taskManager.List()
                .Should().Contain(processWithHigherPriority)
                .And
                .NotContain(processWithLowerPriority);
        }

        [Fact]
        public void
            PriorityBehaviorAndCapacityIsFull_WhenAddingProcessWithLeastPriority_OldestShouldBeKeptNewShouldSkipped()
        {
            var processWithHigherPriority = new Process(1, 1);
            var processWithLowerPriority = new Process(2, 5);

            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<PriorityBehavior>()
                .Build()
                .WithExistingProcess(processWithHigherPriority);

            taskManager.Add(processWithLowerPriority);

            taskManager.List()
                .Should().Contain(processWithHigherPriority)
                .And
                .NotContain(processWithLowerPriority);
        }

        [Fact]
        public void PriorityBehaviorWithExistingProcess_WhenKillingExistingProcess_ProcessShouldBeKilled()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<PriorityBehavior>()
                .Build()
                .WithExistingProcess();

            var existingProcess = taskManager.List().Single();

            taskManager.Kill(existingProcess.Id.PID);

            taskManager
                .List().Should().BeEmpty();
        }
    }
}