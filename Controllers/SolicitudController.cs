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
    public class SolicitudController : ControllerBase
    {
        private readonly ISolicitudService _solicitudService;
        private readonly IMapper _mapper;
        public SolicitudController(ISolicitudService solicitudService, IMapper mapper)
        {
            _solicitudService = solicitudService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<SolicitudResource>> GetAllAsync()
        {
            var solicitudes = await _solicitudService.ListAsync();
            var rsc = _mapper.Map<IEnumerable<Solicitud>, IEnumerable<SolicitudResource>>(solicitudes);
            /*var lista_solicitudes = new List<SolicitudInfoResource>();
            foreach (var solicitud in rsc)
            {

            }*/
            return rsc;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveSolicitudResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var solicitud = _mapper.Map<SaveSolicitudResource, Solicitud>(resource);

            var result = await _solicitudService.SaveAsync(solicitud);

            if (!result.Success)
                return BadRequest(new ErrorResource { ErrorMessage = result.Message});

            var solicitudResource = _mapper.Map<Solicitud, SolicitudResource>(result.Solicitud);
            return Ok(solicitudResource);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveSolicitudResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var solicitud = _mapper.Map<SaveSolicitudResource, Solicitud>(resource);
            var result = await _solicitudService.UpdateAsync(id, solicitud);

            if (!result.Success)
                return BadRequest(new ErrorResource { ErrorMessage = result.Message });

            var solicitudResource = _mapper.Map<Solicitud, SolicitudResource>(result.Solicitud);
            return Ok(solicitudResource);
        }
    }
}
