using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Auth.Domain.Entities;
using Auth.Application.Interface;

namespace Auth.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        public Token GenerateToken(User user)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);
			var SecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
                expires: expiration,
                claims: claims,
                signingCredentials: signingCredentials);

            return new Token()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(SecurityToken),
                Expiration = expiration,
                RefreshToken = GenerateRefreshToken(user)
            };
        }
        public bool VerifyToken(string? token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }
		public string GenerateRefreshToken(User user)
		{
			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
				SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		};

			var SecurityToken = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshMinutes),
				claims: claims,
				signingCredentials: signingCredentials);

			return new JwtSecurityTokenHandler().WriteToken(SecurityToken);
		}
		public bool VerifyRefreshToken(string refreshToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
				ValidateIssuer = true,
				ValidIssuer = _jwtSettings.Issuer,
				ValidateAudience = true,
				ValidAudience = _jwtSettings.Audience,
				ValidateLifetime = false, // On ne vérifie pas l'expiration pour le token de rafraîchissement
				ClockSkew = TimeSpan.Zero
			};

			try
			{
				tokenHandler.ValidateToken(refreshToken, validationParameters, out var validatedToken);

				// Vérifier que le token est bien un JWT et qu'il a l'algorithme de signature HMACSHA256
				if (!(validatedToken is JwtSecurityToken jwtSecurityToken) ||
					!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				{
					return false;
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

	}
}