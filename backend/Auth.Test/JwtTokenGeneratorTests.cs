using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Domain.Entities;
using Auth.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;

namespace Auth.Infrastructure.Tests.Authentication
{
    public class JwtTokenGeneratorTests
    {
        private readonly Mock<IOptions<JwtSettings>> _jwtOptionsMock;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public JwtTokenGeneratorTests()
        {
            _jwtSettings = new JwtSettings
            {
                Secret = "MySuperSecretKey",
                Audience = "MyAudience",
                Issuer = "MyIssuer",
                ExpiryMinutes = 30
            };
            _jwtOptionsMock = new Mock<IOptions<JwtSettings>>();
            _jwtOptionsMock.Setup(jwtOptions => jwtOptions.Value).Returns(_jwtSettings);

            _jwtTokenGenerator = new JwtTokenGenerator(_jwtOptionsMock.Object);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtToken()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var jwtToken = _jwtTokenGenerator.GenerateToken(user);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var _);
            Assert.NotNull(claimsPrincipal);
        }

        [Fact]
        public void VerifyToken_WithValidToken_ShouldReturnTrue()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe"
            };
            var jwtToken = _jwtTokenGenerator.GenerateToken(user);

            // Act
            var isTokenValid = _jwtTokenGenerator.VerifyToken(jwtToken);

            // Assert
            Assert.True(isTokenValid);
        }

        [Fact]
        public void VerifyToken_WithInvalidToken_ShouldReturnFalse()
        {
            // Arrange
            var jwtToken = "invalid token";

            // Act
            var isTokenValid = _jwtTokenGenerator.VerifyToken(jwtToken);

            // Assert
            Assert.False(isTokenValid);
        }
    }
}
