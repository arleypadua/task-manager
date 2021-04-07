using System;
using Xunit;

namespace TaskManager.Tests
{
    public partial class TaskManagerTests
    {
        [Fact]
        public void PriorityBehavior_WhenAddingProcess_ProcessShouldBeAdded()
        {
        }
        
        [Fact]
        public void PriorityBehaviorAndCapacityIsFull_WhenAddingProcessWithHigherPriority_OldestShouldBeKilledNewShouldTakePlace()
        {
        }
        
        [Fact]
        public void PriorityBehaviorAndCapacityIsFull_WhenAddingProcessWithLeastPriority_OldestShouldBeKeptNewShouldSkipped()
        {
        }
        
        [Fact]
        public void PriorityBehaviorWithExistingProcess_WhenKillingExistingProcess_ProcessShouldBeKilled()
        {
        }
    }
}