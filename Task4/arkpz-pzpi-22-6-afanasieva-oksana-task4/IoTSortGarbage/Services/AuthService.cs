using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoTSortGarbage.Services
{
    /// <summary>
    /// Service for handling authentication with the API
    /// </summary>
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the AuthService class
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests</param>
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves a JWT token by authenticating with email and password
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <returns>The JWT token string</returns>
        public async Task<string> GetJwtTokenAsync(string email, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { Email = email, Password = password }), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("/auth/login", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error: {response.StatusCode}, Details: {errorDetails}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json)["token"];
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve JWT token", ex);
            }
        }
    }
}