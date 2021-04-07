using System;
using System.Collections.Generic;
using TaskManager.Core.Behaviors;

namespace TaskManager.Core
{
    internal static class BehaviorFactory
    {
        private static readonly Dictionary<string, Func<int, Behavior>> _factories = new()
        {
            {nameof(DefaultBehavior), DefaultBehavior.Create},
            {nameof(FifoBehavior), FifoBehavior.Create},
            {nameof(PriorityBehavior), PriorityBehavior.Create},
        };

        public static Behavior CreateFor<TBehavior>(int maxCapacity)
        {
            if (!_factories.TryGetValue(typeof(TBehavior).Name, out var behavior))
            {
                throw new InvalidOperationException($"Factory for {typeof(TBehavior).Name} does not exist. " +
                                                    $"Please choose one existing: {string.Join(", ", _factories.Keys)}");
            }

            return behavior.Invoke(maxCapacity);
        }

        public static Behavior CreateFor(string behaviorName, int maxCapacity)
        {
            if (!_factories.TryGetValue(behaviorName, out var behavior))
            {
                throw new InvalidOperationException($"Factory for {behaviorName} does not exist. " +
                                                    $"Please choose one existing: {string.Join(", ", _factories.Keys)}");
            }

            return behavior.Invoke(maxCapacity);
        }
    }
}