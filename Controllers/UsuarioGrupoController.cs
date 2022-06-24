using AutoMapper;
using kairosApp.Domain.Services;
using kairosApp.Extensions;
using kairosApp.Models;
using kairosApp.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace kairosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioGrupoController : ControllerBase
    {
        private readonly IUsuarioGrupoService _usuarioGrupoService;
        private readonly IMapper _mapper;
        public UsuarioGrupoController(IUsuarioGrupoService usuarioGrupoService, IMapper mapper)
        {
            _usuarioGrupoService = usuarioGrupoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UsuarioGrupoResource>> GetAllAsync()
        {
            var usuarioGrupos = await _usuarioGrupoService.ListAsync();
            var rsc = _mapper.Map<IEnumerable<UsuarioGrupo>, IEnumerable<UsuarioGrupoResource>>(usuarioGrupos);

            return rsc;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveUsuarioGrupoResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var usuarioGrupo = _mapper.Map<SaveUsuarioGrupoResource, UsuarioGrupo>(resource);

            var result = await _usuarioGrupoService.SaveAsync(usuarioGrupo);

            if (!result.Success)
                return BadRequest(result.Message);

            var usuarioGrupoResource = _mapper.Map<UsuarioGrupo, UsuarioGrupoResource>(result.UsuarioGrupo);
            return Ok(usuarioGrupoResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveUsuarioGrupoResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var usuarioGrupo = _mapper.Map<SaveUsuarioGrupoResource, UsuarioGrupo>(resource);
            var result = await _usuarioGrupoService.UpdateAsync(id, usuarioGrupo);

            if (!result.Success)
                return BadRequest(result.Message);

            var solicitudResource = _mapper.Map<UsuarioGrupo, UsuarioGrupoResource>(result.UsuarioGrupo);
            return Ok(solicitudResource);
        }
        [HttpPut]
        [Route("editar")]
        public async Task<IActionResult> UpdateGruposToUser([FromBody]UsurioListaGrupos grupos)
        {
            var proceso = _usuarioGrupoService.UpdateUsuarioGrupos(grupos.CuentaUsuarioId, grupos.GruposIds);
            if (proceso)
            {
                return Ok("Operacion exitosa");
            }
            return BadRequest(new ErrorResource { ErrorMessage = "Ocurrio un problema al quitar los grupos" });
        }
    }

    public class UsurioListaGrupos
    {
        [Required]
        public int CuentaUsuarioId { get; set; }
        [Required]
        public List<int> GruposIds     { get; set; }
    }
}
