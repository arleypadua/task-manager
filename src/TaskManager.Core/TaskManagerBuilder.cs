using TaskManager.Core.Behaviors;

namespace TaskManager.Core
{
    public class TaskManagerBuilder
    {
        private readonly int _maxCapacity;
        private Behavior _behavior;

        public TaskManagerBuilder(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
        }

        public TaskManagerBuilder With<TBehavior>()
            where TBehavior : Behavior, new()
        {
            _behavior = new TBehavior
            {
                MaxCapacity = _maxCapacity
            };

            return this;
        }

        public TaskManager Build()
        {
            return new(
                _behavior);
        }
    }
}