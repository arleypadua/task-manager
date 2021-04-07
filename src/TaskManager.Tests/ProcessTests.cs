using FluentAssertions;
using TaskManager.Core;
using Xunit;

namespace TaskManager.Tests
{
    public class ProcessTests
    {
        [Fact]
        public void WhenKillingProcess_ProcessKilledEventShouldBePublished()
        {
            var process = new Process(1, 1);
            var handled = false;

            void Handler(Process process)
            {
                handled = true;
            }

            process.ProcessKilled += Handler;
            process.Kill();
            process.ProcessKilled -= Handler;

            handled.Should().BeTrue();
        }
    }
}