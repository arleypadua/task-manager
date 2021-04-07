using Xunit;

namespace TaskManager.Tests
{
    public partial class TaskManagerTests
    {
        [Fact]
        public void DefaultBehavior_WhenAddingProcess_ProcessShouldBeAdded()
        {
        }
        
        [Fact]
        public void DefaultBehaviorAndCapacityIsFull_WhenAddingProcess_ErrorShouldBeThrown()
        {
        }
        
        [Fact]
        public void DefaultBehaviorWithExistingProcess_WhenKillingExistingProcess_ProcessShouldBeKilled()
        {
        }
    }
}