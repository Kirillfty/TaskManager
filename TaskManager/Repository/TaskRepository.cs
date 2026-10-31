using Microsoft.EntityFrameworkCore;
using TaskManager.Entities;
namespace TaskManager.Repository
{
    public class TaskRepository : ITaskRepository
    {
        public ApplicationDB _context;
        private ITaskRepository _taskRepositoryImplementation;

        public TaskRepository(ApplicationDB context)
        {
            _context = context;
        }

        public async Task<bool> Create(TaskItem taskItem)
        {
            _context.Tasks.Add(taskItem);
            //taskItem.CreateDate = DateTime.Now;
            taskItem.Status = "Pending";
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.Id == id);

            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<TaskItem>> GetAllTaskByUser(int userId,CancellationToken ct)
        {
            return await  _context.Tasks.AsNoTracking().Where(x => x.UserId == userId).ToListAsync(ct);
        }

        public async Task<List<TaskItem>> GetAll(string? status)
        {
            if (!string.IsNullOrWhiteSpace(status))
            {
                 return await _context.Tasks
                     .Where(x => x.Status == status)
                     .ToListAsync(); 
            }
            return await _context.Tasks.AsNoTracking().ToListAsync();
        }

        public async Task<TaskItem?> GetById(int id)
        {
            return _context.Tasks.FirstOrDefault(u => u.Id == id);
        }

        public bool Update(TaskItem taskItem)
        {
            _context.Tasks.Update(taskItem);

            try
            { 
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}