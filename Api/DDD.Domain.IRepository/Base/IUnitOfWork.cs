using System.Threading.Tasks;

namespace DDD.Domain.IRepository
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();       
    }
}
