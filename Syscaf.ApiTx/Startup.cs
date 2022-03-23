
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
using Syscaf.ApiTx.ViewModels;
using Syscaf.Data;
using Syscaf.Data.Helpers;
using Syscaf.Data.Interface;
using Syscaf.Service.eBus.Gcp;
using Syscaf.WebApiCore.ApiBehavior;
using Syscaf.WebApiCore.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.ApiTx
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
            services.AddControllers();
            services.AddOptions();
            services.Configure<PubsubOptions>(
                Configuration.GetSection("Pubsub"));
            services.Configure<GlobalVariables>(
                Configuration.GetSection("Constants"));

            services.AddDbContext<SyscafBD>(options =>
                          options.UseSqlServer(
                              Configuration.GetConnectionString("SyscafBDDWH")));
            services.AddDbContext<SyscafBDCore>(options =>
                          options.UseSqlServer(
                              Configuration.GetConnectionString("SyscafBDCore")));

            //Register dapper in scope    
            services.AddSingleton<ISyscafConn, SyscafConn>();          
            services.AddSingleton<IeBusGcpService, eBusGcpService>();



            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<SyscafBD>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options => options.TokenValidationParameters = new TokenValidationParameters { 
                 ValidateIssuer = false,
                 ValidateAudience = false,
                 ValidateLifetime= true,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(Configuration["llavejwt"])
                     ),
                 ClockSkew = TimeSpan.Zero
                }
                );

            //services.AddAuthorization(opciones =>
            //{
            //    opciones.AddPolicy("EsAdmin", policy => policy.RequireClaim("role", "admin"));
            //});


            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
                options.Filters.Add(typeof(ParserBadRequest));
            }).ConfigureApiBehaviorOptions(BehaviorBadRequests.Parsear);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(/*c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PeliculasAPI v1")*/);
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
