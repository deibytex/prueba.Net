
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.PORTAL;
using Syscaf.Common.Services;
using Syscaf.Common.Utils;
using Syscaf.Data;
using Syscaf.Data.Helpers;
using Syscaf.Data.Interface;
using Syscaf.Service.Automaper;
using Syscaf.Service.Portal;
using Syscaf.Service.PORTAL;
using SyscafWebApi.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
           
            services.AddDbContext<SyscafBDCore>(options =>
                          options.UseSqlServer(
                              Configuration.GetConnectionString("SyscafBDCore")));

            //Register dapper in scope    
            services.AddScoped<ISyscafConn, SyscafConn>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IClientService, ClientService>();         
            services.AddTransient<IMixIntegrateService, MixIntegrateService>();
            services.AddTransient<IPortalService, PortalService>();        
            services.AddTransient<IAssetsService, AssetsService>();
            services.AddTransient<ITransmisionService, TransmisionService>();

            Constants.Inicializar(Configuration);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Syscaf Api DWH v1"));
            }

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
