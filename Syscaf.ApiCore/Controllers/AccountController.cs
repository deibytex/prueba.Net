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
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            var resultado = await signInManager.PasswordSignInAsync(credenciales.UserName, credenciales.Password,
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
                string testString  = Decrypt(item.contrasena, item.key, item.IV).TrimEnd("\a".ToCharArray()).TrimEnd("\u000f".ToCharArray()).TrimEnd("\u0006".ToCharArray()).TrimEnd("\b".ToCharArray()).TrimEnd("\u0005".ToCharArray());
                string password =  Regex.Replace(testString, @"[^\t\r\n -~]", "");
                var usuariomodel = _mapper.Map<UsuarioDTO>(usuario);
               
                var isfind = await userManager.FindByNameAsync(item.usuario.ToUpper().Trim());
                if (isfind != null)
                {
                    var claim = await userManager.GetClaimsAsync(isfind);

                    var claimid = claim.Where(w => w.Type.Equals("Password")).FirstOrDefault();
                    await  userManager.RemovePasswordAsync(isfind);

                    var isActualizado =await userManager.AddPasswordAsync(isfind, password);
                  

                    isfind.Nombres = $"{item.nombre} {item.apellido}";
                    isfind.PerfilId = item.perfilIdS;
                    isfind.ClienteId = -1;
                    isfind.UserName = item.usuario;
                    isfind.usuarioIdS = item.usuarioIdS;
                    isfind.esMigrado = isActualizado.Succeeded;
                    await userManager.UpdateAsync(isfind);

                  
                    await userManager.ReplaceClaimAsync(isfind, claimid, new Claim("Password", password));
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

        [HttpGet("GetUserId")]        
        public async Task<ActionResult<string>> GetUserId([FromQuery] string username)
        {
            var isfind = await userManager.FindByNameAsync(username.ToUpper().Trim());
            if (isfind != null)
            {
                var claims =  await userManager.GetClaimsAsync(isfind);


                return claims.Where(w => w.Type.Equals("UsuarioID")).FirstOrDefault().Value;
            }
            return null ;
        }

        private  string Decrypt(byte[] encryptedText, byte[] SaltKey, byte[] VIKey)
        {
            byte[] cipherTextBytes = encryptedText;
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
            var decryptor = symmetricKey.CreateDecryptor(SaltKey, VIKey);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}
