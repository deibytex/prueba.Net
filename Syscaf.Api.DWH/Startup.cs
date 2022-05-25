
using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Syscaf.Api.DWH.Utilities;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.PORTAL;
using Syscaf.Common.Services;
using Syscaf.Common.Utils;
using Syscaf.Data;
using Syscaf.Data.Helpers;

using Syscaf.Service.Automaper;
using Syscaf.Service.Portal;

using SyscafWebApi.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Syscaf.Api.DWH
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
           
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton(provider =>
                new MapperConfiguration(config =>
                {
                    config.AddProfile(new AutoMapperProfiles());
                }).CreateMapper());

            services.AddControllers();
            services.AddOptions();           
            // variables de las credenciales de mix
            services.Configure<MixCredenciales>(
                Configuration.GetSection("MixCredenciales"));

            services.Configure<GlobalVariables>(
                Configuration.GetSection("Constants"));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
           
            

            //Register dapper in scope    
            services.AddScoped<ISyscafConn>(options => new SyscafConn(Configuration.GetConnectionString("SyscafBDDWH")));
            services.AddScoped( options => new Data.SyscafCoreConn(Configuration.GetConnectionString("SyscafBDCore")));


            // configura todas las interfaces a utilizar en la aplicacion
            InterfacesAplication.ConfigureServices(services);
            //Para documentacion de swagger
            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            Constants.Inicializar(Configuration);
        }
       
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Informacion de configuracion
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Syscaf Api DWH v1"));
                
           // }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

       

      

     
    }
}
