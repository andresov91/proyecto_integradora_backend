using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using System.Threading.Tasks;

namespace kairosApp.Domain.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

