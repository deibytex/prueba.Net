
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Syscaf.Api.DWH.ApiBehavior;
using Syscaf.Api.DWH.Filters;
using Syscaf.Api.DWH.Utilities;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.PORTAL;
using Syscaf.Common.Services;
using Syscaf.Common.Utils;
using Syscaf.Data;
using Syscaf.Data.Helpers;
using Syscaf.Data.Models.Auth;
using Syscaf.PBIConn.Services;
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
            services.AddCors(c => {

                var frontend_url = Configuration.GetValue<string>("frontend_url");
                c.AddDefaultPolicy(b => {
                    b.WithOrigins(frontend_url).AllowAnyMethod().AllowAnyHeader();

                });
            });
            services.AddOptions();           
            // variables de las credenciales de mix
            services.Configure<MixCredenciales>(
                Configuration.GetSection("MixCredenciales"));

            services.Configure<GlobalVariables>(
                Configuration.GetSection("Constants"));

            services.Configure<PegVariablesConn>(
               Configuration.GetSection("PegVariablesConn"));

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<SyscafBDCore>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SyscafBDCore")));


            //Register dapper in scope    
            services.AddScoped<ISyscafConn>(options => new SyscafConn(Configuration.GetConnectionString("SyscafBDDWH")));
            services.AddScoped( options => new Data.SyscafCoreConn(Configuration.GetConnectionString("SyscafBDCore")));


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


            //Para documentacion de swagger
            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            Constants.Inicializar(Configuration);
            ConfigValidatorService.Inicializar(Configuration);

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
                options.Filters.Add(typeof(ParserBadRequest));
            }).ConfigureApiBehaviorOptions(BehaviorBadRequests.Parsear);



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

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

       

      

     
    }
}
