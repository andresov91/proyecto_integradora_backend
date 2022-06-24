using kairosApp.Domain.Persistence.Contexts;
using kairosApp.Domain.Persistence.Repositories;
using kairosApp.Domain.Repositories;
using kairosApp.Domain.Services;
using kairosApp.Extensions;
using kairosApp.Models.Support;
using kairosApp.Models.Support.Mail;
using kairosApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kairosApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJWTTokenServices(Configuration);
            //EMAIL CONFIGURATION
            var emailConfig = Configuration
                    .GetSection("EmailConfiguration")
                    .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();
            //
            services.AddControllers();
            services.AddCors();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
            });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            //SERVICES
            services.AddScoped<IPersonaService, PersonaService>();
            services.AddScoped<ICuentaUsuarioService, CuentaUsuarioService>();
            services.AddScoped<IGrupoService, GrupoService>();
            services.AddScoped<ISolicitudService, SolicitudService>();
            services.AddScoped<IUsuarioGrupoService, UsuarioGrupoService>();
            services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();
            services.AddScoped<IRGService, RGService>();

            //REPOSITORIES
            services.AddScoped<IPersonaRepository, PersonaRepository>();
            services.AddScoped<ICuentaUsuarioRepository, CuentaUsuarioRepository>();
            services.AddScoped<IGrupoRepository, GrupoRepository>();
            services.AddScoped<IUsuarioGrupoRepository, UsuarioGrupoRepository>();
            services.AddScoped<ISolicitudRepository, SolicitudRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "kairosApp", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:3000");
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "kairosApp v1"));
            }

            app.UseRouting();
            
            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
