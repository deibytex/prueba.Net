using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Syscaf.Data.Helpers.Auth.DTOs;
using Syscaf.Data.Models.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.ApiCore.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration)
        {
            this._userManager = _userManager;
            this._configuration = _configuration;
        }
        public async Task<ResponseAccount> ConstruirToken(UsuarioDTO credenciales)
        {
            var claims = new List<Claim>()
            {
                new Claim("username", credenciales.UserName)
            };

            var usuario = await _userManager.FindByNameAsync(credenciales.UserName);
            var claimsDB = await _userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);
            if (usuario.usuarioIdS.HasValue)
                claims.Add(new Claim("usuarioIds", usuario.usuarioIdS.ToString()));
            claims.Add(new Claim("Id", usuario.Id));

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new ResponseAccount()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };
        }



    }

    public interface IAuthService
    {
        Task<ResponseAccount> ConstruirToken(UsuarioDTO credenciales);
    }
}
