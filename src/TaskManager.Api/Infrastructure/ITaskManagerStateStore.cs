namespace TaskManager.Api.Infrastructure
{
    public interface ITaskManagerStateStore
    {
        void CreateOrReplace(string behavior, int maxCapacity);
        Core.TaskManager GetFor(string behavior);
    }
}