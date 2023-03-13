using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Auth.Domain.Entities;
using Auth.Infrastructure.Authentication;
using AutoFixture;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Tests.Authentication
{
    public class JwtTokenGeneratorTests
    {
        private readonly JwtSettings _jwtSettings;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
		private readonly Fixture _fixture = new();

		public JwtTokenGeneratorTests()
        {
			_fixture = new Fixture();
			_jwtSettings = _fixture.Create<JwtSettings>();
			var jwtOptions = Options.Create(_jwtSettings);
			_jwtTokenGenerator = new JwtTokenGenerator(jwtOptions);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtToken()
        {
			// Arrange
			var user = _fixture.Create<User>();

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

            var claimsPrincipal = tokenHandler.ValidateToken(jwtToken.AccessToken, validationParameters, out var _);
            Assert.NotNull(claimsPrincipal);
        }
		[Fact]
		public void GenerateToken_ShouldReturnTokenWithExpiration()
		{
			// Arrange
			var user = _fixture.Create<User>();

			// Act
			var token = _jwtTokenGenerator.GenerateToken(user);

			// Assert
			var handler = new JwtSecurityTokenHandler();
			var decodedToken = handler.ReadJwtToken(token.AccessToken);
			Assert.NotNull(decodedToken);
			Assert.True(decodedToken.ValidTo > DateTime.UtcNow);
		}
		[Fact]
		public void GenerateToken_ShouldReturnTokenWithRefreshToken()
		{
			// Arrange
			var user = _fixture.Create<User>();

			// Act
			var token = _jwtTokenGenerator.GenerateToken(user);

			// Assert
			Assert.NotNull(token.RefreshToken);
		}
		[Fact]
		public void GenerateRefreshToken_Should_Return_RefreshToken()
		{
			// Arrange
			var user = _fixture.Create<User>();

			// Act
			var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

			// Assert
			Assert.NotNull(refreshToken);
		}
		[Fact]
		public void VerifyToken_WithValidToken_ShouldReturnTrue()
		{
			// Arrange
			var user = _fixture.Create<User>();

			var token = _jwtTokenGenerator.GenerateToken(user);

			// Act
			var isTokenValid = _jwtTokenGenerator.VerifyToken(token.AccessToken);

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
		[Fact]
		public void VerifyRefreshToken_Should_Return_True_When_Token_Is_Valid()
		{
			// Arrange
			var user = _fixture.Create<User>();
			var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

			// Act
			var isValid = _jwtTokenGenerator.VerifyRefreshToken(refreshToken);

			// Assert
			Assert.True(isValid);
		}

		[Fact]
		public void VerifyRefreshToken_Should_Return_False_When_Token_Is_Invalid()
		{
			// Arrange
			var invalidToken = "invalid_refresh_token";

			// Act
			var isValid = _jwtTokenGenerator.VerifyRefreshToken(invalidToken);

			// Assert
			Assert.False(isValid);
		}
	}
}
