using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;
using Microsoft.IdentityModel.Tokens;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class TokenService : ITokenService
{
    
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    
    public TokenService(IConfiguration configuration)
    {
        _secretKey = configuration["TokenService:secretKey"];
        _issuer = configuration["TokenService:issuer"];
        _audience = configuration["TokenService:audience"];
    }
    
    public async Task<Result<ClaimsPrincipal, Err>> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);
        
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken || 
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {

                return Result<ClaimsPrincipal,Err>.Err(UtilErrors.InternalServerError(new Exception("Invalid token")));
            }

            return Result<ClaimsPrincipal,Err>.Ok(principal);
        }
        catch (Exception ex)
        {
            Console.WriteLine("loi 3");
            Console.WriteLine(ex.Message);
            return Result<ClaimsPrincipal, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<string,Err>> GenerateToken(string userEmail, int expireMinutes = 5)
    {
        try
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64),
                new Claim("user_email", userEmail),
                new Claim("exp",DateTimeOffset.Now.AddMinutes(expireMinutes).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64 ),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials);

            return Result<string, Err>.Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
        catch (Exception ex)
        {
            return Result<string,Err>.Err(UtilErrors.InternalServerError(ex));
        }
         
    }
}