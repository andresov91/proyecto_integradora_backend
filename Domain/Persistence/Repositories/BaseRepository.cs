using kairosApp.Domain.Persistence.Contexts;

namespace kairosApp.Domain.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
