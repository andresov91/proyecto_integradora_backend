using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using kairosApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Persistence.Repositories
{
    public class GrupoRepository : BaseRepository, IGrupoRepository
    {
        public GrupoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Grupo grupo)
        {
            await _context.Grupos.AddAsync(grupo);
        }

        public async Task<IEnumerable<Grupo>> ListAsync()
        {
            return await _context.Grupos.ToListAsync();
        }

        public async Task<Grupo> FindByIdAsync(int id)
        {
            return await _context.Grupos.FindAsync(id);
        }

        public void Update(Grupo grupo)
        {
            _context.Grupos.Update(grupo);
        }
    }
}
