using AutoMapper;
using kairosApp.Models;
using kairosApp.Resources;

namespace kairosApp.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Persona, PersonaResource>();
            CreateMap<CuentaUsuario, CuentaUsuarioResource>();
            CreateMap<Grupo, GrupoResource>();
            CreateMap<Solicitud, SolicitudResource>();
            CreateMap<UsuarioGrupo, UsuarioGrupoResource>();
        }
    }
}
