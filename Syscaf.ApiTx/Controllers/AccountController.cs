using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Syscaf.Data;
using Syscaf.Data.Helpers.Auth.DTOs;
using Syscaf.WebApiCore.Auth;
using System.Threading.Tasks;

namespace Syscaf.WebApiCore.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration _configuration;
        private readonly SyscafBDCore _ctx;
        private readonly IMapper mapper;
        private readonly IAuthService _authService;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration _configuration,
            SyscafBDCore _ctx,
            IMapper mapper, IAuthService _authService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._configuration = _configuration;
            this._ctx = _ctx;
            this.mapper = mapper;
            this._authService = _authService;
        }

        [HttpPost("Crear")]
        public async Task<ActionResult<ResponseAccount>> Crear([FromBody] UsuarioDTO usuarioModel) 
        {
            var usuario = new IdentityUser { UserName = usuarioModel.Email, Email = usuarioModel.Email };
            var resultado = await userManager.CreateAsync(usuario, usuarioModel.Password);

            if (resultado.Succeeded)
            {
                return await _authService.ConstruirToken(usuarioModel);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }

        }
    }
}
