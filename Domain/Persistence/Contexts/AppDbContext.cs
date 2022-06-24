using kairosApp.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace kairosApp.Domain.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Persona> Personas { get; set; }
        public DbSet<CuentaUsuario> CuentaUsuarios { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<UsuarioGrupo> UsuarioGrupos { get; set; }
        public DbSet<SolicitudPersona> SolicitudPersonas { get; set; }
             

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //PERSONA
            builder.Entity<Persona>().HasKey(p => p.Id);
            builder.Entity<Persona>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Persona>().Property(p => p.Nombres).IsRequired().HasMaxLength(30);
            builder.Entity<Persona>().Property(p => p.Apellidos).IsRequired().HasMaxLength(30);
            builder.Entity<Persona>().Property(p => p.Telefono).HasMaxLength(10);
            builder.Entity<Persona>().Property(p => p.Identificacion).IsRequired().HasMaxLength(10);
            builder.Entity<Persona>().Property(p => p.Unidad).HasMaxLength(20);
            builder.Entity<Persona>().Property(p => p.CorreoAlterno).HasMaxLength(30);
            builder.Entity<Persona>().Property(p => p.Rol).HasMaxLength(20);
            builder.Entity<Persona>().HasOne(p => p.CuentaUsuario).WithOne(p => p.Persona).HasForeignKey<CuentaUsuario>(p => p.PersonaId);
            builder.Entity<Persona>().HasMany(p => p.SolicitudPersonas).WithOne(p => p.Persona).HasForeignKey(p => p.PersonaId);

            builder.Entity<Persona>().HasData
            (
                new Persona { Id=100, Nombres="Carlos Fernando",Apellidos="Salazar Moreira",Telefono="0990964027",Identificacion="0940529829",Unidad="FIEC",CorreoAlterno="chardan1225@hotmail.com", Rol="Admin"},
                new Persona { Id = 101, Nombres = "Melanie Dayanna",Apellidos="Banchon Chavez",Telefono="0987654321",Identificacion="0925153212",Unidad="FIEC",CorreoAlterno="melanie1998@hotmail.com", Rol="Estudiante"},
                new Persona { Id = 102, Nombres = "Melissa Roxanna",Apellidos="Pachar Duche",Telefono="0978791998",Identificacion="09876543231",Unidad="Rectorado",CorreoAlterno="melissa_pachar@hotmail.com", Rol="Estudiante"}
                );
            //CUENTA USUARIO
            builder.Entity<CuentaUsuario>().HasKey(p => p.Id);
            builder.Entity<CuentaUsuario>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<CuentaUsuario>().Property(p => p.Username).IsRequired().HasMaxLength(10);
            builder.Entity<CuentaUsuario>().Property(p => p.IsActive).IsRequired();
            builder.Entity<CuentaUsuario>().Property(p => p.Alias).IsRequired().HasMaxLength(20);
            builder.Entity<CuentaUsuario>().HasMany(p => p.UsuarioGrupo).WithOne(p => p.CuentaUsuario).HasForeignKey(p => p.CuentaUsuarioId);
            builder.Entity<CuentaUsuario>().HasData
            (
                new CuentaUsuario { Id = 101, Username = "carfesal", Alias = "carlos.salazar", PersonaId = 100, IsActive = true},
                new CuentaUsuario { Id = 102, Username = "meldaban", Alias = "melanie.banchon", PersonaId = 101, IsActive = true },
                new CuentaUsuario { Id = 103, Username = "melroxan", Alias = "melissa.pachar", PersonaId = 102, IsActive = true }
                );
            //GRUPO
            builder.Entity<Grupo>().HasKey(p => p.Id);
            builder.Entity<Grupo>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Grupo>().Property(p => p.Nombre).IsRequired().HasMaxLength(20);
            builder.Entity<Grupo>().HasMany(p => p.UsuarioGrupos).WithOne(p => p.Grupo).HasForeignKey(p => p.GrupoId);
            builder.Entity<Grupo>().HasData
            (
                new Grupo { Id = 101, Nombre = "FIEC"},
                new Grupo { Id = 102, Nombre = "FCSH"},
                new Grupo { Id = 103, Nombre = "FCNM"},
                new Grupo { Id = 104, Nombre = "FCV"},
                new Grupo { Id = 105, Nombre = "FADCOM"},
                new Grupo { Id = 106, Nombre = "FIMCP"},
                new Grupo { Id = 107, Nombre = "FIMCBOR"},
                new Grupo { Id = 108, Nombre = "Rectorado"},
                new Grupo { Id = 109, Nombre = "Vinculos"},
                new Grupo { Id = 110, Nombre = "STEM"}
                );
            //SOLICITUD
            builder.Entity<Solicitud>().HasKey(p => p.Id);
            builder.Entity<Solicitud>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Solicitud>().Property(p => p.Estado).IsRequired().HasMaxLength(20);
            builder.Entity<Solicitud>().Property(p => p.FechaCreacion).IsRequired();
            builder.Entity<Solicitud>().Property(p => p.InfoSolicitud).IsRequired();
            //builder.Entity<Solicitud>().HasOne(p => p.SolicitudPersona).WithOne(p => p.Solicitud).HasForeignKey<SolicitudPersona>(p => p.SolicitudId);

            builder.Entity<Solicitud>().HasData
            (
                new Solicitud { Id = 101, Estado = "No Revisado", FechaCreacion = DateTime.Now, InfoSolicitud = JsonConvert.SerializeObject(new InfoSolicitud { nombres="Fernando Carlos", apellidos="Moreira Salazar",identificacion="0912654798", actividad="Estudiante", correo="fernandomoreira@gmail.com", telefono="0997063143", unidad="FIEC", alias_sugerido="fernando.moreira", usuario_sugerido="fermorsa"})},
                new Solicitud { Id = 102, Estado = "No Revisado", FechaCreacion = DateTime.Now, InfoSolicitud = JsonConvert.SerializeObject(new InfoSolicitud { nombres="Rosa Cristina", apellidos="Alvarado Castillo",identificacion="0914365487", actividad="Estudiante", correo="rosita_alvarado@gmail.com", telefono="0991193877", unidad="FIMCP", alias_sugerido="rosa.cristina", usuario_sugerido="rosaalva"})}
                );
            //UsuarioGrupo
            builder.Entity<UsuarioGrupo>().HasKey(p => p.Id);
            builder.Entity<UsuarioGrupo>().HasOne(p => p.CuentaUsuario).WithMany(p => p.UsuarioGrupo).HasForeignKey(p => p.CuentaUsuarioId);
            builder.Entity<UsuarioGrupo>().HasOne(p => p.Grupo).WithMany(p => p.UsuarioGrupos).HasForeignKey(p => p.GrupoId);
            builder.Entity<UsuarioGrupo>().HasData
                (
                new UsuarioGrupo { Id = 100, CuentaUsuarioId = 101, GrupoId = 101},
                new UsuarioGrupo { Id = 101, CuentaUsuarioId = 102, GrupoId = 103},
                new UsuarioGrupo { Id = 102, CuentaUsuarioId = 102, GrupoId = 104},
                new UsuarioGrupo { Id = 103, CuentaUsuarioId = 103, GrupoId = 101},
                new UsuarioGrupo { Id = 104, CuentaUsuarioId = 103, GrupoId = 110},
                new UsuarioGrupo { Id = 105, CuentaUsuarioId = 102, GrupoId = 109}
                );
            //Solicitud Persona
            builder.Entity<SolicitudPersona>().HasKey(p => p.Id);
            builder.Entity<SolicitudPersona>().HasOne(p => p.Persona).WithMany(p => p.SolicitudPersonas).HasForeignKey(p => p.PersonaId);
            builder.Entity<SolicitudPersona>().HasOne(p => p.Solicitud).WithOne(p => p.SolicitudPersona).HasForeignKey<SolicitudPersona>(p => p.SolicitudId);



        }
    }
}

