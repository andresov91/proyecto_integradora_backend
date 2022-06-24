using AutoMapper;
using kairosApp.Domain.Services;
using kairosApp.Extensions;
using kairosApp.Models;
using kairosApp.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kairosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoController : ControllerBase
    {
        private readonly IGrupoService _grupoService;
        private readonly IMapper _mapper;
        public GrupoController(IGrupoService grupoService, IMapper mapper)
        {
            _grupoService = grupoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<GrupoResource>> GetAllAsync()
        {
            var accounts = await _grupoService.ListAsync();
            var rsc = _mapper.Map<IEnumerable<Grupo>, IEnumerable<GrupoResource>>(accounts);

            return rsc;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveGrupoResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var grupo = _mapper.Map<SaveGrupoResource,Grupo>(resource);

            var result = await _grupoService.SaveAsync(grupo);

            if (!result.Success)
                return BadRequest(result.Message);

            var grupoResource = _mapper.Map<Grupo, GrupoResource>(result.Grupo);
            return Ok(grupoResource);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveGrupoResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var grupo = _mapper.Map<SaveGrupoResource, Grupo>(resource);
            var result = await _grupoService.UpdateAsync(id, grupo);

            if (!result.Success)
                return BadRequest(result.Message);

            var grupoResource = _mapper.Map<Grupo, GrupoResource>(result.Grupo);
            return Ok(grupoResource);
        }
    }
}
