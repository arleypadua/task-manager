using TaskManager.Core.Abstractions;

namespace TaskManager.Core
{
    public class TaskManagerBuilder
    {
        private readonly int _maxCapacity;
        private ITaskManagerBehavior _behavior;

        public TaskManagerBuilder(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
        }

        public TaskManagerBuilder With<TBehavior>()
            where TBehavior : ITaskManagerBehavior, new()
        {
            _behavior = new TBehavior();

            return this;
        }

        public TaskManager Build()
        {
            return new(
                _maxCapacity,
                _behavior);
        }
    }
}