using AutoMapper;
using kairosApp.Models;
using kairosApp.Resources;

namespace kairosApp.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SavePersonaResource, Persona>();
            CreateMap<SaveCuentaUsuarioResource, CuentaUsuario>();
            CreateMap<SaveGrupoResource, Grupo>();
            CreateMap<SaveSolicitudResource, Solicitud>();
            CreateMap<SaveUsuarioGrupoResource, UsuarioGrupo>();
        }
    }
}
