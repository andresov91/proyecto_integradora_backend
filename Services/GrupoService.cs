using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Domain.Services.Communication;
using kairosApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Services
{
    public class GrupoService : IGrupoService
    {
        private readonly IGrupoRepository _grupoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GrupoService(IGrupoRepository grupoRepository, IUnitOfWork unitOfWork)
        {
            _grupoRepository = grupoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Grupo>> ListAsync()
        {
            return await _grupoRepository.ListAsync();
        }

        public async Task<SaveGrupoResponse> SaveAsync(Grupo grupo)
        {
            try
            {
                await _grupoRepository.AddAsync(grupo);
                await _unitOfWork.CompleteAsync();

                return new SaveGrupoResponse(grupo);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveGrupoResponse($"Un error ocurrio mientras se guardaba el grupo: {ex.Message}");
            }
        }

        public async Task<SaveGrupoResponse> UpdateAsync(int id, Grupo grupo)
        {
            var existingGrupo = await _grupoRepository.FindByIdAsync(id);

            if (existingGrupo == null)
                return new SaveGrupoResponse("Grupo no Encontrada.");

            existingGrupo.Nombre = grupo.Nombre;
            

            try
            {
                _grupoRepository.Update(existingGrupo);
                await _unitOfWork.CompleteAsync();

                return new SaveGrupoResponse(existingGrupo);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new SaveGrupoResponse($"Un error ocurrio mientras se actualizaba el grupo: {ex.Message}");
            }
        }
    }
}
