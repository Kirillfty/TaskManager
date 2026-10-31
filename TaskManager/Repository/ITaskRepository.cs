using TaskManager.Entities;
using System.Threading.Tasks;
namespace TaskManager.Repository
{
    public interface ITaskRepository
    {
        public Task<List<TaskItem>> GetAll(string?  status);
        
        public Task<TaskItem?> GetById(int id);

        public Task<bool> Create(TaskItem taskItem);

        public bool Update(TaskItem taskItem);

        public Task<bool> Delete(int id);
        public Task<List<TaskItem>> GetAllTaskByUser(int userId,CancellationToken ct);

    }
}
