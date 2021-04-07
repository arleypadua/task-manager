using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Api.Infrastructure;
using TaskManager.Core;

namespace TaskManager.Api.Controllers
{
    [Route("task-manager/{behavior}")]
    [ApiController]
    public class TaskManagerController : ControllerBase
    {
        private readonly ITaskManagerStateStore _store;

        public TaskManagerController(ITaskManagerStateStore store)
        {
            _store = store;
        }

        [SwaggerOperation(Summary = "Initializes a task manager for the specified behavior with a given capacity")]
        [Route("initialize")]
        [HttpPost]
        public IActionResult Initialize(
            [FromRoute] Behaviors behavior,
            [FromQuery] int maxCapacity)
        {
            _store.CreateOrReplace(behavior.ToString(), maxCapacity);

            return Ok();
        }

        [SwaggerOperation(Summary =
            "Lists the process for a given task manager, optionally sorting with specified sort")]
        [Route("process")]
        [HttpGet]
        public IActionResult List(
            [FromRoute] Behaviors behavior,
            [FromQuery] SortBy sortBy = SortBy.Default)
        {
            return Ok(_store.GetFor(behavior.ToString())
                .List(sortBy));
        }

        [SwaggerOperation(Summary = "Adds a process to a task manager using a specific behavior")]
        [Route("process")]
        [HttpPost]
        public IActionResult AddProcess(
            [FromRoute] Behaviors behavior,
            [FromBody] AddProcessRequest request)
        {
            _store.GetFor(behavior.ToString())
                .Add(new Process(request.PID, request.Priority));

            return Ok();
        }

        [SwaggerOperation(Summary = "Kills a process with a given PID")]
        [Route("process/{pid}/kill")]
        [HttpPost]
        public IActionResult KillProcess(
            [FromRoute] Behaviors behavior,
            [FromRoute] int pid)
        {
            _store.GetFor(behavior.ToString())
                .Kill(pid);

            return Ok();
        }

        [SwaggerOperation(Summary = "Kills all processes with the specified priority")]
        [Route("process/group/{priority}/kill")]
        [HttpPost]
        public IActionResult KillProcessByGroup(
            [FromRoute] Behaviors behavior,
            [FromRoute] int priority)
        {
            _store.GetFor(behavior.ToString())
                .KillGroup(priority);

            return Ok();
        }

        [SwaggerOperation(Summary = "Kills all processes tracked by the task manager")]
        [Route("process/kill")]
        [HttpPost]
        public IActionResult KillAllProcesses(
            [FromRoute] Behaviors behavior)
        {
            _store.GetFor(behavior.ToString())
                .KillAll();

            return Ok();
        }

        public record AddProcessRequest(int PID, int Priority);

        public enum Behaviors
        {
            DefaultBehavior,
            FifoBehavior,
            PriorityBehavior
        }
    }
}