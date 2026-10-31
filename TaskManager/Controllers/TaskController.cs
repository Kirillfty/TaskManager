using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Entities;
using TaskManager.Repository;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TaskController : ControllerBase
    {
        public ITaskRepository _repository;
        public record TaskItemRequest(string Title, string Description,string Status);
        public record UpdateTaskRequest(int Id,string Title, string Description, string Status,int userId);
        public TaskController(ITaskRepository repository) { 
            _repository = repository;
        }

        
        [Authorize]
        [HttpGet]
        public async Task<List<TaskItem>> GetAll([FromQuery] string? status) {
            return await _repository.GetAll(status);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<TaskItem?> GetById([FromBody]int id) { 
            return await _repository.GetById(id);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] TaskItemRequest taskItemRequest)
        {
            var user =  HttpContext.User.Identity.Name;
            TaskItem item = new TaskItem
            {
                Title = taskItemRequest.Title,
                Description = taskItemRequest.Description,
                Status = taskItemRequest.Status,
                UserId = int.Parse(user)
            };
            if (taskItemRequest is not null) { 
                await _repository.Create(item);
                return Created();
            }
            return BadRequest();
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id) {
            var user = await _repository.GetById(id);
            if (user is null)
            {
                return NotFound();
            }
            await _repository.Delete(id);
            return Ok();
        }

   
        [HttpGet("/by-user{userId:int}")]
        public async Task<ActionResult<List<TaskItem>>> GetAllByUser([FromRoute]int userId,CancellationToken ct)
        {
           return await _repository.GetAllTaskByUser(userId,ct);
        }
        
        [Authorize]
        [HttpPut]
        public ActionResult UpdateTask([FromBody] UpdateTaskRequest? taskItem)
        {
            var user = HttpContext.User.Identity.Name;
            TaskItem task = new TaskItem
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Status = taskItem.Status,
                UserId = int.Parse(user)
            };
            if (_repository.Update(task) == null)
            {
                return BadRequest();
            }

            return Ok();
        }


    }
}
