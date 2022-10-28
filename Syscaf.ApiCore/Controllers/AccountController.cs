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
using Syscaf.Data.Models.Auth;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Syscaf.Service.Helpers;
using Syscaf.Service.Auth;

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
        private readonly IUsuarioService _usuarioService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration _configuration,
            SyscafBDCore _ctx,
            ISyscafConn _ctxDwh,
            IMapper mapper, IAuthService _authService, IUsuarioService _usuarioService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._configuration = _configuration;
            this._ctx = _ctx;
            this._ctxDwh = _ctxDwh;
            this._mapper = mapper;
            this._authService = _authService;
            this._usuarioService = _usuarioService;
        }

        [HttpPost("Crear")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<dynamic>> Crear([FromBody] UsuarioDTO usuarioModel)
        {
            // verifica que exista el usuario y manda un badrequest


            var usuario = _mapper.Map<ApplicationUser>(usuarioModel);
            usuario.Id = Guid.NewGuid().ToString();
            IdentityResult resultado;

            usuario.UserName = usuarioModel.Email;

            if (usuarioModel.Password != null)
                resultado = await userManager.CreateAsync(usuario, usuarioModel.Password);
            else
                resultado = await userManager.CreateAsync(usuario);



            if (resultado.Succeeded)
            {

                var token = await userManager.GeneratePasswordResetTokenAsync(usuario);
                return new ResultObject()
                {
                    Exitoso = true,
                    Data = new { token, username= usuarioModel.Email, nombres = usuario.Nombres }
                };
            }
            else
                return BadRequest(resultado.Errors);


        }


        [HttpPost("ResetPassWord")]
        public async Task<ActionResult<dynamic>> ResetPassWord([FromBody] ResetPassWord usuarioModel)
        {
            // verifica que exista el usuario y manda un badrequest
            ResultObject result = new();
            var isfind = await userManager.FindByNameAsync(usuarioModel.UserName.ToUpper().Trim());
            if (isfind != null)
            {
                var resultado = await userManager.ResetPasswordAsync(isfind, usuarioModel.token, usuarioModel.NewPassword);

                result.Exitoso = resultado.Succeeded;
                if (resultado.Succeeded)
                {
                    if (usuarioModel.EmailConfirm)
                    {

                        var token = await userManager.GenerateEmailConfirmationTokenAsync(isfind);
                        var resultIdentity = await userManager.ConfirmEmailAsync(isfind, token);

                        ////LOGUEAR TODO
                    }

                }
                else
                    result.Mensaje = resultado.Errors.Select(s => getMensajeByCode(s.Code, s.Description)).Aggregate((a, b) => $"{a},{b}");
                return result;

            }

            return BadRequest($" Username: {usuarioModel.UserName} No existe ");
        }

        private string getMensajeByCode(string Code, string Description) {
           
            switch (Code) {
                case "InvalidToken":
                    Description = "Token ha expirado, favor vuelva a la opción de Recuperar contraseña.";
                    break;
                case "PasswordRequiresLower":
                    Description = "Password al menos debe contener una letra minúscula.";
                    break;
                case "PasswordRequiresUpper":
                    Description = "Password al menos debe contener una letra Mayúscula.";
                    
                    break;
                case "PasswordTooShort":
                    Description = "Password debe ser al menos 6 caracteres.";
                    break;
                case "PasswordRequiresDigit":
                    Description = "Password al menos debe contener un número";                    
                    break;
                case "PasswordRequiresNonAlphanumeric":
                    Description = "Password debe contener al menos un caracter no alphanumérico.";
                    break;
            }

            return Description;
        }

        [HttpPost("GetTokenRecuperarContrasenia")]
        public async Task<ActionResult<dynamic>> GetTokenPassword([FromBody] string correo)
        {
            // verifica que exista el usuario y manda un badrequest

            var isfind = await userManager.FindByEmailAsync(correo.ToLower());
            if (isfind != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(isfind);
                return new { token, isfind.UserName, isfind.Nombres };
            }

            return BadRequest($" Correo: {correo} No existe ");
        }
      
      

        [HttpGet("listadoUsuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<dynamic>> ListadoUsuarios([FromQuery] PaginacionDTO paginacionDTO, [FromQuery] string? Search, [FromQuery] string? UsuarioId, [FromQuery] int? UsuarioIds)
        {
            try
            {
                var queryable = _ctx.Users.AsQueryable().Where(w => (UsuarioId == null || w.Id == UsuarioId) && (UsuarioIds == null || w.usuarioIdS == UsuarioIds));

                if (Search != null)
                {
                    queryable = (from e in queryable
                                 where (e.Nombres.ToLower().Contains(Search.ToLower())
                              || e.Email.ToLower().Contains(Search.ToLower()))
                                 select e);
                }
                await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
                var usuarios = await queryable.OrderBy(x => x.Email).Paginar(paginacionDTO).ToListAsync();
                return usuarios;
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        [HttpGet("GetAspnetUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<dynamic>>> GetAspnetUsers(string? UserId, int? PerfilId)
        {
            try
            {
                return await _usuarioService.GetUsuarios(UserId, PerfilId);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        [HttpGet("GetMenuUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<dynamic>>> GetMenuUsuario(string UserId)
        {
            try
            {
                var MenuDesagregadoDTO = await _usuarioService.GetMenuUsuario(UserId);

                return MenuDesagregadoDTO.GroupBy(g => new
                {
                    g.UserName,
                    g.NombreOpcion,
                    g.Accion,
                    g.Controlador,
                    g.Logo,
                    g.EsVisible,
                    g.OpcionId,
                    g.OpcionPadreId,
                    g.Orden,
                    g.ParametrosAdicionales

                }).Select(
                    s => new
                    {
                        s.Key.UserName,
                        s.Key.NombreOpcion,
                        s.Key.Accion,
                        s.Key.Controlador,
                        s.Key.Logo,
                        s.Key.EsVisible,
                        s.Key.OpcionId,
                        s.Key.OpcionPadreId,
                        s.Key.Orden,
                        s.Key.ParametrosAdicionales,
                        lstOperacion = s.Select(s => new { s.NombreOperacion, s.Operacion })
                    }
                    ).ToList<dynamic>();


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }


        [HttpPost("login")]
        public async Task<ActionResult<ResponseAccount>> Login([FromBody] UsuarioDTO credenciales)
        {
            var resultado = await signInManager.PasswordSignInAsync(credenciales.UserName, credenciales.Password,
                isPersistent: false, lockoutOnFailure: true);

            if (resultado.Succeeded)
            {
                return await _authService.ConstruirToken(credenciales);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }


        [HttpPost("CambiarEstadoUsuario")]
        public async Task<ActionResult<bool>> CambiarEstadoUsuario([FromBody] string id)
        {
            var isfind = await userManager.FindByIdAsync(id);
            if (isfind != null)
            {
                var resultado = await userManager.SetLockoutEnabledAsync(isfind, !isfind.LockoutEnabled);
                return resultado.Succeeded;
            }
            return BadRequest("Usuario no encontrado en el sistema");

        }



        [HttpGet("GetUserId")]
        public async Task<ActionResult<string>> GetUserId([FromQuery] string username)
        {
            var isfind = await userManager.FindByNameAsync(username.ToUpper().Trim());
            if (isfind != null)
            {
                var claims = await userManager.GetClaimsAsync(isfind);


                return claims.Where(w => w.Type.Equals("UsuarioID")).FirstOrDefault().Value;
            }
            return BadRequest("Usuario no encontrado en el sistema");
        }

     
    }
}
