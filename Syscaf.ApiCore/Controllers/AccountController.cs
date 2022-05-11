using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Syscaf.Data;
using Syscaf.Data.Helpers.Auth.DTOs;
using Syscaf.ApiCore.Auth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Syscaf.ApiCore.DTOs;
using Syscaf.ApiCore.Utilidades;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Syscaf.Data.Helpers.Auth;
using Syscaf.Data.Interface;
using Syscaf.Data.Models.Auth;
using System.Data;

namespace Syscaf.ApiCore.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration _configuration;
        private readonly SyscafBDCore _ctx;
        private readonly ISyscafConn _ctxDwh;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration _configuration,
            SyscafBDCore _ctx,
            ISyscafConn _ctxDwh,
            IMapper mapper, IAuthService _authService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._configuration = _configuration;
            this._ctx = _ctx;
            this._ctxDwh = _ctxDwh;
            this._mapper = mapper;
            this._authService = _authService;
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<ResponseAccount>> Crear([FromBody] UsuarioDTO usuarioModel) 
        {
            // verifica que exista el usuario y manda un badrequest


            var usuario = _mapper.Map<ApplicationUser>(usuarioModel); 
            var resultado = await userManager.CreateAsync(usuario, usuarioModel.Password);
         
           
            if (resultado.Succeeded)
            {
                //// adicionamos los claims necesarios de la inforamcion basica del cliente
                // await userManager.AddClaimAsync(usuario, new Claim(AuthConstans.Perfil, usuarioModel.PerfilId.ToString()));                
                // await userManager.AddClaimAsync(usuario, new Claim(AuthConstans.Cliente, usuarioModel.ClienteId.ToString()));

                return await _authService.ConstruirToken(usuarioModel);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }

        }

        [HttpGet("listadoUsuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<UsuarioDTO>>> ListadoUsuarios([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = _ctx.Users.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var usuarios = await queryable.OrderBy(x => x.Email).Paginar(paginacionDTO).ToListAsync();
            return _mapper.Map<List<UsuarioDTO>>(usuarios);
        }


        [HttpPost("login")]
        public async Task<ActionResult<ResponseAccount>> Login([FromBody] UsuarioDTO credenciales)
        {
            var resultado = await signInManager.PasswordSignInAsync(credenciales.Email, credenciales.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await _authService.ConstruirToken(credenciales);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }



        [HttpGet("migrarUsuarios")]
       
        public async Task<ActionResult<string>> MigracionUsuarios()
        {
         
            var result = await Task.FromResult(_ctxDwh.GetAll<Usuario>(QueryHelper._QUsuariosAll, null, commandType: CommandType.Text));

            foreach (var item in result) {
               
                var usuario = new ApplicationUser { UserName = item.correo, Email = item.correo, Nombres = $"{item.nombre} {item.apellido}", PerfilId = item.perfilIdS };
                string password = $"M_{item.correo.Split('@')[0]}.{new Random().Next(999)}";
               
                var usuariomodel = _mapper.Map<UsuarioDTO>(usuario);
               
                var isfind = await userManager.FindByNameAsync(item.correo.ToUpper().Trim());
                if (isfind != null)
                {
                    isfind.Nombres = $"{item.nombre} {item.apellido}";
                    isfind.PerfilId = item.perfilIdS;
                    isfind.ClienteId = -1;
                    await userManager.UpdateAsync(isfind);
                }
                else
                {
                    var resultado = await userManager.CreateAsync(usuario, password);


                    if (resultado.Succeeded)
                    {
                        // adicionamos los claims necesarios de la inforamcion basica del cliente

                        await userManager.AddClaimAsync(usuario, new Claim("Password", password));
                        await userManager.AddClaimAsync(usuario, new Claim("UsuarioID", item.usuarioIdS.ToString()));

                        await _authService.ConstruirToken(usuariomodel);
                    }
                }

            }
         
            return "ok";
        }
    }
}
