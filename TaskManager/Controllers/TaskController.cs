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

        public TaskController(ITaskRepository repository) { 
            _repository = repository;
        }

        [HttpGet]
        public List<Tasks> GetTasks() {
            return _repository.GetAll();
        }

        [HttpGet("{id:int}")]
        public Tasks? GetById([FromBody]int id) { 
            return _repository.GetById(id);
        }

        [HttpPost]
        public ActionResult<Tasks> CreateTask([FromBody] Tasks task) {
            if (task is not null) { 
                _repository.Create(task);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public bool Delete([FromRoute]int id) {
            return _repository.Delete(id);
        }

        [HttpPut]
        public ActionResult UpdateTask([FromBody] Tasks task) {
            if (task is not null)
            {
                _repository.Update(task);
                return Ok();
            }
            return BadRequest();
        }


    }
}
