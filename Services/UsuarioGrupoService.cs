using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp.Services
{
    public class UsuarioGrupoService : IUsuarioGrupoService
    {
        private readonly IUsuarioGrupoRepository _usuarioGrupoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public UsuarioGrupoService(IUsuarioGrupoRepository usuarioGrupoRepository, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _usuarioGrupoRepository = usuarioGrupoRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<UsuarioGrupo>> ListAsync()
        {
            return await _usuarioGrupoRepository.ListAsync();
        }

        public async Task<SaveUsuarioGrupoResponse> SaveAsync(UsuarioGrupo usuarioGrupo)
        {
            try
            {
                await _usuarioGrupoRepository.AddAsync(usuarioGrupo);
                await _unitOfWork.CompleteAsync();

                return new SaveUsuarioGrupoResponse(usuarioGrupo);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveUsuarioGrupoResponse($"Un error ocurrio mientras se guardaba el grupo usuario: {ex.Message}");
            }
        }

        public async Task<SaveUsuarioGrupoResponse> UpdateAsync(int id, UsuarioGrupo usuarioGrupo)
        {
            var existingUsuarioGrupo = await _usuarioGrupoRepository.FindByIdAsync(id);

            if (existingUsuarioGrupo == null)
                return new SaveUsuarioGrupoResponse("Usuario grupo no Encontrada.");

            existingUsuarioGrupo.GrupoId = usuarioGrupo.GrupoId;
            existingUsuarioGrupo.CuentaUsuarioId = usuarioGrupo.CuentaUsuarioId;

            try
            {
                _usuarioGrupoRepository.Update(existingUsuarioGrupo);
                await _unitOfWork.CompleteAsync();

                return new SaveUsuarioGrupoResponse(existingUsuarioGrupo);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveUsuarioGrupoResponse($"Un error ocurrio mientras se actualizaba a la persona: {ex.Message}");
            }
        }

        public bool UpdateUsuarioGrupos(int usuarioId, List<int> gruposIds)
        {
            try 
            {
                var gruposAEliminar = _context.UsuarioGrupos.Where(p => p.CuentaUsuarioId == usuarioId).ToList();
                if (gruposIds.Any())
                {
                    //Primero Elimino los grupos que no esten
                    foreach (var grupo in gruposAEliminar)
                    {

                        if (!gruposIds.Contains(grupo.Id))
                        {
                            Debug.WriteLine("Elimino el grupo con el grupoId:" + grupo.GrupoId);
                            _context.UsuarioGrupos.Remove(grupo);
                        }
                        else
                        {
                            Debug.WriteLine("Quito de la lista de Ids a el grupoId:" + grupo.GrupoId);
                            gruposIds.RemoveAll(item => item == grupo.GrupoId);
                        }
                    }
                    //Luego añado los grupos que falten
                    foreach (var id in gruposIds)
                    {
                        var userGrupo = new UsuarioGrupo { GrupoId = id, CuentaUsuarioId = usuarioId };
                        _context.UsuarioGrupos.Add(userGrupo);
                        Debug.WriteLine("Añado al grupo Id:" + id);
                    }

                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    foreach(var grupo in gruposAEliminar)
                    {
                        _context.UsuarioGrupos.Remove(grupo);
                    }
                    _context.SaveChanges();
                    return true;
                }
                
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        
    }
}
