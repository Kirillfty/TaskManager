using Microsoft.AspNetCore.Mvc;
using TaskManager.Entities;
using TaskManager.Repository;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public IUserRepository _repository;

        public UserController(IUserRepository rep) { 
            _repository = rep;
        }



        [HttpGet]
        public List<User> GetAll()
        {
            return _repository.GetAll();
        }

        [HttpGet("{id:int}")]
        public User? GetById([FromRoute]int id)
        {
            return _repository.Get(id);
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody] User u) {
            if (u is not null) {
                _repository.CreateUser(u);
                return Ok();
            }
            return BadRequest();
        }
    }
}
