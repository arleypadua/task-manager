using System;
using Xunit;

namespace TaskManager.Tests
{
    public partial class TaskManagerTests
    {
        [Fact]
        public void FIFOBehavior_WhenAddingProcess_ProcessShouldBeAdded()
        {
        }
        
        [Fact]
        public void FIFOBehaviorAndCapacityIsFull_WhenAddingProcess_OldestShouldBeKilled()
        {
        }
        
        [Fact]
        public void FIFOBehaviorWithExistingProcess_WhenKillingExistingProcess_ProcessShouldBeKilled()
        {
        }
    }
}