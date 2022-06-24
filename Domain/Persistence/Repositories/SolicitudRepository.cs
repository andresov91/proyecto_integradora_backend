using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using kairosApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Domain.Persistence.Repositories
{
    public class SolicitudRepository : BaseRepository, ISolicitudRepository
    {
        public SolicitudRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Solicitud solicitud)
        {
            await _context.Solicitudes.AddAsync(solicitud);
        }

        public async Task<IEnumerable<Solicitud>> ListAsync()
        {
            return await _context.Solicitudes.ToListAsync();
        }

        public async Task<Solicitud> FindByIdAsync(int id)
        {
            return await _context.Solicitudes.FindAsync(id);
        }

        public void Update(Solicitud solicitud)
        {
            _context.Solicitudes.Update(solicitud);
        }
    }
}
