using System;
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
        public void FIFOBehavior_WhenAddingProcess_ProcessShouldBeAdded()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<FifoBehavior>()
                .Build();

            var process = new Process(1, 2);

            taskManager.Add(process);

            taskManager
                .List().Should().Contain(process);
        }
        
        [Fact]
        public void FIFOBehaviorAndCapacityIsFull_WhenAddingProcess_OldestShouldBeKilled()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<FifoBehavior>()
                .Build()
                .WithExistingProcess();

            var oldProcess = taskManager.List().Single();
            var newProcess = new Process(1, 2);
            
            taskManager.Add(newProcess);

            taskManager
                .List().Should().Contain(newProcess)
                .And
                .NotContain(oldProcess);
        }
        
        [Fact]
        public void FIFOBehaviorWithExistingProcess_WhenKillingExistingProcess_ProcessShouldBeKilled()
        {
            var taskManager = new TaskManagerBuilder(maxCapacity: 1)
                .With<FifoBehavior>()
                .Build()
                .WithExistingProcess();

            var existingProcess = taskManager.List().Single();
            
            taskManager.Kill(existingProcess.Id.PID);

            taskManager
                .List().Should().BeEmpty();
        }
    }
}