using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Syscaf.Common.Helpers;
using Syscaf.Data.Helpers.Auth.DTOs;
using Syscaf.Data.Models.Auth;
using Syscaf.Service.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.ApiCore.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;
        public AuthService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration, IUsuarioService _usuarioService)
        {
            this._userManager = _userManager;
            this._configuration = _configuration;
            this._usuarioService = _usuarioService;
        }
        public async Task<ResponseAccount> ConstruirToken(UsuarioDTO credenciales)
        {
            var usuario = await _userManager.FindByNameAsync(credenciales.UserName);
            var claimsDB = await _userManager.GetClaimsAsync(usuario);      
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, credenciales.UserName),
                new Claim("username", credenciales.UserName),
                new Claim("Id", usuario.Id),
                new Claim("Nombres", usuario.Nombres),
                new Claim("email", usuario.Email)

            };
            claims.AddRange(claimsDB);
            
            
            if (usuario.usuarioIdS.HasValue)
                claims.Add(new Claim("usuarioIds", usuario.usuarioIdS.ToString()));


            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new ResponseAccount()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion,
                RefreshToken = GenerateRefreshToken()
            };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string keyConfiguration)
        {
            var Key = Encoding.UTF8.GetBytes(keyConfiguration);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }


            return principal;
        }

    }


    public interface IAuthService
    {
        Task<ResponseAccount> ConstruirToken(UsuarioDTO credenciales);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string keyConfiguration);
    }
}
