using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using kairosApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Domain.Persistence.Repositories
{
    public class PersonaRepository : BaseRepository, IPersonaRepository
    {
        public PersonaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Persona persona)
        {
            await _context.Personas.AddAsync(persona);
        }

        public async Task<IEnumerable<Persona>> ListAsync()
        {
            return await _context.Personas.ToListAsync();
        }

        public async Task<Persona> FindByIdAsync(int id)
        {
            return await _context.Personas.FindAsync(id);
        }

        public void Update(Persona persona)
        {
            _context.Personas.Update(persona);
        }

        public async Task<Persona> FindByCedula(string cedula)
        {
            try
            {
                var persona = _context.Personas.Where(p => p.Identificacion == cedula).Single();
                return persona;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
