using System.Net.Http.Headers;
using System.Net.Http.Json;
using Auth0.AspNetCore.Authentication;
using Newtonsoft.Json;
using ThreeAmigos.WebApp.Services;
using ThreeAmigos.WebApp.Models;
using Auth0.ManagementApi;
using Newtonsoft.Json.Linq;
using Auth0.ManagementApi.Models.Actions;
using Auth0.ManagementApi.Models.Grants;
using System.Text;

namespace ThreeAmigos.WebApp.Services;

public class UserService : IUserService{
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration){
        _configuration = configuration;
    }

    record managementTokenDto(string access_token, string token_type, int expires_in);

    public async Task<string> GetManagementApiTokenAsync() {
        
        var tokenClient = new HttpClient();
        var authBaseAddress = _configuration["Auth:Authority"];
        tokenClient.BaseAddress = new Uri(authBaseAddress);

        var response = await tokenClient.PostAsync("oauth/token", new FormUrlEncodedContent(
            new Dictionary<string, string>{
                {"client_id", _configuration["Auth:ClientId"]},
                {"client_secret", _configuration["Auth:ClientSecret"]},
                {"audience", _configuration["Auth:ProfileAudience"]},
                {"grant_type", "client_credentials"}
            }
        ));

        var content = await response.Content.ReadAsStringAsync();
        var jsonResult = JObject.Parse(content);

        var token = jsonResult["access_token"].Value<string>();
        var audience = _configuration["Auth:ProfileAudience"];
        return token;
    }

    public async Task<string> GetUserDataAsync(string userEmail){
        var managementApiToken = await GetManagementApiTokenAsync();

        var client = new HttpClient();
        var serviceBaseAddress = _configuration["Auth:ProfileAudience"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", managementApiToken); 

        var apiResponse = await client.GetAsync($"users-by-email?email={userEmail}");
        apiResponse.EnsureSuccessStatusCode();
        var result = await apiResponse.Content.ReadAsStringAsync();
        return result;
    }
    public async Task UpdateUserDetails(string userId, string userName, string billingAddress, string phoneNumber){
        var managementApiToken = await GetManagementApiTokenAsync();

        var client = new HttpClient();
        var serviceBaseAddress = _configuration["Auth:ProfileAudience"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", managementApiToken); 

        var newUserDetails = new {
            nickname = userName,
            user_metadata = new {
                billing_address = billingAddress,
                number = phoneNumber
            }
        };

        string jsonContent = JsonConvert.SerializeObject(newUserDetails);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var apiResponse = await client.PatchAsync($"users/{userId}", content);
        apiResponse.EnsureSuccessStatusCode();
        return;
    }

    public async Task DeleteUserProfile(string userId){
        var managementApiToken = await GetManagementApiTokenAsync();

        var client = new HttpClient();
        var serviceBaseAddress = _configuration["Auth:ProfileAudience"];
        client.BaseAddress = new Uri(serviceBaseAddress);
        client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", managementApiToken); 

        var apiResponse = await client.DeleteAsync($"users/{userId}");
        apiResponse.EnsureSuccessStatusCode();
        return;
    }
}