using TaskManager.Entities;
namespace TaskManager.Repository
{
    public class TaskRepository : ITaskRepository
    {
        public ApplicationDB _context;

        public TaskRepository(ApplicationDB context)
        {
            _context = context;
        }

        public bool Create(Tasks task)
        {
            _context.Tasks.Add(task);

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

        public bool Delete(int id)
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

        public List<Tasks> GetAll()
        {
            return _context.Tasks.ToList();
        }

        public Tasks? GetById(int id)
        {
            return _context.Tasks.FirstOrDefault(u => u.Id == id);
        }

        public bool Update(Tasks task)
        {
            _context.Tasks.Update(task);

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