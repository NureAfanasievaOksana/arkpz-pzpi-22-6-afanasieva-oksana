using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoTSortGarbage.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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