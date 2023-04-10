                                                                       
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using Syscaf.ApiCore.ApiBehavior;
using Syscaf.ApiCore.Auth;
using Syscaf.ApiCore.Filters;
using Syscaf.ApiCore.Utilidades;
using Syscaf.ApiCore.ViewModels;
using Syscaf.Common.Helpers;
using Syscaf.Common.PORTAL;
using Syscaf.Data;
using Syscaf.Data.Helpers;

using Syscaf.Data.Models.Auth;
using Syscaf.PBIConn.Services;
using Syscaf.Service.Automaper;
using Syscaf.Service.eBus.Gcp;
using Syscaf.Service.Portal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.ApiCore
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

            services.AddCors(c => {

                var frontend_url = Configuration.GetValue<string>("frontend_url");
                c.AddDefaultPolicy(b => {
                    b.WithOrigins(frontend_url, $"{frontend_url}/").AllowAnyMethod().AllowAnyHeader().
                    WithExposedHeaders(new string[] { "totalregistros", "IS-TOKEN-EXPIRED" });

                });
            });

            services.AddOptions();
            // variables para ITS ebus
            services.Configure<PubsubOptions>(
                Configuration.GetSection("Pubsub"));
            // variables globales de la aplicacion 
            services.Configure<GlobalVariables>(
                Configuration.GetSection("Constants"));
            // variables de las credenciales de mix
            services.Configure<MixCredenciales>(
                Configuration.GetSection("MixCredenciales"));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //services.AddDbContext<SyscafBD>(options =>
            //              options.UseSqlServer(
            //                  Configuration.GetConnectionString("SyscafBDDWH")));
            services.AddDbContext<SyscafBDCore>(options =>
                          options.UseSqlServer(
                              Configuration.GetConnectionString("SyscafBDCore")));

            //Register dapper in scope    
            services.AddScoped<ISyscafConn>(options => new SyscafConn(Configuration.GetConnectionString("SyscafBDDWH")));
            services.AddScoped(options => new Data.SyscafCoreConn(Configuration.GetConnectionString("SyscafBDCore")));
            // configura todas las interfaces a utilizar en la aplicacion
            InterfacesAplication.ConfigureServices(services);

            services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {                
                    options.Password.RequiredLength = 6;
                }
        )
                .AddEntityFrameworkStores<SyscafBDCore>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(Configuration["llavejwt"])
                       ),
                        ClockSkew = TimeSpan.Zero
                    };
                  
                }
                );
            
            ;

            services.AddCors(options =>
             {
                 var frontendURL = Configuration.GetValue<string>("frontend_url");
                 options.AddDefaultPolicy(builder => { 
                  builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
                 });

            });

            //services.AddAuthorization(opciones =>
            //{
            //    opciones.AddPolicy("EsAdmin", policy => policy.RequireClaim("role", "admin"));
            //});


            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
                options.Filters.Add(typeof(ParserBadRequest));
            }).ConfigureApiBehaviorOptions(BehaviorBadRequests.Parsear);

            Constants.Inicializar(Configuration);
            ConfigValidatorService.Inicializar(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Syscaf ApiCore v1"));
            //}

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
