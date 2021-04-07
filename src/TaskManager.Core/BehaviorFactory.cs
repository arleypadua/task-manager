using System;
using System.Collections.Generic;
using TaskManager.Core.Behaviors;

namespace TaskManager.Core
{
    internal static class BehaviorFactory
    {
        private static readonly Dictionary<Type, Func<int, Behavior>> _factories = new()
        {
            {typeof(DefaultBehavior), DefaultBehavior.Create},
            {typeof(FifoBehavior), FifoBehavior.Create},
            {typeof(PriorityBehavior), PriorityBehavior.Create},
        };

        public static Behavior CreateFor<TBehavior>(int maxCapacity)
        {
            if (!_factories.TryGetValue(typeof(TBehavior), out var behavior))
            {
                throw new InvalidOperationException($"Factory for {typeof(TBehavior).Name} does not exist");
            }

            return behavior.Invoke(maxCapacity);
        }
    }
}