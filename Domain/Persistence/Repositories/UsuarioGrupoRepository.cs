using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using kairosApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Persistence.Repositories
{
    public class UsuarioGrupoRepository : BaseRepository, IUsuarioGrupoRepository
    {
        public UsuarioGrupoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(UsuarioGrupo usuarioGrupo)
        {
            await _context.UsuarioGrupos.AddAsync(usuarioGrupo);
        }

        public async Task<IEnumerable<UsuarioGrupo>> ListAsync()
        {
            return await _context.UsuarioGrupos.Include(p => p.Grupo).ToListAsync();
        }

        public async Task<UsuarioGrupo> FindByIdAsync(int id)
        {
            return await _context.UsuarioGrupos.FindAsync(id);
        }

        public void Update(UsuarioGrupo usuarioGrupo)
        {
            _context.UsuarioGrupos.Update(usuarioGrupo);
        }
    }
}
