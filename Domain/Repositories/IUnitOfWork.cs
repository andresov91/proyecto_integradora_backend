using System.Threading.Tasks;

namespace kairosApp.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
