using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cosmonaut.Extensions;
using Game.Data;
using Game.Domain;
using Game.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Game.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _dataConext;
        public IdentityService(UserManager<IdentityUser> usermanager, JwtSettings jwtSettings, DataContext dataConext)
        {
            _usermanager = usermanager;
            _jwtSettings = jwtSettings;
            _dataConext = dataConext;
        }

        public async Task<AuthenticationResult> LoginAsync(string username, string password)
        {
            var user = await _usermanager.FindByNameAsync(username);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "This account doesn't exists" }
                };
            }

            var userHasValidPassword = await _usermanager.CheckPasswordAsync(user, password);

            if(!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Wrong username or password" }
                };
            }
            return await GenereteAutenticationForUser(user);

        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var ValidatedToken = GetPrincipalFromToken(token);

            if(ValidatedToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "Bad Token" } };
            }

            var expiryDate = long.Parse(ValidatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDate);

            if(expiryUtc > DateTime.UtcNow) return new AuthenticationResult { Errors = new[] { "Token is ok" } };

            var jti = ValidatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _dataConext.RefreshToken.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if(storedRefreshToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "This token doesnt exists" } };
            }
            if(DateTime.UtcNow > storedRefreshToken.ExpDate)
            {
                return new AuthenticationResult { Errors = new[] { "This token is no longer valid" } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult { Errors = new[] { "This token is wrong" } };
            }
            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult { Errors = new[] { "This token was used" } };
            }
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { Errors = new[] { "nie pasuje" } };
            }

            storedRefreshToken.Used = true;
            _dataConext.RefreshToken.Update(storedRefreshToken);
            await _dataConext.SaveChangesAsync();

            var user = await _usermanager.FindByIdAsync(ValidatedToken.Claims.Single(x => x.Type == "id").Value);
            return await GenereteAutenticationForUser(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtValidate(validatedToken))
                {
                    return null;
                }
                else{
                    return principal;
                }
            }
            catch
            {
                return null;
            }
        } 

        private bool IsJwtValidate(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticationResult> RegisterAsync(string username, string password)
        {
            var existingUser = await _usermanager.FindByNameAsync(username);  

            if(existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this username already exists" }
                };
            }
            var newUserId = Guid.NewGuid();
            var newUser = new IdentityUser
            {
                Id = newUserId.ToString(),
                UserName = username
            };
            

            

            var createdUser = await _usermanager.CreateAsync(newUser, password);
          


            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            await _usermanager.AddClaimAsync(newUser, new Claim("tags.view", "true"));
            await _usermanager.AddToRoleAsync(newUser, "Casual");
            return await GenereteAutenticationForUser(newUser);
           

        }

        async Task<AuthenticationResult> GenereteAutenticationForUser(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                  new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim("id",user.Id )
            };
            var userClaims = await _usermanager.GetClaimsAsync(user);
            claims.AddRange(userClaims);


            var userRoles = await _usermanager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _usermanager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _usermanager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpDate = DateTime.UtcNow.AddHours(2)


            };

            await _dataConext.RefreshToken.AddAsync(refreshToken);
            await _dataConext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }
    }
}
