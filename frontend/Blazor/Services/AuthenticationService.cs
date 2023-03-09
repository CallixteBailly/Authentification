using System.Net.Http;
using System.Net.Http.Headers;
using Blazor.Configuration;
using Blazor.Models;

namespace Blazor.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResult?> AuthenticatedAsync(LoginRequest loginRequest);
        Task<bool> IsAuthenticatedAsync(string token);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly UserState _userState;

        public AuthenticationService(UserState userState, IConfiguration configuration)
        {
            var authenticationApi = configuration.GetValue<string>("AuthenticationAPI") ?? string.Empty;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(authenticationApi)
            } ?? throw new ArgumentNullException(nameof(_httpClient));
            _userState = userState ?? throw new ArgumentNullException(nameof(userState));
        }

		public async Task<LoginResult?> AuthenticatedAsync(LoginRequest loginRequest)
		{
			if (loginRequest == null)
				throw new ArgumentNullException(nameof(loginRequest));

			var response = await _httpClient.PostAsJsonAsync("authentication/login", loginRequest);

            var loginResult = new LoginResult();

			if (response.IsSuccessStatusCode)
			{
				loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
				(_userState.Token, _userState.FirstName, _userState.LastName) = (loginResult?.Token?.AccessToken, loginResult?.FirstName, loginResult?.LastName);

				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userState.Token);
			}
			else
			{
				loginResult.Errors = await response.Content.ReadFromJsonAsync<ErrorsResponse>();
			}

			return loginResult;
		}

		public async Task<bool> IsAuthenticatedAsync(string token)
        {
            var response = await _httpClient.PostAsJsonAsync("authentication/verifyToken", new VerifyTokenRequest(token));
            if (response.IsSuccessStatusCode)
            {
                var loginResult = await response.Content.ReadFromJsonAsync<bool>();
                _userState.IsAuthenticated = loginResult;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return loginResult;
            }

            return false;
        }
    }
}
