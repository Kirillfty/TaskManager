using TaskManager.Entities;

namespace TaskManager.Repository
{
    public interface ITaskRepository
    {
        public List<Tasks> GetAll();

        public Tasks? GetById(int id);

        public bool Create(Tasks task);

        public bool Update(Tasks task);

        public bool Delete(int id);

    }
}
